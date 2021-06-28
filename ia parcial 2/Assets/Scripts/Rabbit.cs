using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
	public float speed;
	private Rigidbody _rb;

	private PlayerController _player;

	public GameObject[] destinations; // array de waypoints

	[Header("Line Of Sight")]
	LineOfSight _lineofSight;


	//Steering Behaviour
	ISteering _obstacleAvoidance;
	ISteering _obstacleAvoidanceWaypoint;
	public float radius;
	public float avoidAmmount;
	public LayerMask mask;
	public int _currentTarget = 1;
	float patrolSpeed = 5f;
	float changeTargetDistance = 0.5f;
	public Transform patrolWaypoints;


	private void Awake()
	{
		//_roulette = new RouletteWheel();
		_lineofSight = GetComponent<LineOfSight>();
		_rb = GetComponent<Rigidbody>();
		_player = GameObject.FindObjectOfType<PlayerController>();
	}

	// Start is called before the first frame update
	void Start()
	{
		
		//_currentTarget = destinations[currentDestination].transform.position; // Asigna el primer destino para empezar a moverse

		//Steering Behaviours
		_obstacleAvoidance = new ObstacleAvoidance(transform, _player.transform, radius, avoidAmmount, mask);


	}

	// Update is called once per frame
	void Update()
	{
		

		if (_lineofSight.IsInSight(_player.transform) )
		{
			Vector3 dir = _obstacleAvoidance.GetDirection();
			transform.position += new Vector3(dir.x,0,dir.z) * speed * Time.deltaTime;
			transform.rotation = Quaternion.Slerp(transform.rotation,
			Quaternion.LookRotation(_player.transform.position - transform.position), 10 * Time.deltaTime);

			//var dir = (_player.transform.position - transform.position).normalized;
			//transform.position += dir * speed * Time.deltaTime;
		}

		else
		{
			if (patrolWaypoints)
			{
				if (MoveToTarget())
				{
					_currentTarget = GetNextTarget();
				}

				
			}
		
		}

	}


	public bool MoveToTarget()
	{
		
		Vector3 distanceVector = patrolWaypoints.GetComponentsInChildren<Transform>()[_currentTarget].position - transform.position;
		if (distanceVector.magnitude < changeTargetDistance)
		{
			return true;
		}
		Vector3 dir = _obstacleAvoidance.GetDirectionWaypoint(patrolWaypoints.GetComponentsInChildren<Transform>()[_currentTarget]);
		transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;
		//Vector3 velocityVector = distanceVector.normalized;
		//transform.position += new Vector3(velocityVector.x, 0, velocityVector.z) * patrolSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Slerp(transform.rotation,
		Quaternion.LookRotation(patrolWaypoints.GetComponentsInChildren<Transform>()[_currentTarget].position - transform.position), 10 * Time.deltaTime);
		
		
		return false;
	}

	public int GetNextTarget()
	{
		_currentTarget++;

		int _count = patrolWaypoints.GetComponentsInChildren<Transform>().Length - 1;
		if (_currentTarget > _count)
		{
			_currentTarget = 1;
		}

		return _currentTarget;
	}


	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerController>().recibirDaño();
		}
	}

}
