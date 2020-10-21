using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class PlayerInput : MonoBehaviour
    {
        UnitScript currentUnitRef;
        public UnitScript CurrentUnitRef
        {
            get
            {
                return currentUnitRef;
            }
        }
        public GameObject pathLineRenderer;
        bool isGadgetControl = false;
        int gadgetControlType = 0;

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
                    else if (!(currentUnitRef.Moving || isGadgetControl))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Is valid position
                            currentUnitRef.SelectMovePosition(currentUnitRef.GetPathListFromPathfinder(hit.point));
                            EndLine();
                        }
                    }
                    else
                    {
                        switch (gadgetControlType)
                        {
                            default:
                                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                                RaycastHit hit;
                                if (Physics.Raycast(ray, out hit))
                                {
                                    if (!hit.collider.gameObject.GetComponent<UnitScript>())
                                    {
                                        currentUnitRef.HeldGadget.SelectPosition(hit.point);
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (isGadgetControl)
                    {
                        currentUnitRef.UnequipGadget();
                    }
                    else
                    {
                        RemovePlayerUnitRef();
                        EndLine();
                    }
                }
                else if (currentUnitRef && !currentUnitRef.Moving && !isGadgetControl)
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

        public void EndLine()
        {
            if (FindObjectOfType<PathLineScript>())
            {
                FindObjectOfType<PathLineScript>().RemoveSelf();
            }
        }

        public void RemovePlayerUnitRef()
        {
            currentUnitRef = null;
        }

        public void SetGadgetControl(int controlType)
        {
            isGadgetControl = true;
            gadgetControlType = controlType;
        }

        public void RemoveGadgetControl()
        {
            isGadgetControl = false;
            gadgetControlType = -1;
        }
    }
}