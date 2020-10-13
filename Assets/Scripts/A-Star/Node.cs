using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [System.Serializable]
    public class Node
    {
        public int gridX; //X Pos in the node array
        public int gridY; //Y pos in the node array

        public bool isWall; //if this node is being obstructed or not
        public Vector3 pos; //world pos of the node

        public Node parent; //for A* algorithm, stores what node it previously came form to trace back shortest route

        public int gCost; //cost of moving to next square
        public int hCost; //Dist from goal to this node

        //quick get function to add G and H costs (read only)
        public int fCost { get { return gCost + hCost; } }

        //constructor to auto-set values on creation
        public Node(bool _isWall, Vector3 _pos, int _gridX, int _gridY)
        {
            isWall = _isWall; //is node being obstructed
            pos = _pos; //world pos of node
            gridX = _gridX; //X pos on node array
            gridY = _gridY; //Y pos on node array
        }
    }
}