using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    List<Transform> gridPoints;

    public Vector3 GetNearestPoint(Vector3 vector)
    {
        int index = 0;
        float distance = Vector3.Distance(vector, new Vector3(gridPoints[0].position.x, 0, gridPoints[0].position.y));
        for (int i = 0; i < gridPoints.Count; i++)
        {
            if (Vector3.Distance(vector, new Vector3(gridPoints[i].position.x, 0, gridPoints[i].position.y)) <= distance)
            {
                index = i;
            }
        }

        return new Vector3(gridPoints[index].position.x, 0, gridPoints[index].position.y);
    }
}