using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : ISteering
{
    public delegate void WpAction();
    public PathNode lastPoint => listWaypoints[currentIndex];
    WpAction wpAction = delegate { };

    Transform from;
    List<PathNode> listWaypoints;
    Vector3 nextTarget;
    float speedFrom;

    int currentIndex;
    bool goingBackwards;

    public Waypoint(WpAction pWpAction, Transform pFrom, List<PathNode> pWaypoints, float pSpeedFrom)
    {
        from = pFrom;
        listWaypoints = pWaypoints;
        wpAction = pWpAction;
        speedFrom = pSpeedFrom;
    }

    public Vector3 GetDirection()
    {
        nextTarget = listWaypoints[currentIndex].transform.position;

        if (Math.Ceiling(Vector3.Distance(listWaypoints[currentIndex].transform.position, from.position)) <= Math.Ceiling(Time.deltaTime * speedFrom))
        {
            wpAction();

            if (currentIndex <= 0)
            {
                goingBackwards = false;
            }
            else if (currentIndex >= listWaypoints.Count - 1)
            {
                goingBackwards = true;
            }

            if (!goingBackwards)
            {
                currentIndex++;
            }
            else
            {
                currentIndex--;
            }
        }

        return (nextTarget - from.position).normalized;
    }
}
