using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Enemy, IObserver
{
    public float speed;
    public List<Transform> waypointsContainer;
    int _WpContainerIndex = 0;
    Transform _nextWP;
    public float attackRange;
    public float shootTimer;

    public Transform pointToSpawn;
    private BulletSpawn _bulletSpawn;
    private bool _canShoot = true;
    GameManager _gameManager;
    bool _end;
    public bool _canAttack = false;
    AudioSource _audioSource;
    public AudioClip screamSound;

    //Strategy
    IAdvance _linearAdvanceStrategy;
    IAdvance _sinuousAdvanceStrategy;

	IAdvance _currentAdvance;
    IAdvance _linearAdvance;
    IAdvance _sinuousAdvance;
	IAdvance _waypointsAdvance;

    // Start is called before the first frame update
    void Start()
    {
        GameObject wpCpontainer = GameObject.Find("Waypoints");
        if (wpCpontainer != null)
        {
            var childs = wpCpontainer.GetComponentsInChildren(typeof(Transform));
            foreach (var item in childs)
            {
                waypointsContainer.Add(item.transform);
            }
            waypointsContainer.RemoveAt(0);
        }
        _bulletSpawn = FindObjectOfType<BulletSpawn>();
        _canAttack = false;
        target = FindObjectOfType<PlayerModel>().transform;
        _nextWP = waypointsContainer[0];
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager != null)
        _gameManager.Subscribe(this);
        _actionsDic.Add("End", End);
        _audioSource = GetComponent<AudioSource>();

		_linearAdvance = new LinearAdvance(transform, speed, transform.forward);
		_sinuousAdvance = new SinuousAdvance(transform, speed, transform.forward);
		_waypointsAdvance = new WaypointsAdvance(transform, waypointsContainer, speed);
		_currentAdvance = _waypointsAdvance;
    }

    // Update is called once per frame
    void Update()
    {
		if (_currentAdvance != null)
			_currentAdvance.Advance();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _audioSource.clip = screamSound;
            _audioSource.Play(); 
        }

        if (_canAttack && (target.position - transform.position).magnitude <= attackRange)
        {
            Shoot();
        }
    }

    public override IEnumerator RunTreeTimer(QuestionNode question)
    {
        return null;
    }

    public void SelectWaypoint()
    {
        if (_WpContainerIndex == waypointsContainer.Count - 1)
            _WpContainerIndex = 0;
        else _WpContainerIndex++;

        _nextWP = waypointsContainer[_WpContainerIndex];
    }


    private Vector3 WaypointDirection()
    {
        return (_nextWP.position - transform.position).normalized;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<PlayerModel>() != null)
        {
            Debug.Log("trigger enter");
            _canAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerModel>() != null)
        {
            Debug.Log("trigger exit");
            _canAttack = false;
        }
    }
    public override void Shoot()
    {
        if (_canShoot)
        {
            _audioSource.clip = screamSound;
            _audioSource.Play();
            Debug.Log("disparo");
            _canShoot = false;
            var p = _bulletSpawn.applePool.PickObjectFromPool();
            p.transform.position = pointToSpawn.position;
            p.transform.rotation = transform.rotation;
            Vector3 dir = (target.position - p.transform.position);
            p.dir = dir;


            int random = Random.Range(0, 3);
            if (random <= 1)
            {
                _linearAdvanceStrategy = new LinearAdvance(p.transform, p.straightSpeed, dir);
                p.SetStretegy(_linearAdvanceStrategy);
            }
            else
            {
                _sinuousAdvanceStrategy = new SinuousAdvance(p.transform, p.sinuousSpeed, dir);
                p.SetStretegy(_sinuousAdvanceStrategy);
            }

            StartCoroutine(ShootTimer());
        }
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootTimer);
        _canShoot = true;
    }

    public void OnAction(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
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
