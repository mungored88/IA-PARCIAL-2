using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour, IObservable
{
    public float straightSpeed;
    public float sinuousSpeed;
    public float timeToDie;
    public IObserver owner;
    public Vector3 dir;
    private bool _move;
    private List<IObserver> _observers = new List<IObserver>();
    private BulletSpawn _spawn;
    IAdvance myAdvanceStrategy;
    private void Start()
    {
        _move = true;
        _spawn = FindObjectOfType<BulletSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move)
            Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
    	PlayerModel player = collision.gameObject.GetComponent<PlayerModel>();

		if (player != null)
		{
			NotifyObserver("TargetHit");
            player.GetHit();
		}
		_move = false;
		_spawn.ReturnAppleToPool(this);
	}

    private void ResetProjectile()
    {
        _move = true;
    }

    private void Move()
    {
		if(myAdvanceStrategy != null)
			myAdvanceStrategy.Advance();
    }

    public void SetStretegy(IAdvance strategy)
    {
        myAdvanceStrategy = strategy;
        
    }

    public static void TurnOnBullet(Apple bullet)
    {
        bullet.ResetProjectile();
        bullet.gameObject.SetActive(true);
    }

    public static void TurnOffBullet(Apple bullet)
    {
        bullet.gameObject.SetActive(false);
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
        {
            _observers[i].OnAction(action);
        }
    }
}
