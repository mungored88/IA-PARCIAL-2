using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearAdvance : IAdvance
{
	float _speed;
	Transform _transform;
	Vector3 _dir;

	public LinearAdvance(Transform transform, float speed, Vector3 dir)
	{
		_transform = transform;
		_speed = speed;
		_dir = dir;
	}

	public void Advance()
	{
		_transform.position += _dir.normalized * _speed * Time.deltaTime;
		_transform.forward = Vector3.Lerp(_transform.forward, _dir, 0.2f);
	}
}
