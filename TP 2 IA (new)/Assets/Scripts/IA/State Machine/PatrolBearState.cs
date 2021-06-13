using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBearState<T> : States<T>
{
    Bear _bear;
   // Waypoint waypoint;

	IAdvance _waypointAdvance;

    public PatrolBearState(Bear _bear, List<Transform> listWp)
    {
        this._bear = _bear;
        //this._bear.pathAgent.init = listWp[0];
        //this._bear.pathAgent.finit = listWp[1];
        //this._bear.pathAgent.PathFindingTheta();
        //waypoint = new Waypoint(_bear.ActionPoint, _bear.transform, _bear.waypoints , _bear.speed);
       // waypoint = new Waypoint(_bear.ActionPoint, _bear.transform, listWp, _bear.speed);
		_waypointAdvance = new WaypointsAdvance(_bear.transform, listWp, _bear.speed);
    }

    public override void Awake()
    {
    }

    public override void Execute() 
    {
		//_bear.Patrol(waypoint.GetDirection());
		if(_waypointAdvance != null)
			_waypointAdvance.Advance();
        _bear.StartCoroutine(_bear.RunTreeTimer(_bear.questionInSight));        
    }

    public override void Sleep()
    {
        
    }
}
