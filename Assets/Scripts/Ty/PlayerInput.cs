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
        public bool unpaseAllowed = true;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FindObjectOfType<UnitHUDScript>().TogglePause();
            }
            else if (TurnScript.IsPlayerTurn && !TurnScript.paused)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.GetComponent<UnitScript>() && hit.collider.GetComponent<UnitScript>().PlayerControlled && !isGadgetControl)
                        {
                            RemovePlayerUnitRef();
                            EndLine();
                            currentUnitRef = hit.collider.GetComponent<UnitScript>();
                            currentUnitRef.SelectUnit();
                            print("Select Unit");
                        }
                        else if (currentUnitRef && !(currentUnitRef.Moving || isGadgetControl || currentUnitRef.MovedThisTurn))
                        {
                            //Is valid position
                            currentUnitRef.SelectMovePosition(currentUnitRef.GetPathListFromPathfinder(hit.point));
                            EndLine();
                        }
                        else if (isGadgetControl)
                        {
                            print("Hit");
                            switch (gadgetControlType)
                            {
                                default:
                                    if (!hit.collider.GetComponent<UnitScript>() && !(hit.collider.gameObject.layer == 11))
                                    {
                                        currentUnitRef.HeldGadget.SelectPosition(hit.point);
                                    }
                                    break;
                                case 1:
                                    if (hit.collider.GetComponent<UnitScript>() && hit.collider.GetComponent<UnitScript>() == currentUnitRef)
                                    {
                                        currentUnitRef.HeldGadget.AddEffectToUnit(hit.collider.GetComponent<UnitScript>());
                                    }
                                    break;
                            }
                        }
                    }
                }

                else if (Input.GetMouseButtonDown(1))
                {
                    if (isGadgetControl)
                    {
                        currentUnitRef.UnequipGadget();
                    }
                    else if (currentUnitRef)
                    {
                        RemovePlayerUnitRef();
                        EndLine();
                        FindObjectOfType<UnitHUDScript>().HideUnitHUD();
                    }
                }
                else if (currentUnitRef && !currentUnitRef.Moving && !isGadgetControl && !currentUnitRef.MovedThisTurn)
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
                FindObjectOfType<PathLineScript>().DestroySelf();
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