using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour, IObservable
{
    private List<IObserver> _observers = new List<IObserver>();
    

    // Start is called before the first frame update
    void Start()
    {
        var manager = FindObjectOfType<GameManager>();
        Subscribe(manager);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>() != null)
        {
            Debug.Log("InFlag");
            NotifyObserver("PlayerIn");
        }
    }
    public void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].OnAction(action);
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
}
