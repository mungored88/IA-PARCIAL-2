using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReturnState<T> : States<T>
{
	Bear _bear;
    List<PathNode> listNodes;
    Waypoint waypointReturn;
	IAdvance _waypointAdvance;
	public ReturnState(Bear _bear)
	{
		this._bear = _bear;
		_waypointAdvance = new WaypointsAdvance(_bear.transform, _bear.waypointsList, _bear.speed);
	}

	public override void Awake()
	{
		Debug.Log("Return Awake");
		#region IA
		//this._bear.pathAgent.init = _bear.pathAgent.FindNearbyNode();
		//this._bear.pathAgent.finit = _bear.waypointSystem.lastPoint;
		//this._bear.pathAgent.PathFindingTheta();
		/*_bear.pathAgent.SetNewPath(_bear.pathAgent.FindNearbyNode(), _bear.waypointSystem.lastPoint, _bear.mask);
		listNodes = _bear.pathAgent.PathFindingTheta();
		waypointReturn = new Waypoint(_bear.ActionPoint, _bear.transform, listNodes, _bear.speed);*/
		#endregion
	}

	public override void Execute()
	{
		Debug.Log("Return Execute");
		if (!_bear.inSight) _waypointAdvance.Advance();
		//_bear.Return(waypointReturn.GetDirection(), listNodes.Last().transform.position); //IA
		else
			_bear.StartCoroutine(_bear.RunTreeTimer(_bear.questionInSight));
	}

	public override void Sleep()
	{
		Debug.Log("Return Sleep");
	}
}
