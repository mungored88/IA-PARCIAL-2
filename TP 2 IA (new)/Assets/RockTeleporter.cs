using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTeleporter : MonoBehaviour
{
    public Transform destiny;

    public void Teleport(Transform obj)
    {
        obj.position = destiny.position;
    }
}
