using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlatforms : MonoBehaviour
{
	public float disappearTime;
	public float reappearTime;
	public bool _active = true;
	MeshRenderer _myRenderer;
	Collider _myCollider;

    // Start is called before the first frame update
    void Start()
    {
		_myRenderer = GetComponent<MeshRenderer>();
		_myCollider = GetComponent<Collider>();
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			_active = false;
			StartCoroutine(Disappear());
		}
	}

	IEnumerator Disappear()
	{ 
		yield return new WaitForSecondsRealtime(disappearTime);
		_myRenderer.enabled = _active;
		_myCollider.enabled = _active;
		StartCoroutine(Reappear());
	}

	IEnumerator Reappear()
	{
		yield return new WaitForSecondsRealtime(reappearTime);
		_active = true;
		_myCollider.enabled = _active;
		_myRenderer.enabled = _active;
	}
}
