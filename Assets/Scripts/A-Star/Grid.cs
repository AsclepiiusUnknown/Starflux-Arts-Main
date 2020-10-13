using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class Grid : MonoBehaviour
    {
        public Transform startPos; //where pathfinding begins from
        public LayerMask wallMask; //compared against when testing for obstructions
        public Vector2 gridWorldSize; //width & height of grid in real-world units
        public float nodeRadius; //size of nodes
        public float dist; //node spacing

        // [HideInInspector]
        public Node[,] grid; //our grid array (2D is easier to read but harder to manage)
        public List<Node> finalPath; //completed path the algorithm finds

        float nodeDiameter; // doubled radius
        int gridSizeX, gridSizeY; //stores position of node in array units

        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool wall = true;

                    if (Physics.CheckSphere(worldPoint, nodeRadius, wallMask))
                        wall = false;

                    grid[x, y] = new Node(wall, worldPoint, x, y);
                }
        }

        public Node NodeFromWorldPos(Vector3 _worldPos)
        {
            float xPoint = ((_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
            float yPoint = ((_worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

            xPoint = Mathf.Clamp01(xPoint);
            yPoint = Mathf.Clamp01(yPoint);

            int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
            int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

            return grid[x, y];
        }

        public List<Node> GetNeighborNodes(Node _node)
        {
            List<Node> neighboringNodes = new List<Node>();
            int xCheck;
            int yCheck;

            //right side
            xCheck = _node.gridX + 1;
            yCheck = _node.gridY;
            if (xCheck >= 0 && xCheck < gridSizeX)
            {
                if (yCheck >= 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            //left side
            xCheck = _node.gridX - 1;
            yCheck = _node.gridY;
            if (xCheck >= 0 && xCheck < gridSizeX)
            {
                if (yCheck >= 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            //top side
            xCheck = _node.gridX;
            yCheck = _node.gridY + 1;
            if (xCheck >= 0 && xCheck < gridSizeX)
            {
                if (yCheck >= 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            //down side
            xCheck = _node.gridX;
            yCheck = _node.gridY - 1;
            if (xCheck >= 0 && xCheck < gridSizeX)
            {
                if (yCheck >= 0 && yCheck < gridSizeY)
                {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }

            return neighboringNodes;
        }

        //Draws the wireframe
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); //draw wire cube with given dimensions

            if (grid != null)//if the grid is not empty
            {
                foreach (Node node in grid)//for wach node in the grid
                {
                    if (node.isWall) //if its a wall
                    {
                        Gizmos.color = Color.white; //make it white
                    }
                    else
                    {
                        Gizmos.color = Color.yellow; //make it yellow
                    }

                    if (finalPath != null) //if the final path is not empty
                    {
                        if (finalPath.Contains(node)) //if the current node is in the final path
                        {
                            Gizmos.color = Color.red; //make it red
                        }
                    }

                    Gizmos.DrawCube(node.pos, Vector3.one * (nodeDiameter - dist)); //draw node at its pos
                }
            }
        }
    }
}