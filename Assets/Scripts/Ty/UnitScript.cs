using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class UnitScript : MonoBehaviour
    {
        [SerializeField]
        bool playerControlled;
        public bool PlayerControlled
        {
            get
            {
                return playerControlled;
            }
        }
        [SerializeField]
        UnitInformation infoStruct;
        public UnitInformation InfoStruct
        {
            get
            {
                return infoStruct;
            }
        }
        bool moving;
        public bool Moving
        {
            get
            {
                return moving;
            }
        }
        bool movedThisTurn;
        public bool MovedThisTurn
        {
            get
            {
                return movedThisTurn;
            }
            set
            {
                movedThisTurn = value;
            }
        }
        public Transform holdPosition;
        GadgetScript heldGadget;
        public GadgetScript HeldGadget
        {
            get
            {
                return heldGadget;
            }
        }
        List<Vector3> movePositionList = new List<Vector3>();
        int moveIndex;
        public float moveSpeed = 3f;

        public void SelectUnit()
        {
            if (playerControlled && !MovedThisTurn)
            {
                FindObjectOfType<UnitHUDScript>().ShowPlayerHUD();
                FindObjectOfType<UnitHUDScript>().ShowUnitInfo(InfoStruct);
            }
        }

        public void SpawnGadget(GadgetInfoStruct gadgetIn)
        {
            UnequipGadget();
            GadgetScript gadg = Instantiate(gadgetIn.prefab, holdPosition.position, holdPosition.rotation, holdPosition).GetComponent<GadgetScript>();
            heldGadget = gadg;
            if (playerControlled)
            {
                FindObjectOfType<PlayerInput>().SetGadgetControl(gadg.Info.controlType);
                FindObjectOfType<PlayerInput>().EndLine();
            }
            gadg.unitHolding = this;
        }

        public void UnequipGadget()
        {
            if (heldGadget)
            {
                Destroy(heldGadget.gameObject);
                heldGadget = null;
                if (playerControlled)
                {
                    FindObjectOfType<PlayerInput>().RemoveGadgetControl();
                }
            }
        }

        public void SelectMovePosition(List<Vector3> positions)
        {
            if (infoStruct.TileMoveSpeed < positions.Count)
            {
                positions.RemoveRange(infoStruct.TileMoveSpeed, positions.Count - infoStruct.TileMoveSpeed);
            }
            if (positions.Count > 0)
            {
                movePositionList = positions;
                moveIndex = 0;
                moving = true;
                MovedThisTurn = true;
            }
        }

        public List<Vector3> GetPathListFromPathfinder(Vector3 destination)
        {
            FindObjectOfType<AStar.Pathfinding>().FindPath(transform.position, destination);
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i < FindObjectOfType<AStar.Grid>().finalPath.Count; i++)
            {
                vectorList.Add(FindObjectOfType<AStar.Grid>().finalPath[i].pos);
            }
            return vectorList;
        }

        private void Update()
        {
            if (moving)
            {
                if (moveIndex < movePositionList.Count)
                {
                    if (Vector3.Distance(transform.position, movePositionList[moveIndex]) > 0.1f)
                    {
                        transform.position = transform.position + (Vector3.Normalize(movePositionList[moveIndex] - transform.position)) * moveSpeed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = movePositionList[moveIndex];
                        moveIndex++;
                    }
                }
                else
                {
                    moving = false;
                    if (playerControlled)
                    {
                        FindObjectOfType<TurnScript>().PlayerFinishedMove();
                    }
                    else
                    {
                        FindObjectOfType<TurnScript>().EnemyCompleteMovement();
                    }
                }
            }
        }

        public static List<UnitScript> GetAllUnitsOfControlType(bool player)
        {
            UnitScript[] units = FindObjectsOfType<UnitScript>();
            List<UnitScript> unitsOut = new List<UnitScript>();
            for (int i = 0; i < units.Length; i++)
            {
                if ((units[i].PlayerControlled && player) || (!units[i].PlayerControlled && !player))
                {
                    unitsOut.Add(units[i]);
                }
            }
            return unitsOut;
        }
    }

    [System.Serializable]
    public struct UnitInformation
    {
        [SerializeField]
        List<GadgetInfoStruct> gadgetList;
        public List<GadgetInfoStruct> GadgetList
        {
            get
            {
                return gadgetList;
            }
        }
        [SerializeField]
        int maxHealth;
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }
        [SerializeField]
        int tileMoveSpeed;
        public int TileMoveSpeed
        {
            get
            {
                return tileMoveSpeed;
            }
        }
        /*
        public UnitInformation(List<GadgetScript> gadgets, int health, int movement)
        {
            this.gadgetList = gadgets;
            this.maxHealth = health;
            this.tileMoveSpeed = movement;
        }
        */
    }
}