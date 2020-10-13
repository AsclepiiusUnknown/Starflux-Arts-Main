using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class Pathfinding : MonoBehaviour
    {
        Grid grid;
        public Transform startPos;
        public Transform targetPos;

        private void Awake()
        {
            grid = GetComponent<Grid>();

        }

        private void Update()
        {
            FindPath(startPos.position, targetPos.position);
        }

        public void FindPath(Vector3 _startPos, Vector3 _targetPos)
        {
            Node startNode = grid.NodeFromWorldPos(_startPos);
            Node targetNode = grid.NodeFromWorldPos(_targetPos);

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>(); //Note: hash sets dont hold values or variables, hence being used to store unnescecary nodes

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < currentNode.fCost || (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    GetFinalPath(startNode, targetNode);
                }

                foreach (Node neighborNode in grid.GetNeighborNodes(currentNode))
                {
                    if (!neighborNode.isWall || closedList.Contains(neighborNode))
                    {
                        continue;
                    }
                    int moveCost = currentNode.gCost + GetManhattenDist(currentNode, neighborNode);

                    if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                    {
                        neighborNode.gCost = moveCost;
                        neighborNode.hCost = GetManhattenDist(neighborNode, targetNode);
                        neighborNode.parent = currentNode;

                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }
        }

        void GetFinalPath(Node _startNode, Node _endNode)
        {
            List<Node> finalPath = new List<Node>();
            Node currentNode = _endNode;

            while (currentNode != _startNode)
            {
                finalPath.Add(currentNode);
                currentNode = currentNode.parent;
            }

            finalPath.Reverse();

            grid.finalPath = finalPath;
        }

        int GetManhattenDist(Node _nodeA, Node _nodeB)
        {
            int ix = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
            int iy = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

            return ix + iy;
        }
    }
}