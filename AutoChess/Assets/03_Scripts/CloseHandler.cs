using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseHandler : MonoBehaviour
{
    string _shopName;
    string _shopObjName;
    GameObject _shop;
    private void Awake()
    {
        _shopName = "Shop";
        _shopObjName = "ShopObj";
        _shop = GameObject.Find(_shopName);
    }

    public void OnClick()
    {
        _shop.transform.Find(_shopObjName).gameObject.SetActive(false);
    }
}
