using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemy, IObserver
{
    public float speed;
    public float attackDistance;
    public float homeDistance;
    public float avoidanceRadius, avoidanceWeight;
    public LayerMask mask;
    public AudioClip screamSound;
    public AudioClip attackSound;
    public float screamTime;
    AudioSource _audioSource;
    bool _scream;
    ISteering _obstacleAvoidance;
    Vector3 _startPos;
    public MeshRenderer meshRender;
    Color _defaultColor;
    bool _colorChanged = false;
    //Vector3 _obstacleAvoidanceDirection = Vector3.zero;

    GameManager _gameManager;
    bool _end;

    //Decision Tree Actions
    ActionNode<string> _actionChase;
    ActionNode<string> _actionIdle;
    ActionNode<string> _actionPatrol;
    ActionNode<string> _actionAttack;
    ActionNode<string> _actionReturn;
	[HideInInspector]
    public QuestionNode questionClose;
	[HideInInspector]
    public QuestionNode questionKeepChasing;
    [HideInInspector]
    public QuestionNode questionCanRest;

    ChaseState<string> chase;


    #region pathfinding
    /*int _nextPoint = 0;
    public List<PathNode> waypoints;
    public PathAgent pathAgent;
    public bool readyToMove;
    #endregion

    #region waypointSystem
    public Waypoint waypointSystem;
    int wpCount;*/
	#endregion

	//Waypoint Strategy
	public List<Transform> waypointsList = new List<Transform>();


    private void Awake()
    {
        LineOfSight = GetComponent<LineOfSight>();
        //pathAgent = GetComponent<PathAgent>();
        //waypointSystem = new Waypoint(ActionPoint, transform, waypoints, speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerModel>().transform;
        GameObject wpCpontainer = GameObject.Find("Waypoints");
        if (wpCpontainer != null)
        {
            var childs = wpCpontainer.GetComponentsInChildren(typeof(Transform));
            foreach (var item in childs)
            {
                waypointsList.Add(item.transform);
            }
            waypointsList.RemoveAt(0);
        }
        
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = screamSound;
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.Subscribe(this);
        _actionsDic.Add("End", End);
        _obstacleAvoidance = new ObstacleAvoidance(transform, target, avoidanceRadius, avoidanceWeight, mask);
		_startPos = transform.position;
        _defaultColor = meshRender.material.color;
        var idle = new IdleState<string>(this);
        chase = new ChaseState<string>(this, transform, speed);
		var attack = new AttackState<string>(this, target.GetComponent<PlayerModel>());
		var returnState = new ReturnState<string>(this);
        var patrolState = new PatrolBearState<string>(this, waypointsList);

        idle.AddTransition("Chase", chase);
        chase.AddTransition("Idle", idle);
        chase.AddTransition("Attack", attack);
		chase.AddTransition("Return", returnState);
        chase.AddTransition("Patrol", patrolState);
        attack.AddTransition("Chase", chase);
		attack.AddTransition("Idle", idle);
        returnState.AddTransition("Patrol", patrolState);
        returnState.AddTransition("Chase", chase);
		returnState.AddTransition("Idle", idle);
        patrolState.AddTransition("Chase", chase);
        patrolState.AddTransition("Idle", chase);


        _fsm = new FSM<string>(patrolState);

        _actionChase = new ActionNode<string>(_fsm, "Chase");
        _actionIdle = new ActionNode<string>(_fsm, "Idle");
        _actionAttack = new ActionNode<string>(_fsm, "Attack");
        _actionReturn = new ActionNode<string>(_fsm, "Return");
        _actionPatrol = new ActionNode<string>(_fsm, "Patrol");

        questionInSight = new QuestionNode(IsInSight, _actionChase, _actionPatrol);
        questionKeepChasing = new QuestionNode(IsInSight, _actionChase, _actionReturn);
        questionClose = new QuestionNode(IsCloseToTarget, _actionAttack, questionKeepChasing);

        StartCoroutine(RunTreeTimer(questionInSight));

    }

    // Update is called once per frame
    void Update()
    {
        if (_end == false)
        {
            chase.SetDirection(_obstacleAvoidance.GetDirection());

            _fsm.OnUpdate();

            Debug.Log("insight oso:   " + inSight);
            if (IsInSight())
            {
                meshRender.material.color = Color.red;
                _colorChanged = true;
                if (_scream == false)
                {
                    Scream();
                }
            }
            else if (_colorChanged)
            {
                meshRender.material.color = _defaultColor;
                _colorChanged = false;
            }
        }
		
    }

	public override IEnumerator RunTreeTimer(QuestionNode question)
    {
        yield return new WaitForSeconds(treeTimer);
		question.Execute();
    }

    public bool IsCloseToTarget()
    {
        var dist = (target.position - transform.position).magnitude;
        if (dist < attackDistance)
            return true;
        else return false;
    }

    public void Attack()
    {
        _audioSource.clip = attackSound;
        _audioSource.Play();
        target.GetComponent<PlayerModel>().OneShotKill();
    }

	#region IA

	//public IEnumerator AttackTimer(float seconds)
	//{
	//       Debug.Log("attack timer");
	//	yield return new WaitForSeconds(seconds);
	//	Attack();
	//}

	/*public void Return(Vector3 target, Vector3 homeTarget)
    {
        //VUELVO Al ORIGEN, ACA SE EJECUTARIA EL PATHFINDING
        CheckIfArrived(homeTarget);
        Move(target);
    }

    //BORRAR SI NO ES NECESARIO PARA EL PF
    void CheckIfArrived(Vector3 target)
    {
        var dist = (target - transform.position).magnitude;
        if (dist <= homeDistance)
            StartCoroutine(RunTreeTimer(questionInSight));
        else _actionReturn.Execute();
    }

    public void SetWayPoints(List<PathNode> newPoints)
    {
        waypoints = newPoints;
    }

    public void Patrol(Vector3 target)
    {
        Move(target);
    }

    public void ActionPoint()
    {
        // cada que llega a un wp hace esto
        wpCount++;
    }

    public void Move(Vector3 target)
    {
        transform.position += target * speed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, target, 0.2f);
    }*/
	#endregion

	public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }

    public void End()
    {
        _end = true;
    }

    void Scream()
    {
        _scream = true;
        _audioSource.clip = screamSound;
        _audioSource.Play();
        
        StartCoroutine(ScreamTimer());
    }

    IEnumerator ScreamTimer()
    {
        yield return new WaitForSeconds(screamTime);
        _scream = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerModel>(out PlayerModel player))
            player.GetHit();
    }
}
