using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class GridMouse : MonoBehaviour
    {
        public float smooth;
        public float speed;

        private float z;
        private float x;
        private Vector3 targetPosition;

        public GridBehaviour behaviour;
        public GameObject[,] gridArray;

        void Update()
        {
            if (gridArray == null)
                gridArray = behaviour.gridArray;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                speed = 1;
                smooth = 20;

                RaycastHit hit;

                //var playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //double hitdist = 0.0;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPosition = hit.point;

                    x = hit.point.x;
                    z = hit.point.z;
                    //var targetRotation = Quaternion.LookRotation(targetPoint    - transform.position);
                    //transform.rotation = targetRotation;

                    if (gridArray != null)
                    {
                        float max = behaviour.rows * behaviour.columns * gridArray.Length;
                        float closestDist = max;
                        GameObject closestObj = FindObjectOfType<GameObject>();
                        Vector3 clickPos = new Vector3(targetPosition.x, 0, targetPosition.z);
                        int closestX = 0;
                        int closestY = 0;

                        for (int x = 0; x < behaviour.columns; x++)
                        {
                            for (int y = 0; y < behaviour.rows; y++)
                            {
                                if (Vector3.Distance(clickPos, gridArray[x, y].transform.position) < closestDist)
                                {
                                    closestDist = Vector3.Distance(clickPos, gridArray[x, y].transform.position);
                                    closestObj = gridArray[x, y];
                                    closestX = x;
                                    closestY = y;
                                }
                            }
                        }

                        behaviour.endX = closestX;
                        behaviour.endY = closestY;
                        behaviour.findDist = true;

                        /**foreach (GameObject obj in gridArray)
                        {
                            if (Vector3.Distance(clickPos, obj.transform.position) < closestDist)
                            {
                                closestObj = obj;
                            }
                        }**/
                    }
                }
            }
        }
    }
}