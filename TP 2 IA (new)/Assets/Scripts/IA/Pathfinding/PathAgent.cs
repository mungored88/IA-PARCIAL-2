using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAgent : MonoBehaviour
{
    public LayerMask mask;
    public PathNode init;
    public PathNode finit;
    public PathNode lastNode { get; private set; }
    //public Transform owner;

    List<PathNode> _list;
    Theta<PathNode> _theta = new Theta<PathNode>();


    public void SetNewPath(PathNode init, PathNode finit, LayerMask mask)
    {
        this.init = init;
        this.finit = finit;
        this.mask = mask;
    }

    public List<PathNode> PathFindingTheta()
    {
        return _list = _theta.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic, InSight);
    }
    bool Satisfies(PathNode curr)
    {
        return curr == finit;
    }
    List<PathNode> GetNeighbours(PathNode curr)
    {
        return curr.neightbourds;
    }
    float GetCost(PathNode from, PathNode to)
    {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }
    float Heuristic(PathNode curr)
    {
        return Vector3.Distance(curr.transform.position, finit.transform.position);
    }
    bool InSight(PathNode gP, PathNode gC)
    {
        var dir = gC.transform.position - gP.transform.position;
        if (Physics.Raycast(gP.transform.position, dir.normalized, dir.magnitude, mask))
            return false;

        return true;
    }
    public PathNode FindNearbyNode(float range = 1f)
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, range);
        lastNode = null;
        foreach (Collider coll in colls)
        {
            if (coll.GetComponent<PathNode>())
            {
                lastNode = coll.GetComponent<PathNode>();
            }
        }

        if(!lastNode)
            FindNearbyNode(range * 2f);

        return lastNode;
    }
    #region Gizmos
    public Vector3 offset;
    public float radius;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (init != null)
            Gizmos.DrawSphere(init.transform.position + offset, radius);
        if (finit != null)
            Gizmos.DrawSphere(finit.transform.position + offset, radius);
        if (_list != null)
        {
            Gizmos.color = Color.blue;
            foreach (var item in _list)
            {
                if (item != init && item != finit)
                    Gizmos.DrawSphere(item.transform.position + offset, radius);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    #endregion
}
