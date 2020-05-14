using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    ShopCollider shopCollider;
    RollChampions shopContainer;
    Inventory inven;

    private void Awake()
    {
        shopCollider = transform.parent.GetComponent<ShopCollider>();
        shopContainer = MyFunc.GetObject(MyFunc.ObjType.SHOP_CONTAINER).GetComponent<RollChampions>();
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
    }

    private void OnMouseDown()
    {
        int champIdx = shopCollider.GetSlotIdx(gameObject);

        // 골드가 충분하다면, 인벤에 자리가 있다면
        if (inven.IsRemainInven())
        {
            BuyChampion(champIdx);
        }
    }

    void BuyChampion(int champIdx)
    {
        string champName;
        // Roll champions 에서 챔피언 지우기
        champName = shopContainer.ReleaseChamp(champIdx);
        // 골드 깎기
        // 인벤토리에 챔피언 넣기
        inven.IntoInventory(champName);

        // 스스로 Set false 하기
        gameObject.SetActive(false);
    }
}
