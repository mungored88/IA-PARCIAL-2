using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : Enemy, IObserver
{
    
    public float shootTimer;
    public float jumpForce;
    private int _jumpTimes;
    private Rigidbody _rb;

    AudioSource _audioSource;
    public AudioClip screamClip;
    public AudioClip shootClip;

    public MeshRenderer meshRender;
    Color _defaultColor;
    bool _colorChanged = false;

    //Bullet Spawn
    private BulletSpawn _bulletSpawn;
    private bool _canShoot = true;

    //Decision Tree Actions
    ActionNode<string> _actionShoot;
    ActionNode<string> _actionIdle;

    bool _end;

    //Strategy
    IAdvance _linearAdvanceStrategy;
    IAdvance _sinuousAdvanceStrategy;
    private Transform _appleSpawnPoint;

    private void Awake()
    {
        target = FindObjectOfType<PlayerModel>().GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        LineOfSight = GetComponent<LineOfSight>();
        _jumpTimes = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _appleSpawnPoint = transform.GetChild(0).GetComponent<Transform>();
        _audioSource = GetComponent<AudioSource>();
        _bulletSpawn = FindObjectOfType<BulletSpawn>();
        GameManager gm = FindObjectOfType<GameManager>();
        gm.Subscribe(this);
        _actionsDic.Add("TargetHit", TargetHit);
        _actionsDic.Add("End", End);

		var idle = new IdleState<string>(this);
		var shoot = new ShootState<string>(this, _bulletSpawn, target);

		idle.AddTransition("Shoot", shoot);
		shoot.AddTransition("Idle", idle);
		_fsm = new FSM<string>(idle);


        _actionShoot = new ActionNode<string>(_fsm, "Shoot");
        _actionIdle = new ActionNode<string>(_fsm, "Idle");

        _actionIdle.Execute();
	}

    // Update is called once per frame
    void Update()
    {
        if (_end == false)
            _fsm.OnUpdate();

        if (IsInSight())
        {
            if (_colorChanged == false)
            {
                meshRender.material.color = Color.red;
                _colorChanged = true;
            }
        }
        else if (_colorChanged)
        {
            meshRender.material.color = _defaultColor;
            _colorChanged = false;
        }
    }

	public override void Shoot()
    {
        
        if (_canShoot)
        {
            _audioSource.clip = shootClip;
            _audioSource.Play();
            Debug.Log("disparo");
            _canShoot = false;
            var apple = _bulletSpawn.applePool.PickObjectFromPool();
            apple.transform.position = _appleSpawnPoint.position;
            apple.transform.rotation = transform.rotation;
            Vector3 dir = (target.position - transform.position);
            apple.dir = dir;
            apple.owner = this;
            apple.Subscribe(this);

            int random = Random.Range(1, 11);

            if (random <= 5)
            {
                Debug.Log("lineal");
                _linearAdvanceStrategy = new LinearAdvance(apple.transform, apple.straightSpeed, dir);
                apple.SetStretegy(_linearAdvanceStrategy);
            } 
            else
            {
                Debug.Log("sinuos");
                _sinuousAdvanceStrategy = new SinuousAdvance(apple.transform, apple.sinuousSpeed, dir);
                apple.SetStretegy(_sinuousAdvanceStrategy);
            }

            StartCoroutine(ShootTimer());
        }
    }

    public void TargetHit()
    {
        _audioSource.clip = screamClip;
        _audioSource.Play();
        StartCoroutine(Jump());
    }

    public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
    }

    IEnumerator Jump()
    {
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        if (_jumpTimes < 2)
        {
            yield return new WaitForSeconds(0.5f);
            _jumpTimes++;
            StartCoroutine(Jump());
        }
        else _jumpTimes = 0;
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootTimer);
        _canShoot = true;
        if (IsInSight())
        {
            
            _actionShoot.Execute();
        }
        else
        {
            
            _actionIdle.Execute();
        }
    }

    public override IEnumerator RunTreeTimer(QuestionNode question)
    {
        yield return new WaitForSeconds(treeTimer);
        if (IsInSight())
            _actionShoot.Execute();
        else _actionIdle.Execute();
    }

    public void End()
    {
        _end = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerModel>(out PlayerModel player))
            player.GetHit();
    }
}
