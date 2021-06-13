using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public float range;
    public List<PathNode> neightbourds;
    public bool hasTrap;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        GetNeightbourd(Vector3.right);
        GetNeightbourd(Vector3.left);
        GetNeightbourd(Vector3.forward);
        GetNeightbourd(Vector3.back);
    }
    
    void GetNeightbourd(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, range))
        {
            var node = hit.collider.GetComponent<PathNode>();
            if (node != null)
                neightbourds.Add(node);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}
