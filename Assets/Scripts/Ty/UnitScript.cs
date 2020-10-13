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
        List<Vector3> movePositionList = new List<Vector3>();
        int moveIndex;
        public float moveSpeed = 3f;

        public void SelectUnit()
        {
            if (playerControlled)
            {
                //Unit selected
            }
        }

        public void SelectMovePosition(List<Vector3> positions)
        {
            movePositionList = positions;
            moveIndex = 0;
            moving = true;
        }

        private void Update()
        {
            if (moving)
            {
                if (moveIndex < movePositionList.Count)
                {
                    if (Vector3.Distance(transform.position, movePositionList[moveIndex]) < 0.1f)
                    {
                        transform.position = new Vector3(Mathf.Lerp(transform.position.x, movePositionList[moveIndex].x, moveSpeed * Time.deltaTime), transform.position.y, Mathf.Lerp(transform.position.z, movePositionList[moveIndex].z, moveSpeed * Time.deltaTime));
                    }
                    else
                    {
                        moveIndex++;
                    }
                }
                else
                {
                    moving = false;
                    if (!playerControlled)
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
        List<GadgetScript> gadgetList;
        public List<GadgetScript> GadgetList
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