using UnityEngine;
using System.Collections;

public class WaterLine : MonoBehaviour
{
    // Mais detalhes em:
    // https://gamedevelopment.tutsplus.com/tutorials/creating-dynamic-2d-water-effects-in-unity--gamedev-14143

    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;
    LineRenderer Body;
    GameObject[] colliders;
    const float springconstant = 0.02f;
    const float damping = 0.04f;
    const float spread = 0.05f;
    const float z = -1f;
    float baseheight;

    public void SpawnWater(float Left, float Width, float Top)
    {
        int edgecount = Mathf.RoundToInt(Width) * 5;
        int nodecount = edgecount + 1;
        Body = gameObject.AddComponent<LineRenderer>();
        // Body.material = mat;
        Body.material.renderQueue = 1000;
        Body.SetVertexCount(nodecount);
        Body.SetWidth(0.1f, 0.1f);

        xpositions = new float[nodecount];
        ypositions = new float[nodecount];
        velocities = new float[nodecount];
        accelerations = new float[nodecount];

        colliders = new GameObject[edgecount];

        baseheight = Top;

        for (int i = 0; i < nodecount; i++)
        {
            ypositions[i] = Top;
            xpositions[i] = Left + Width * i / edgecount;
            accelerations[i] = 0;
            velocities[i] = 0;
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
        }

        for (int i = 0; i < edgecount; i++)
        {
            colliders[i] = new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            colliders[i].transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, 0f, 0f);
            colliders[i].transform.localScale = new Vector3(0.001f, 0.001f, 1);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();
        }
    }

    public void Splash(float xpos, float velocity)
    {
        //If the position is within the bounds of the water:
        if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length - 1])
        {
            //Offset the x position to be the distance from the left side
            xpos -= xpositions[0];

            //Find which spring we're touching
            int index = Mathf.RoundToInt((xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));

            //Add the velocity of the falling object to the spring
            velocities[index] += velocity;
        }
    }

    // Use this for initialization
    void Start()
    {
        SpawnWater(-9, 18, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < xpositions.Length; i++)
        {
            float force = springconstant * (ypositions[i] - baseheight) + velocities[i] * damping;
            accelerations[i] = -force;
            ypositions[i] += velocities[i];
            velocities[i] += accelerations[i];
            Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
        }

        float[] leftDeltas = new float[xpositions.Length];
        float[] rightDeltas = new float[xpositions.Length];

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = spread * (ypositions[i] - ypositions[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xpositions.Length - 1)
                {
                    rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }
        }

        for (int i = 0; i < xpositions.Length; i++)
        {
            if (i > 0)
            {
                ypositions[i - 1] += leftDeltas[i];
            }
            if (i < xpositions.Length - 1)
            {
                ypositions[i + 1] += rightDeltas[i];
            }
        }

    }
}
