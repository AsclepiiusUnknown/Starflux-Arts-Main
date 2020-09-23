using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class Pathfinder : MonoBehaviour
    {
        public GridScript gridRef;

        public void FindPath(Vector3 position, Vector3 destination)
        {
            Vector3 inDestination = gridRef.GetNearestPoint(destination);
        }
    }
}