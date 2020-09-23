using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    //List<Transform> gridPoints;
    public float gridDistance;
    public float height;
    public float width;

    public Vector3 GetNearestPoint(Vector3 vector)
    {
        Vector3 mVec = new Vector3(Mathf.Clamp(vector.x, -((width / 2) * gridDistance), ((width / 2) * gridDistance)), 0, Mathf.Clamp(vector.z, -((height / 2) * gridDistance), ((height / 2) * gridDistance)));
        mVec /= gridDistance;
        mVec = new Vector3(Mathf.Round(mVec.x), 0, Mathf.Round(mVec.z));
        mVec *= gridDistance;

        /*
        int index = 0;
        float distance = Vector3.Distance(vector, new Vector3(gridPoints[0].position.x, 0, gridPoints[0].position.y));
        for (int i = 0; i < gridPoints.Count; i++)
        {
            if (Vector3.Distance(vector, new Vector3(gridPoints[i].position.x, 0, gridPoints[i].position.y)) <= distance)
            {
                index = i;
            }
        }
        */

        return mVec;
    }
}