using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class TestMovement : MonoBehaviour
    {
        public GridScript gridRef;
        UnitScript selectedUnit;

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                RaycastHit hit;

                //var playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //double hitdist = 0.0;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject.GetComponent<UnitScript>())
                    {
                        selectedUnit = hit.collider.gameObject.GetComponent<UnitScript>();
                        selectedUnit.gameObject.GetComponent<Pathfinder>().FindPath(hit.point);
                    }
                }
            }
        }
    }
}