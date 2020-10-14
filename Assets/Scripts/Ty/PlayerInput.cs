using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class PlayerInput : MonoBehaviour
    {
        UnitScript currentUnitRef;
        public GameObject pathLineRenderer;


        private void Update()
        {
            if (TurnScript.IsPlayerTurn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!currentUnitRef)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject.GetComponent<UnitScript>() && hit.collider.gameObject.GetComponent<UnitScript>().PlayerControlled)
                            {
                                currentUnitRef = hit.collider.gameObject.GetComponent<UnitScript>();
                                currentUnitRef.SelectUnit();
                                print("Select Unit");
                            }
                        }
                    }
                    else if (!currentUnitRef.Moving)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Is valid position
                            currentUnitRef.SelectMovePosition(currentUnitRef.GetPathListFromPathfinder(hit.point));
                            if (FindObjectOfType<PathLineScript>())
                            {
                                FindObjectOfType<PathLineScript>().RemoveSelf();
                            }
                        }
                    }
                }
                else if (currentUnitRef && !currentUnitRef.Moving)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.layer == 0) //Temp check, replace with more complex one.
                        {
                            GameObject lne;
                            if (FindObjectOfType<PathLineScript>())
                            {
                                lne = FindObjectOfType<PathLineScript>().gameObject;
                            }
                            else
                            {
                                lne = Instantiate(pathLineRenderer);
                            }
                            lne.transform.position = currentUnitRef.transform.position;
                            lne.GetComponent<Ty.PathLineScript>().ShowPath(currentUnitRef.transform.position, currentUnitRef.GetPathListFromPathfinder(hit.point), currentUnitRef.InfoStruct.TileMoveSpeed);
                        }
                    }   
                }
            }
        }

        public void RemovePlayerUnitRef()
        {
            currentUnitRef = null;
        }
    }
}