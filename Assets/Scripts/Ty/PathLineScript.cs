using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class PathLineScript : MonoBehaviour
    {
        public void ShowPath(Vector3 startPos, List<Vector3> pathList, int nodeCount = 0)
        {
            if (nodeCount != 0 && nodeCount < pathList.Count)
            {
                pathList.RemoveRange(nodeCount, pathList.Count - nodeCount);
            }
            if (pathList.Count > 0)
            {
                GetComponent<LineRenderer>().positionCount = pathList.Count + 1;
                pathList.Insert(0, startPos);
                GetComponent<LineRenderer>().SetPositions(pathList.ToArray());
                transform.position = pathList[0];
            }
        }

        public void RemoveSelf()
        {
            Destroy(gameObject);
        }
    }
}