using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IObservable
{

    private List<IObserver> _observers = new List<IObserver>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        Subscribe(FindObjectOfType<PlayerModel>());
    }

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
