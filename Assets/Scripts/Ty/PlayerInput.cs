using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class PlayerInput : MonoBehaviour
    {
        UnitScript currentUnitRef;

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
                                FindObjectOfType<UnitHUDScript>().ShowUnitInfo(currentUnitRef.InfoStruct);
                            }
                        }
                    }
                    else
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Is valid position
                            //currentUnitRef.SelectMovePosition(FindObjectOfType<AStar.Pathfinding>().FindPath(currentUnitRef.transform.position, hit.point));
                        }
                    }
                }
                else
                {
                    //FindObjectOfType<AStar.Pathfinding>().FindPath(currentUnitRef.transform.position, Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
                }
            }
        }
    }
}