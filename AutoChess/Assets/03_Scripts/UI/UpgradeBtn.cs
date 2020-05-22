using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBtn : MonoBehaviour
{
    Inventory inven;
    GameObject upgradeButton;
    private void Awake()
    {
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        upgradeButton = transform.Find("Image").gameObject;
    }

    public void OnClick()
    {
        inven.QualityUpChampion();
    }

    public void VisibleButton(bool isVisible)
    {
        upgradeButton.SetActive(isVisible);
    }
}
