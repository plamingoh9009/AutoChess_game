using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAwake : MonoBehaviour
{
    GameObject shopObj;
    RollChampions rollChamps;
    ShopCollider shopCollider;
    private void Awake()
    {
        shopObj = transform.Find("ShopObj").gameObject;
        shopObj.SetActive(true);

        shopCollider = MyFunc.GetObject(MyFunc.ObjType.SHOP_COLLIDER).GetComponent<ShopCollider>();
        rollChamps = transform.Find("ShopObj/ChampContainer").GetComponent<RollChampions>();
    }
    private void Start()
    {
        StartCoroutine(WaitAndStart());
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        rollChamps.SetupShopCollider(shopCollider);
    }
}
