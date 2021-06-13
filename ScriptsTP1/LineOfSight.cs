using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
	public float range;
	public float angle;
	public LayerMask mask;
	public LineRenderer rightLine;
	public LineRenderer leftLine;
	public bool IsInSight(Transform target)
	{
		Vector3 dis = target.position - transform.position;
		float distance = dis.magnitude;

		if (distance > range) return false;
		if (Vector3.Angle(transform.forward, dis) > angle / 2) return false;
		if (Physics.Raycast(transform.position, dis.normalized, distance, mask)) return false;
		 
		return true;
	}
    private void Update()
    {
		DrawLine();
    }
    private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(transform.position, transform.forward * range);
		Gizmos.DrawWireSphere(transform.position, range);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range);
	}

	//Modifico las lineas que funcionan como "cono" de vision en play.
	private void DrawLine()
    {
		rightLine.transform.position = transform.position;
		leftLine.transform.position = transform.position;
		rightLine.SetPosition(1, Quaternion.Euler(0, angle / 2, 0) * transform.forward * range/2);
		leftLine.SetPosition(1, Quaternion.Euler(0, -angle / 2, 0) * transform.forward * range/2);
	}
}
