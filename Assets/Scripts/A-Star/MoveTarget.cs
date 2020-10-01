using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class MoveTarget : MonoBehaviour
    {
        public LayerMask hitLayers;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;

                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
                {
                    this.transform.position = hit.point;
                }
            }
        }
    }
}