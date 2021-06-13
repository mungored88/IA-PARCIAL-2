using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IObservable
{
	GameManager _manager;
    private List<IObserver> _observers = new List<IObserver>();
    private void Awake()
	{
		_manager = FindObjectOfType<GameManager>();
        Subscribe(_manager);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerModel>())
            Destroy();
    }
    public void Destroy()
    {
        NotifyObserver("Fruit");
		gameObject.SetActive(false);
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
