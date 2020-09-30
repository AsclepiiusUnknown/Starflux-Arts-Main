using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class GridBehaviour : MonoBehaviour
    {
        public bool findDist = false;

        public int rows = 10;
        public int columns = 10;
        public int scale = 1;
        public GameObject gridPrefab;
        public Vector3 startPos = Vector3.zero; //Bottom left position

        public GameObject[,] gridArray;
        public int startX = 0;
        public int startY = 0;
        public int endX = 2;
        public int endY = 2;

        public List<GameObject> path = new List<GameObject>();

        private void Awake()
        {
            gridArray = new GameObject[columns, rows];

            if (gridPrefab)
                GenerateGrid();
            else
                print("**NULL**\nMissing GridPrefab");
        }

        private void Update()
        {
            if (findDist)
            {
                SetDistance();
                SetPath();
                findDist = false;
            }
        }

        void GenerateGrid()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    GameObject gridGO = Instantiate(gridPrefab, new Vector3(startPos.x + scale * x, startPos.y, startPos.z + scale * y), Quaternion.identity);
                    gridGO.transform.SetParent(gameObject.transform);
                    gridGO.GetComponent<GridStat>().x = x;
                    gridGO.GetComponent<GridStat>().y = y;
                    gridArray[x, y] = gridGO;
                }
            }
        }

        void SetDistance()
        {
            InitialSetup();
            int x = startX;
            int y = startY;
            int[] testArray = new int[rows * columns];

            for (int step = 1; step < rows * columns; step++)
            {
                foreach (GameObject obj in gridArray)
                {
                    if (obj && obj.GetComponent<GridStat>().visited == step - 1)
                        TestFourDir(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
                }
            }
        }

        void SetPath()
        {
            int step;
            int x = endX;
            int y = endY;
            List<GameObject> tempList = new List<GameObject>();
            path.Clear();
            if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
            {
                path.Add(gridArray[x, y]);
                step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
            }
            else
            {
                print("Cant reach desired location");
                return;
            }

            for (int i = step; step > -1; step--)
            {
                if (TestDirection(x, y, step, 1))
                    tempList.Add(gridArray[x, y + 1]);
                if (TestDirection(x, y, step, 2))
                    tempList.Add(gridArray[x + 1, y]);
                if (TestDirection(x, y, step, 3))
                    tempList.Add(gridArray[x, y - 1]);
                if (TestDirection(x, y, step, 4))
                    tempList.Add(gridArray[x - 1, y]);

                GameObject tempGO = FindClosest(gridArray[endX, endY].transform, tempList);
                path.Add(tempGO);
                x = tempGO.GetComponent<GridStat>().x;
                y = tempGO.GetComponent<GridStat>().y;
                tempList.Clear();
            }

            InvertPath();
        }

        void InitialSetup()
        {
            foreach (GameObject obj in gridArray)
            {
                obj.GetComponent<GridStat>().visited = -1;
            }
            gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
        }

        bool TestDirection(int x, int y, int step, int dir)
        {
            switch (dir)
            {
                case 1:
                    if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
                        return true;
                    else
                        return false;
                case 2:
                    if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                        return true;
                    else
                        return false;
                case 3:
                    if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                        return true;
                    else
                        return false;
                case 4:
                    if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
                        return true;
                    else
                        return false;
            }

            return false;
        }

        void TestFourDir(int x, int y, int step)
        {
            if (TestDirection(x, y, -1, 1))
                SetVisited(x, y + 1, step);
            if (TestDirection(x, y, -1, 2))
                SetVisited(x + 1, y, step);
            if (TestDirection(x, y, -1, 3))
                SetVisited(x, y - 1, step);
            if (TestDirection(x, y, -1, 4))
                SetVisited(x - 1, y, step);
        }

        void SetVisited(int x, int y, int step)
        {
            if (gridArray[x, y])
                gridArray[x, y].GetComponent<GridStat>().visited = step;
        }

        GameObject FindClosest(Transform targetPos, List<GameObject> list)
        {
            float currentDist = scale * rows * columns;
            int index = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (Vector3.Distance(targetPos.position, list[i].transform.position) < currentDist)
                {
                    currentDist = Vector3.Distance(targetPos.position, list[i].transform.position);
                    index = i;
                }
            }
            return list[index];
        }

        private void InvertPath()
        {
            path.Reverse();
        }
    }
}