using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    public delegate T Factory();
    private Factory _factory;
    public List<T> stock;
    private bool _dynamicStock;

    private Action<T> _turnOn;
    private Action<T> _turnOff;

    public ObjectPool(Factory factory, Action<T> turnOn, Action<T> turnOff, int initialStock = 0, bool dynamic = true)
    {
        _factory = factory;
        _turnOn = turnOn;
        _turnOff = turnOff;
        _dynamicStock = dynamic;

        stock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            var createdObject = _factory();
            _turnOff(createdObject);
            stock.Add(createdObject);
        }
    }

    public T PickObjectFromPool()
    {
        var obj = default(T);

        if (stock.Count > 0)
        {
            obj = stock[0];
            stock.RemoveAt(0);
        }
        else if (_dynamicStock)
        {
            obj = _factory();
        }
        _turnOn(obj);
        return obj;
    }

    public void ReturnObjectoToPool(T obj)
    {
        _turnOff(obj);
        stock.Add(obj);
    }
}
