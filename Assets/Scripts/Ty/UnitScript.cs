using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class UnitScript : MonoBehaviour
    {
        [SerializeField]
        bool playerControlled;
        [SerializeField]
        UnitInformation infoStruct;
        public UnitInformation InfoStruct
        {
            get
            {
                return infoStruct;
            }
        }

        public void SelectUnit()
        {
            if (playerControlled)
            {
                //Unit selected
            }
        }

        void SelectMovePosition()
        {
            if (!playerControlled)
            {
                //Selects position to move to
            }
        }
    }

    [System.Serializable]
    public struct UnitInformation
    {
        [SerializeField]
        List<GameObject> gadgetList;
        [SerializeField]
        int maxHealth;
        [SerializeField]
        int tileMoveSpeed;
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