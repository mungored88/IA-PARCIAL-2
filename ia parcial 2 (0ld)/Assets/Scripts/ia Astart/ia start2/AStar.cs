using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    private List<PathNode> openSet;
    private List<PathNode> closedSet;

    public AStar()
    {
        openSet = new List<PathNode>();
        closedSet = new List<PathNode>();
    }

    public PathNode[]SearchPath(PathNode start, PathNode end)
    {
        PathNode[] resetNodes = GameObject.FindObjectsOfType<PathNode>();
        foreach (var node in resetNodes)
        {
            node.isStart = false;
            node.isEnd = false;
            node.isPath = false;
            node.index = 0;
        }

        start.isStart = true;
        end.isEnd = true;

        openSet.Clear();
        closedSet.Clear();

        openSet.Add(start);

        PathNode currentNode;
        while (openSet.Count > 0)
        {
            currentNode = SearchForLowestFValue();
            if (currentNode == end)
                 return ThetaStar(RebuildPath(start, currentNode));

            var nodeNeighbours = currentNode.neighbors;

            for (int i = 0; i < nodeNeighbours.Count; i++)
            {
                if (closedSet.Contains(nodeNeighbours[i]) || nodeNeighbours[i].isBlocked)
                {
                    closedSet.Add(nodeNeighbours[i]);
                    continue;
                }

                if(!openSet.Contains(nodeNeighbours[i]))
                {
                    openSet.Add(nodeNeighbours[i]);
                    nodeNeighbours[i].father = currentNode;
                    
                    nodeNeighbours[i].G = nodeNeighbours[i].father.G + nodeNeighbours[i].cost + Vector3.Distance(nodeNeighbours[i].transform.position, currentNode.transform.position);
                    nodeNeighbours[i].H = Mathf.Sqrt(Mathf.Pow(nodeNeighbours[i].transform.position.x - end.transform.position.x, 2) + Mathf.Pow(nodeNeighbours[i].transform.position.z - end.transform.position.z, 2));
                    
                    nodeNeighbours[i].F = nodeNeighbours[i].G + nodeNeighbours[i].H;
                }
                else
                {
                    float tempG = currentNode.G + nodeNeighbours[i].cost + Vector3.Distance(nodeNeighbours[i].transform.position, currentNode.transform.position);
                    if(nodeNeighbours[i].G > tempG)
                    {
                        nodeNeighbours[i].father = currentNode;
                        nodeNeighbours[i].G = tempG;
                        nodeNeighbours[i].H = Mathf.Sqrt(Mathf.Pow(nodeNeighbours[i].transform.position.x - end.transform.position.x, 2) + Mathf.Pow(nodeNeighbours[i].transform.position.z - end.transform.position.z, 2));
                        nodeNeighbours[i].F = nodeNeighbours[i].G + nodeNeighbours[i].H;
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == end)
                 return ThetaStar(RebuildPath(start, currentNode));

            else if (openSet.Count == 0)
                Debug.Log("No hay ningun camino disponible");
        }

        return null;
    }

    private PathNode SearchForLowestFValue()
    {
        PathNode lowestF = openSet[openSet.Count - 1];
       
        for (int i = 0; i < openSet.Count; i++)
            if (openSet[i].F < lowestF.F)
                lowestF = openSet[i];

        return lowestF;
    }

    private PathNode[] RebuildPath(PathNode start, PathNode end)
    {
        var current = end;
        List<PathNode> path = new List<PathNode>();

        while(current != start)
        {
            path.Add(current);
            current = current.father;
        }

        path.Reverse();

        return path.ToArray();
    }

    private void RecalculatePath(PathNode node)
    {
        if (node.isEnd) //Lista esta invertida, asi que el padre de start es null
        {
            node.father = null;
            return;
        }

        foreach (PathNode neighbour in node.neighborsInSight)
            if (neighbour.index > node.father.index)
                node.father = neighbour;
    }

    private PathNode[] ThetaStar(PathNode[] AstarPath)
    {
        for (int i = 0; i < AstarPath.Length; i++)
            AstarPath[i].index = i;

        foreach (PathNode nodeOne in AstarPath) //Pasamos todos los nodos que nos da Reconstruir Camino
        {
            nodeOne.neighborsInSight = new List<PathNode>(); //Creamos una nueva lista de vecinos, y borramos la anterior.
            foreach (PathNode nodeTwo in AstarPath) //Creamos otro foreach para los rayos.
            {
                //Tiramos un rayo desde cada nodo, hacia la dirección entre ese nodo y el próximo.
                Ray ray = new Ray(nodeOne.transform.position, (nodeTwo.transform.position - nodeOne.transform.position).normalized);
                RaycastHit hit;

                //Si chocamos 
                if (Physics.Raycast(ray, out hit, 500))
                    if (hit.collider.gameObject == nodeTwo.gameObject)   //Si lo que chocamos es igual al nodo con el que comprobamos, es decir, no hay nada de por medio...
                        nodeOne.neighborsInSight.Add(nodeTwo);  //Lo agregamos a la lista. Esto hace que se acorten caminos. Espectacular.
            }
        }

        foreach (PathNode node in AstarPath)  //Recalculo el camino para cada nodo.
            RecalculatePath(node);


        List<PathNode> path = new List<PathNode>(); //Creamos una nueva lista de Camino.
        PathNode c = AstarPath[0];  //Node C es igual al primer nodo de AStar.
        while (c.father != null && !c.isEnd)      //Mientras c tenga padre y no sea el nodo final...
        {
            path.Add(c);    //Lo agrego a la lista de caminos
            c = c.father; //Igualo c, al padre de c.
        }

        path.Add(c);  //Agrego C al camino
        return path.ToArray(); //y devuelvo el camino a seguir...
    }
}
