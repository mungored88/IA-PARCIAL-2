using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;
	public float changeDistance;
	public float shootDistance;
	public int idleWeight;
	public int patrolWeight;
	public Material alertMaterial;
	public Bullet bulletPrefab;
	public Transform target;
    public List<Transform> waypointsList;
    public Transform actualWaypoint;

    private int _waypointIndex;
    private bool _backtrack = false;
	//private bool _shoot;
	private bool _doneShooting;
	private bool _canMove;
	private Rigidbody _rb;
	private MeshRenderer _renderer;
	private Material _defaultMaterial;
	private RouletteWheel _roulette;

	 

	//Diccionario de la ruleta
	Dictionary<ActionNode<string>, int> _rouletteActions = new Dictionary<ActionNode<string>, int>();

	//State Machine
	private FSM<string> _fsm;
	public bool chasing, isIdle; 

	[Header("Line Of Sight")]
	LineOfSight _lineofSight;
	public bool inSight;
	public bool inSmallSight;
	public float rotationAngle = 10f;
	public float idleTime = 2f;
	public float timeToRotate;

	//Decision Tree
	QuestionNode _questionInSight;
	ActionNode<string> _actionPatrol;
	ActionNode<string> _actionShoot;
	ActionNode<string> _actionIdle;

	//Steering Behaviour
	ISteering _obstacleAvoidance;
	public float radius;
	public float avoidAmmount;
	public LayerMask mask;


	private void Awake()
	{
		_roulette = new RouletteWheel();
		_lineofSight = GetComponent<LineOfSight>();
		_rb = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
    {
		_canMove = true;
		if (waypointsList != null)
        {
            actualWaypoint = waypointsList[0];
        }
        _waypointIndex = 0;

		//Steering Behaviours
		_obstacleAvoidance = new ObstacleAvoidance(transform, target, radius, avoidAmmount, mask);

		//Declaro los estados y hago las transiciones
		var idle = new IdleState<string>(this);
		var patrol = new PatrolState<string>(this);
		var shoot = new ShootState<string>(this, _obstacleAvoidance.GetDirection());

		idle.AddTransition("Patrol", patrol);

		patrol.AddTransition("Idle", idle);
		patrol.AddTransition("Shoot", shoot);

		shoot.AddTransition("Idle", idle);
		shoot.AddTransition("Patrol", patrol);

		_fsm = new FSM<string>(idle);

		_actionShoot = new ActionNode<string>(_fsm, "Shoot");
		_actionPatrol = new ActionNode<string>(_fsm, "Patrol");
		_actionIdle = new ActionNode<string>(_fsm, "Idle");
		_questionInSight = new QuestionNode(IsInSight, _actionShoot, _actionPatrol);

		//Empiezo a ejecutar el decision tree
		_questionInSight.Execute();

		//Agrego las acciones y pesos a la ruleta
		_rouletteActions.Add(_actionIdle, idleWeight);
		_rouletteActions.Add(_actionPatrol, patrolWeight);

		//_shoot = true;
		_renderer = GetComponent<MeshRenderer>();
		_defaultMaterial = _renderer.material;
	}

	// Update is called once per frame
	void Update()
    {
		if(!chasing && !isIdle)
		{
			RunTree();
		}

		if (inSight == false)
        {
			CheckDistanceToWaypoint();
			chasing = false;
			Debug.Log("Out Of Sight");
		}
		
		_fsm.OnUpdate();
	}

	//Ejecuta el árbol cuando lo llaman, así no se ejecuta todos los frames, y se puede llamar desde otros estados.
	public void RunTree()
	{
		_questionInSight.Execute();
	}


	//Mueve al personaje al waypoint que tenga que moverse
	public void Patrol()
    {
		var dir = actualWaypoint.position - transform.position;
		_rb.velocity = dir.normalized * speed * Time.deltaTime;
		transform.forward = Vector3.Lerp(transform.forward, dir, timeToRotate);
	}

    //Chequeo si llego a un limite en el primer if. En el segundo chequeo si esta avanzando o retrocediendo y luego asigno el waypoint al que moverse. Esto se llama al estar cerca del waypoint
    private void ChangeWaypoint()
    {
        if (_waypointIndex == waypointsList.Count - 1)
            _backtrack = true;
        else if (_waypointIndex == 0)
            _backtrack = false;

        if (_backtrack)
            _waypointIndex--;
        else
            _waypointIndex++;

        actualWaypoint = waypointsList[_waypointIndex];

		ActivateRoulette(); //Le dice al enemigo (IA) si al llegar al waypoint cambia a estado Idle o no
    }

	//Chequeo la distancia al waypoint
	private void CheckDistanceToWaypoint()
    {
		if((actualWaypoint.position - transform.position).magnitude < changeDistance)
			ChangeWaypoint();
	}


    public void Idle()
    {
		_rb.velocity = Vector3.zero;
		var rotationY = Quaternion.Euler(0, Mathf.Sin(Time.time * 1.5f) * rotationAngle, 0);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotationY, Time.deltaTime);
		StartCoroutine(Timer());
    }

	//El estado llama al tree al finalizar
	public IEnumerator Timer()
	{
		yield return new WaitForSecondsRealtime(3f);
		RunTree();
	}

	public void Shoot(Vector3 direction, bool chasing)
    {
		if (_canMove)
        {
			if (chasing)
			{
				var dir = (target.transform.position - transform.position).normalized;

				_rb.velocity = dir * speed * Time.deltaTime;
				transform.forward = Vector3.Lerp(transform.forward, dir, 0.2f);
				_renderer.material = alertMaterial;

				//Chequeo si esta a distancia de disparo
				if ((target.position - transform.position).magnitude <= shootDistance && _doneShooting == false)
				{
					var player = target.gameObject.GetComponent<Player>();
					Bullet bullet = Instantiate(bulletPrefab, this.transform);
					bullet.transform.parent = null;
					bullet.target = target.transform;
					_doneShooting = true;
					_canMove = false;
					_rb.velocity = Vector3.zero;
				}
				RunTree();
			}
			else
			{
				_rb.velocity = Vector3.zero;
				_renderer.material = _defaultMaterial;
			}
        }
    }

	bool IsInSight()
    {
		inSight = _lineofSight.IsInSight(target);
		return inSight;
	}


	//La ruleta llama al action node
	private void ActivateRoulette()
    {
		_roulette.ExecuteAction(_rouletteActions).Execute();
    }
}
