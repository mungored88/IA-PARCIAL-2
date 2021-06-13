using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinuousAdvance : IAdvance
{
	float _speed;
	Transform _transform;
	Vector3 _pos;
	Vector3 _dir;
	public SinuousAdvance(Transform transform, float speed, Vector3 dir)
	{
		_transform = transform;
		_pos = _transform.position;
		_speed = speed;
		_dir = dir;
	}

	public void Advance()
	{
		_pos += _dir.normalized * Time.deltaTime * _speed;
		_transform.position = _pos + _transform.right * Mathf.Sin(Time.time * _speed);
	}
}
