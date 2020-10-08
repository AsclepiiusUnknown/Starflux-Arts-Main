using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHUDScript : MonoBehaviour
{
    public GameObject gadgetButtonPrefab;
    public GameObject gadgetButtonParentRef;
    public GameObject unitInfoPanelRef;

    void GadgetButtonSetup()
    {
        //temp
        for (int i = 0; i < 3; i++)
        {
            GameObject gdjt = Instantiate(gadgetButtonPrefab);
            gdjt.transform.SetParent(gadgetButtonParentRef.transform);
            gdjt.transform.position = new Vector2(gadgetButtonParentRef.transform.position.x, i * 50f);

        }
    }

    private void Start()
    {
        ShowUnitInfo();
        Invoke("HideUnitInfo", 3f);
    }

    public void ShowUnitInfo()
    {
        GadgetButtonSetup();
        unitInfoPanelRef.SetActive(true);
    }

    public void HideUnitInfo()
    {
        //Remove buttons from parent here

        unitInfoPanelRef.SetActive(false);
    }
}
