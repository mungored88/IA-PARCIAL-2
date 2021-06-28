using System.Collections.Generic;
using UnityEngine;

public class NPCPathFindingData : MonoBehaviour
{
	[Header("Posibles nodos en el lugar de pelea")]
	public List<PathNode> pathNodesFightZone;
	public BFS bfs;

	void Awake()
	{
		bfs = new BFS();
	}

	public Vector3 AskAPath(NpcController thisNPC, Transform npcPosition)
	{
		PathNode closestNode = thisNPC.FindNearNode(npcPosition.position); //Agarramos un nodo cercano al player.		
		PathNode someNodeInFightZone = pathNodesFightZone[Random.Range(0, pathNodesFightZone.Count)]; //Pedimos un random de los de la zona de pelea, cada npc tiene sus posiciones.

		bool isWalkable = bfs.BreadthFirstSearch(closestNode, someNodeInFightZone);   //Chequeamos que se pueda ir.

		if (isWalkable)    //Si se puede, devolvemos el nodo para que se haga el pathfinding.
			return someNodeInFightZone.transform.position;
		else
			return default(Vector3); //Si llega aca, todo esta mal. Todo. Que verguenza...
	}

}
