using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class PathLineScript : MonoBehaviour
    {
        public GameObject dotRef;
        public GameObject lineRef;
        List<LineRenderer> activeLines = new List<LineRenderer>();
        List<GameObject> activeDots = new List<GameObject>();
        bool destroying = false;

        public void ShowPath(Vector3 startPos, List<Vector3> pathList, int nodeCount = 0)
        {
            if (destroying)
            {
                return;
            }
            DestroyActiveDotsAndLines();
            if (nodeCount != 0 && nodeCount < pathList.Count)
            {
                pathList.RemoveRange(nodeCount, pathList.Count - nodeCount);
            }
            if (pathList.Count > 0)
            {
                startPos = ClipY(startPos);
                pathList.Insert(0, startPos);
                Vector3 lastDir = pathList[1] - pathList[0];
                LineRenderer currentLine = Instantiate(lineRef).GetComponent<LineRenderer>();
                activeLines.Add(currentLine);
                currentLine.positionCount = 2;
                currentLine.SetPositions(pathList.GetRange(0, 2).ToArray());
                for (int i = 2; i < pathList.Count; i++)
                {
                    if ((pathList[i] - pathList[i - 1]) != lastDir)
                    {
                        GameObject dot = Instantiate(dotRef, pathList[i - 1], new Quaternion(0, 0, 0, 0));
                        activeDots.Add(dot);
                        currentLine = Instantiate(lineRef).GetComponent<LineRenderer>();
                        currentLine.positionCount = 1;
                        currentLine.SetPosition(0, pathList[i - 1]);
                        activeLines.Add(currentLine);
                    }
                    currentLine.positionCount++;
                    currentLine.SetPosition(currentLine.positionCount - 1, pathList[i]);
                    if (i - 1 >= 0)
                    {
                        lastDir = pathList[i] - pathList[i - 1];
                    }
                }
            }
        }

        Vector3 ClipY(Vector3 vecIn)
        {
            return new Vector3(vecIn.x, 0.5f, vecIn.z);
        }

        void DestroyActiveDotsAndLines()
        {
            for (int i = activeLines.Count - 1; i >= 0; i--)
            {
                Destroy(activeLines[i].gameObject);
            }
            for (int i = activeDots.Count - 1; i >= 0; i--)
            {
                Destroy(activeDots[i]);
            }
            activeLines.Clear();
            activeDots.Clear();
        }

        public void DestroySelf()
        {
            destroying = true;
            DestroyActiveDotsAndLines();
            Destroy(gameObject);
        }
    }
}