using AStar;
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
            AStar.Pathfinding path = FindObjectOfType<AStar.Pathfinding>();
            AStar.Grid grid = path.GetComponent<AStar.Grid>();
            switch (info.controlType)
            {
                default:
                    List<Node> nearNodes = grid.GetNeighborNodes(grid.NodeFromWorldPos(unitHolding.transform.position));
                    if (nearNodes.Contains(grid.NodeFromWorldPos(pos)))
                    {
                        print("Gadget placed at " + pos);
                        if (info.prefabSpawned)
                        {
                            GameObject obj = Instantiate(info.prefabSpawned, pos, new Quaternion(0, 0, 0, 0));
                        }
                        EndOfUse();
                        return;
                    }
                    break;
            }
            print("Invalid location.");
        }

        public void AddEffectToUnit(UnitScript unitIn)
        {
            print("Showered.");
            unitIn.MakeUnappealing();
            EndOfUse();
        }

        public void EndOfUse()
        {
            FindObjectOfType<TurnScript>().ChangeResource(-info.cost);
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
        public GameObject prefabSpawned;
    }
}