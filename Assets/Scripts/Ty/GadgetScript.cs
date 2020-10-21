using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ty
{
    public class GadgetScript : MonoBehaviour
    {
        [SerializeField] private GadgetInfoStruct info;
        public GadgetInfoStruct Info
        {
            get
            {
                return info;
            }
        }
        public UnitScript unitHolding;

        public void SelectPosition(Vector3 pos)
        {
            print("Gadget placed at " + pos);
            if (info.unequipOnUse)
            {
                unitHolding.UnequipGadget();
            }
        }
    }

    [Serializable]
    public struct GadgetInfoStruct
    {
        [SerializeField]
        public string gadgetName;
        public int cost;
        public GameObject prefab;
        public int controlType;
        public bool unequipOnUse;
    }
}