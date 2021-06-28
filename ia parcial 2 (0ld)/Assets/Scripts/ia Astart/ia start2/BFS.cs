using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS
{
	private Queue<PathNode> Queue = new Queue<PathNode>();
	private List<PathNode> walkPath = new List<PathNode>();

	public BFS()
	{
		Queue = new Queue<PathNode>();
		walkPath = new List<PathNode>();
	}

	//Solo utilizable para saber si hay un camino posible.
	public bool BreadthFirstSearch(PathNode start, PathNode end)
	{
		#region Clear
		PathNode[] reset = GameObject.FindObjectsOfType<PathNode>();
		foreach (PathNode item in reset)
			item.visited = false;

		Queue.Clear();
		walkPath.Clear();
		#endregion;

		Queue.Enqueue(start);   //Agregamos el start
		start.visited = true;   //Decimos que lo visitamos

		var currentNode = start;

		while (Queue.Count > 0)
		{
			currentNode = Queue.Dequeue();  //Sacamos 
			walkPath.Add(currentNode);  //Lo agregamos a los posibles

			if (currentNode == end)   //Si es el final hay camino
				return true;
			if (end.isBlocked)     //Si el final que nos pasaron esta bloqueado, directamente no se puede ir.
				return false;

			foreach (var item in currentNode.neighbors)   //Si no es el final repasamos los vecinos
			{
				if (!item.visited && !item.isBlocked)  //Si el vecino no fue visitado ni tampoco esta bloqueado
				{
					item.visited = true;		//Avisamos que lo visitamos
					Queue.Enqueue(item);		//Lo agregamos a la lista.
				}
			}
		}

		return false; //No hay camino posible.
	}
}
