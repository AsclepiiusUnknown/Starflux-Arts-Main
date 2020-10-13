using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ty {
    public class UnitHUDScript : MonoBehaviour
    {
        public GameObject gadgetButtonPrefab;
        public GameObject gadgetButtonParentRef;
        public GameObject unitInfoPanelRef;

        void GadgetButtonSetup(List<GadgetScript> gadgetList)
        {
            for (int i = 0; i < gadgetList.Count; i++)
            {
                GameObject gdjt = Instantiate(gadgetButtonPrefab);
                gdjt.transform.SetParent(gadgetButtonParentRef.transform);
                gdjt.transform.position = new Vector2(gadgetButtonParentRef.transform.position.x, i * 50f);
                gdjt.GetComponentInChildren<Text>().text = gadgetList[i].gadgetName;
            }
        }

        private void Start()
        {
            //<Temp>
            ShowUnitInfo(new UnitInformation());
            Invoke("HideUnitInfo", 3f);
            //<\Temp>
        }

        public void ShowUnitInfo(UnitInformation unitInfo)
        {
            GadgetButtonSetup(unitInfo.GadgetList);
            unitInfoPanelRef.SetActive(true);
        }

        public void HideUnitInfo()
        {
            unitInfoPanelRef.SetActive(false);
        }

        public void EndTurn()
        {
            FindObjectOfType<TurnScript>().PlayerTurnEnd();
        }
    }
}