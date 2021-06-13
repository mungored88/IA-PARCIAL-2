using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IObserver, IObservable
{
    public event Action<float, float> OnMove = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnGetDamage = delegate { };
    public event Action<float> OnHealthUpdate = delegate { };
    public event Action<float> OnSpeedUp = delegate { };
    public event Action OnSpeedDown = delegate { };
    public event Action<int> OnShieldsChanged = delegate { };
    public event Action OnShieldDeactivated = delegate { };
    public event Action<float> OnMagnetChanged = delegate { };
    public event Action OnMagnetDeactivated = delegate { };

    public int healthPoints;
    public int maxHP;

    public bool isDead = false;

    public float speed;
    public float knockbackForce;

    public float speedBoost;
    public float speedBoostTime;

    public float magnetDistance;
    public float magnetTime;

    public float angle;
    public LayerMask mask;

    public float jumpForce = 15;
    public float gravityChangeTime;
    public int jumpAmmount = 2;
	int _maxJumps;

    private int shieldHits;
    public int maxShieldHits;
    private bool _shieldActive = false;

    private GameManager gameManager;

    private SphereCollider magnetCollider;

    private float _origSpeed;
    private Vector3 _checkpoint;
    private Transform _cam;
    private Rigidbody _rb;
    private bool _end;

    private List<IObserver> _observers = new List<IObserver>();
    private Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    private IController _controller;
    private delegate void Execute();

	//CLIMB TEST
	[Header("Climb")]
	public bool climbing;
	public event Action<float, float> OnClimb = delegate { };

    Vector3 defaultGravity;

	private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        defaultGravity = Physics.gravity;
        _controller = new PlayerController(this, GetComponentInChildren<PlayerView>());
    }

    void Start()
    {
        _cam = Camera.main.transform;
        gameManager = FindObjectOfType<GameManager>();

        _checkpoint = transform.position;

        _origSpeed = speed;
		_maxJumps = jumpAmmount;
		
		_actionsDic.Add("Speed", SpeedUp);
        _actionsDic.Add("Shield", ActivateShield);
        _actionsDic.Add("Magnet", ActivateMagnet);
        _actionsDic.Add("End", SetEnd);
        _actionsDic.Add("Heal", Heal);

        if (magnetCollider == null)
            magnetCollider = GetComponent<SphereCollider>();
        magnetCollider.radius = magnetDistance;
        magnetCollider.isTrigger = true;
        magnetCollider.enabled = false;

        gameManager.Subscribe(this);
        OnHealthUpdate(healthPoints);
    }

    void Update()
    {
        if (!isDead && !_end)
            _controller.OnUpdate();
    }

    public void SetEnd()
    {
        _end = true;
    }

    public void Move(Vector3 direction)
    {
        OnMove(direction.x, direction.z);

        var dir = (_cam.forward * direction.z) + (_cam.right * direction.x);
        dir.y = 0;
        transform.position += dir * speed * Time.deltaTime;
        transform.forward = new Vector3(_cam.forward.x, 0, _cam.forward.z);
    }

    public void Jump()
    {
        if (jumpAmmount > 0 && !climbing)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(DownForce());
        }
        
		else if(jumpAmmount > 0 && climbing)
		{
			_rb.useGravity = true;
			climbing = false;
			//El vector este toma posiciones del mundo y tendría que tomar las del pj para el z más que nada.
			_rb.AddForce(-transform.position * (jumpForce/2)  , ForceMode.Impulse);

			//Tengo que cambiar los valores x & z (entre -1 y 1) dependiendo de que lado este la espalda del pj con respecto a los ejes del mundo.
			
		}
        
        jumpAmmount--;
    }
    IEnumerator DownForce()
    {
        Debug.Log("masaaa");
        yield return new WaitForSeconds(gravityChangeTime);
        Physics.gravity = Physics.gravity * 2;
    }

	public void Climb(Vector3 direction)
	{
		OnClimb(direction.x, direction.y);

		//var dir = new Vector3(0, direction.y, direction.z);

		var dir = (transform.right * direction.x) + (transform.up * direction.y);

		//Debug.Log(dir.z);
		transform.position += dir * (speed/2) * Time.deltaTime;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Water>(out Water water))
            transform.position = _checkpoint;

		if (collision.gameObject.TryGetComponent<ClimbRock>(out ClimbRock rock))
		{
			climbing = true;
			_rb.useGravity = false;
			
			//Mejorar para que quede un angulo recto entre donde colisionas(no necesariamente la posición del objeto) y donde esta el personaje
			var yValue = transform.position.y;
			var otherPos = collision.gameObject.transform.position;
			otherPos.y = yValue;
			transform.forward = -(transform.position - otherPos);
		}

		jumpAmmount = _maxJumps;
        Physics.gravity = defaultGravity;
    }

	

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<ClimbRock>(out ClimbRock rock))
		{
			climbing = false;
			_rb.useGravity = true;
		}
	}

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
            _checkpoint = checkpoint.Position;
    }

    public void OnAction(string action)
    {
        _actionsDic[action]();
    }

    void SpeedUp()
    {
        speed += speedBoost;
        StartCoroutine(SpeedTimer());
    }

    void ActivateShield()
    {
        OnShieldsChanged(maxShieldHits);
        shieldHits = maxShieldHits;
        _shieldActive = true;
    }

    void DeactivateShield()
    {
        OnShieldDeactivated();
        _shieldActive = false;
    }

    void ActivateMagnet()
    {
        magnetCollider.enabled = true;
        StartCoroutine(MagnetTimer());
    }

    void Heal()
    {
        if (healthPoints < maxHP)
        {
            healthPoints++;
            OnHealthUpdate(healthPoints);
        }
    }

    public void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public void Unsuscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    public void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
            _observers[i].OnAction(action);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * magnetDistance);
        Gizmos.DrawWireSphere(transform.position, magnetDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 360 / 2, 0) * transform.forward * magnetDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -360 / 2, 0) * transform.forward * magnetDistance);
    }

    public void GetHit()
    {
        Debug.Log("Me pegaron");
        if (_shieldActive)
        {
            shieldHits--;
            OnShieldsChanged(shieldHits);
            if (shieldHits <= 0)
                DeactivateShield();
        }
        else
        {
            healthPoints--;
            if (healthPoints <= 0)
                NotifyObserver("Lose");
            _rb.AddForce(-transform.forward, ForceMode.Force);
            OnGetDamage();
            OnHealthUpdate(healthPoints);
        }
    }

    public void OneShotKill()
    {
        NotifyObserver("Lose");
    }

    IEnumerator MagnetTimer()
    {
        var time = magnetTime;
        do
        {
            time--;
            OnMagnetChanged(time);
            yield return new WaitForSeconds(1f);
        } while (time > 0);
        magnetCollider.enabled = false;
        OnMagnetDeactivated();
    }

    IEnumerator SpeedTimer()
    {
        var time = speedBoostTime;
        do
        {
            time--;
            OnSpeedUp(time);
            yield return new WaitForSeconds(1f);
        } while (time > 0);

        speed = _origSpeed;
        OnSpeedDown();
    }
}
