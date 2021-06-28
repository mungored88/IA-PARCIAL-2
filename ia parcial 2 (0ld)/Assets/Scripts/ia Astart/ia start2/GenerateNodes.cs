using UnityEngine;

public class GenerateNodes : MonoBehaviour
{
    public GameObject node;

    public int distanceBetween;
    public int inX;
    public int inY;

    //void Start()
    //{
    //    GenerateNodesInScene(distanceBetween, inY, inX);
    //}

    private void GenerateNodesInScene(float distBetweenNodes, float yCant, int xCant)
    {
        float xBase = 0.5f;
        float zBase = 0.5f;
        int num = 0;

        for (int i = 0; i < xCant; i++)
        {
            for (int j = 0; j < yCant; j++)
            {
                var nod = Instantiate(node);
                nod.transform.position = transform.position + new Vector3(xBase - j * distBetweenNodes, 1, zBase);
                nod.name = num + "";
                nod.transform.parent = this.transform;
                num++;
            }
            zBase += distBetweenNodes;
        }
    }
}
