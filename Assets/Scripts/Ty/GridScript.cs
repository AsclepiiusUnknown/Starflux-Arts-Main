using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class GridScript : MonoBehaviour
    {
        //List<Transform> gridPoints;
        public float gridDistance;
        public GameObject gridPart;
        public float height;
        public float width;
        List<GameObject> gridPoints = new List<GameObject>();

        private void Awake()
        {
            Vector3 point = new Vector3(-((width / 2) * gridDistance), 0, (height / 2) * gridDistance);
            int temp = 0;
            print(point.x);
            for (int i = 0; i < ((height + 1) * (width + 1)); i++)
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 10f);
                if (!Physics.Raycast(point, Vector3.up, 1f))
                {
                    GameObject g = Instantiate(gridPart);
                    g.transform.position = point;
                    gridPoints.Add(g);
                }
                if (point.x >= (width / 2) * gridDistance)
                {
                    point.x = -((width / 2) * gridDistance);
                    point.z -= gridDistance;
                }
                else
                {
                    point.x += gridDistance;
                }
                temp++;
                print(point);
            }
            //print(wallPoints.Count + ", " + temp);
        }

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

        public float CalculateValue(Vector2 position, Vector2 destination)
        {
            return Vector2.Distance(position, destination);
        }

        public List<Vector2> GetSurroundingPoints(Vector2 position)
        {
            // Currently unfinished
            List<Vector2> list =  new List<Vector2>();
            float posX = Mathf.Clamp(position.x, -((width / 2) * gridDistance), ((width / 2) * gridDistance));
            float posY = Mathf.Clamp(position.y, -((height / 2) * gridDistance), ((height / 2) * gridDistance));
            list.Add(new Vector2(0, 0));
            return list;
        }
    }
}