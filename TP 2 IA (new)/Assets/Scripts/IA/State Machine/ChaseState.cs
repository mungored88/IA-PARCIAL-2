using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState<T> : States<T>
{
	
	Bear _bear;
	Transform _transform;
	
	Vector3 _direction;
	float _speed;
	IAdvance _linearAdvance;

	public ChaseState(Bear e, Transform transform, float speed)
	{
		_bear = e;
		_transform = transform;
		_speed = speed;
		
	}

	public ChaseState<T> SetDirection(Vector3 dir)
	{
		_direction = dir;
		_linearAdvance = new LinearAdvance(_transform, _speed, _direction);
		return this;
	}

	public override void Awake()
	{
		Debug.Log("Chase Awake");
		_bear.StopAllCoroutines();
	}

	public override void Execute()
	{
		#region IA
		//_transform.position += _direction * _speed * Time.deltaTime;
		//_transform.forward = Vector3.Lerp(_transform.forward, _direction, 0.2f);

		//_bear.Move(_direction);

		//Ejecutar el runtree con el question node correspondiente.
		//_bear.StartCoroutine(_bear.RunTreeTimer())
		#endregion
		if (_linearAdvance != null)
		{
			_linearAdvance.Advance();
		}

		if (_bear.inSight)
		{
			_bear.StartCoroutine(_bear.RunTreeTimer(_bear.questionClose));
		}
		else Sleep();
		;

		Debug.Log("Chase execute");

	}

	public override void Sleep()
	{
		
		Debug.Log("Chase Sleep");
	}
}
