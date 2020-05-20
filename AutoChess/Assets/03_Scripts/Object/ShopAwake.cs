using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAwake : MonoBehaviour
{
    GameObject shopObj;
    private void Awake()
    {
        shopObj = transform.Find("ShopObj").gameObject;
        shopObj.SetActive(true);
    }
}
