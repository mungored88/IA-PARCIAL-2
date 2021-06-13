using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
	public float range;
	public float angle;
	public LayerMask mask;
	public bool IsInSight(Transform target)
	{
		Vector3 dis = target.position - transform.position;
		float distance = dis.magnitude;

		if (distance > range)
		{
			//Debug.Log("distancia false");
			return false;
		}
		if (Vector3.Angle(transform.forward, dis) > angle / 2)
		{
			//Debug.Log("angulo false");
			return false;
		}
		if (Physics.Raycast(transform.position, dis.normalized, distance, mask))
		{
			//Debug.Log("raycast false");
			return false;
		}
		//Debug.Log("insight true");
		return true;
	}
    private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, transform.forward * range);
		Gizmos.DrawWireSphere(transform.position, range);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);
	}
}
