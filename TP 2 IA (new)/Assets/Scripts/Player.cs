using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float speed;
	public Manager manager;
	public Transform cam;

	float _h;
	float _v;
	Rigidbody _rb;

	//State Machine
	private FSM<string> _playerFsm;

	//Decision Tree
	ActionNode<string> _actionIdle;
	ActionNode<string> _actionMove;
	QuestionNode _questionMove;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
    {
		var myRenderer = GetComponent<Renderer>();
		var idle = new PlayerIdleState<string>(this, myRenderer);
		var move = new PlayerMoveState<string>(this);

		idle.AddTransition("Move", move);
		move.AddTransition("Idle", idle);

		_playerFsm = new FSM<string>(idle);

		_actionIdle = new ActionNode<string>(_playerFsm, "Idle");
		_actionMove = new ActionNode<string>(_playerFsm, "Move");

		_questionMove = new QuestionNode(Moving, _actionMove, _actionIdle);
    }

    // Update is called once per frame
    void Update()
    {
		_h = Input.GetAxis("Horizontal");
		_v = Input.GetAxis("Vertical");

		_playerFsm.OnUpdate();
		_questionMove.Execute();
    }

	public void Move()
	{
		

		if (_h == 0 && _v == 0)
			_rb.velocity = Vector3.zero;
        else
        {
			var dir = (cam.forward * _v) + (cam.right * _h);
			_rb.velocity = dir.normalized * speed * Time.deltaTime;
		}
		
		transform.position = new Vector3(transform.position.x, 1, transform.position.z);
	}

	bool Moving()
	{
		if(_h != 0 || _v != 0)
			return true;

		return false;
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
			manager.Lose();

		if (other.gameObject.layer == LayerMask.NameToLayer("Win"))
			manager.Win();
	}
}
