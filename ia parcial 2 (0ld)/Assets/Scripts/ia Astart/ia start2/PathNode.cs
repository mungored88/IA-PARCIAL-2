using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
	[HideInInspector] public PathNode father;
	[HideInInspector] public List<PathNode> neighbors;
	[HideInInspector] public List<PathNode> neighborsInSight;

	[HideInInspector] public float index;
	[HideInInspector] public float G;
	[HideInInspector] public float H;
	[HideInInspector] public float F;
	[HideInInspector] public bool visited;
	[HideInInspector] public bool isStart;
	[HideInInspector] public bool isEnd;
	[HideInInspector] public bool isPath;

	public float cost;
	public bool isBlocked;

	private float neighboursRadius;

	//Función que fue utilizada una unica vez en Start para encontrar a los vecinos. 	
	private void FindNeighbours()
	{
		neighboursRadius = 1.5f;

		if (isBlocked) return;
		Collider[] neighbourSphere = Physics.OverlapSphere(transform.position, neighboursRadius);

		foreach (var sphere in neighbourSphere)
		{
			if (sphere.GetComponent<PathNode>())
			{
				if (sphere.GetComponent<PathNode>().isBlocked == false)
					neighbors.Add(sphere.GetComponent<PathNode>());
			}
		}
	}

	void OnDrawGizmos()
	{
		if (isBlocked)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.3f);
		}
		else
		{
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(transform.position, 0.3f);
		}

		Gizmos.color = Color.white;
		foreach (var neighbour in neighbors)
			Gizmos.DrawLine(transform.position, neighbour.transform.position);

		Gizmos.color = Color.red;
		foreach (var item in neighborsInSight)
			if (item)
				Gizmos.DrawLine(transform.position, item.transform.position);
	}
}
