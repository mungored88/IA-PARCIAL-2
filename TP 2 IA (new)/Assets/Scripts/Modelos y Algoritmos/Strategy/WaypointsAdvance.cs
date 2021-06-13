using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsAdvance : IAdvance
{
	Transform _transform;
	List<Transform> _waypoints;
	Transform _nextWaypoint;
	int _waypointIndex = 0;
	float _speed;

	public WaypointsAdvance(Transform transform, List<Transform> waypoints, float speed)
	{
		_transform = transform;
		_waypoints = waypoints;
		_nextWaypoint = _waypoints[0];
		_speed = speed;
	}

	public void Advance()
	{
		_transform.LookAt(_nextWaypoint);
		_transform.position += (_nextWaypoint.position - _transform.position).normalized * _speed * Time.deltaTime;
		if ((_nextWaypoint.position - _transform.position).magnitude <= 0.1f)
			ChangeWaypoint();
	}

	void ChangeWaypoint()
	{
		if (_waypointIndex == _waypoints.Count - 1)
        {
			_waypointIndex = 0;
			
		}
		else _waypointIndex++;
		_nextWaypoint = _waypoints[_waypointIndex];

	}
}
