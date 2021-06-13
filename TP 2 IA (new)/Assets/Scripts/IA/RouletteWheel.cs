using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheel
{    
    public ActionNode<string> ExecuteAction(Dictionary<ActionNode<string>, int> actions)
    {
        int totalWeight = 0;

        foreach (var item in actions)
        {
            totalWeight += item.Value;
        }

        int random = Random.Range(0, totalWeight);

        foreach (var item in actions)
        {
            random -= item.Value;

            if (random < 0)
            {
                return item.Key;
            }
        }
        return default;
    }

    public string Execute(Dictionary<string, int> options)
    {
        int totalWeight = 0;

        foreach (var item in options)
        {
            totalWeight += item.Value;
        }

        int random = Random.Range(0, totalWeight);

        foreach (var item in options)
        {
            random -= item.Value;

            if (random < 0)
            {
                return item.Key;
            }
        }
        return default(string);
    }
}
