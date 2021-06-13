using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public Apple applePrefab;
    public int appleStock;

    public ObjectPool<Apple> applePool;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("applePool " + applePrefab.gameObject.name);
        applePool = new ObjectPool<Apple>(AppleFactory, Apple.TurnOnBullet, Apple.TurnOffBullet, appleStock, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Apple AppleFactory()
    {
        return Instantiate(applePrefab);
    }

    public void ReturnAppleToPool(Apple apple)
    {
        applePool.ReturnObjectoToPool(apple);
    }
}
