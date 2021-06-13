using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IObservable
{
    public Transform target;
    
    //Line of Sight
    public LineOfSight LineOfSight;
    public bool inSight = false;
    public float treeTimer;

    public delegate void Execute();
    public Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    protected List<IObserver> _observers = new List<IObserver>();
    
    //Decision Tree
    public QuestionNode questionInSight;

	public Coroutine timer;

    //State Machine
    public FSM<string> _fsm;

    private void Start()
    {
        target = FindObjectOfType<PlayerModel>().transform;
    }

    public static void TurnOnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    public static void TurnOffEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

	public virtual void Shoot() { }

	public virtual bool IsInSight()
    {
        inSight = LineOfSight.IsInSight(target);
        return inSight;
    }

    public Enemy SetLineOfSightRange(float range)
    {
        LineOfSight.range = range;
        return this;
    }

    public Enemy SetLineOfSightAngle(float angle)
    {
        LineOfSight.range = angle;
        return this;
    }

    //Para volver a ejecutar el árbol, probablemente lo llame el idle.
    //public abstract void RunTree();
    public abstract IEnumerator RunTreeTimer(QuestionNode question);

    public virtual void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].OnAction(action);
        }
    }

    public virtual void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public virtual void Unsuscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }
}
