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
        public GameObject playerPanelRef;
        public GameObject pausePanelRef;
        public GameObject resumeButton;
        public Text unitNameText;
        public Text unitMoveText;
        public Text paintText;
        public Text pauseText;

        void GadgetButtonSetup(List<GadgetInfoStruct> gadgetList)
        {
            if (gadgetButtonParentRef.transform.childCount > 0)
            {
                for (int i = gadgetButtonParentRef.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(gadgetButtonParentRef.transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < gadgetList.Count; i++)
            {
                GameObject gdjt = Instantiate(gadgetButtonPrefab);
                gdjt.transform.SetParent(gadgetButtonParentRef.transform);
                gdjt.transform.position = new Vector2(gadgetButtonParentRef.transform.position.x, i * 50f);
                gdjt.GetComponentInChildren<Text>().text = gadgetList[i].gadgetName;
                gdjt.GetComponent<GadgetMenuButtonScript>().unitRef = FindObjectOfType<PlayerInput>().CurrentUnitRef;
                gdjt.GetComponent<GadgetMenuButtonScript>().structIn = gadgetList[i];
            }
        }

        public void ShowUnitInfo(UnitScript unitRef)
        {
            GadgetButtonSetup(unitRef.InfoStruct.GadgetList);
            unitInfoPanelRef.SetActive(true);
            unitNameText.text = unitRef.InfoStruct.UnitName;
            UpdateMoveText(unitRef);
        }

        public void UpdateMoveText(UnitScript unitRef)
        {
            if (unitRef.MovedThisTurn)
            {
                unitMoveText.text = "Movement Used";
            }
            else
            {
                unitMoveText.text = "Movement Available";
            }
        }

        public void HideUnitHUD()
        {
            unitInfoPanelRef.SetActive(false);
        }

        public void ShowUnitHUD()
        {
            unitInfoPanelRef.SetActive(true);
        }

        public void ShowPlayerHUD()
        {
            playerPanelRef.SetActive(true);
        }

        public void HidePlayerHUD()
        {
            playerPanelRef.SetActive(false);
        }

        public void UpdatePaintCount(int amount)
        {
            paintText.text = "Paint: " + amount;
        }

        public void ShowPauseMenu(bool unpaseAllowed, string menuString = "Paused")
        {
            pausePanelRef.SetActive(true);
            playerPanelRef.SetActive(false);
            pauseText.text = menuString;
            TurnScript.paused = true;
            FindObjectOfType<PlayerInput>().unpaseAllowed = unpaseAllowed;
            resumeButton.SetActive(unpaseAllowed);
        }

        public void HidePauseMenu()
        {
            pausePanelRef.SetActive(false);
            playerPanelRef.SetActive(true);
            TurnScript.paused = false;
        }

        public void EndTurn()
        {
            HidePlayerHUD();
            FindObjectOfType<TurnScript>().PlayerTurnEnd();
        }

        public void TogglePause()
        {
            if (pausePanelRef.activeInHierarchy)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu(true, "Paused");
            }
        }
    }
}