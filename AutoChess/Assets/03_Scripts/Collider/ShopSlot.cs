using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    ShopCollider shopCollider;
    RollChampions shopContainer;
    Inventory inven;
    GoldUi gold;
    UnitCount unitCount;

    private void Awake()
    {
        shopCollider = transform.parent.GetComponent<ShopCollider>();
        shopContainer = MyFunc.GetObject(MyFunc.ObjType.SHOP_CONTAINER).GetComponent<RollChampions>();
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        gold = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();
    }

    private void OnMouseDown()
    {
        int champIdx = shopCollider.GetSlotIdx(gameObject);

        // 골드가 충분하다면, 인벤에 자리가 있다면, 유닛 개수가 남는다면
        if (inven.IsRemainInven() && (gold.gold >= 2) && 
            (unitCount.currentUnit < unitCount.maxUnit))
        {
            BuyChampion(champIdx);
        }
    }

    void BuyChampion(int champIdx)
    {
        string champName;
        // Roll champions 에서 챔피언 지우기
        champName = shopContainer.ReleaseChamp(champIdx);
        gold.AddGold(-2);                   // 골드 깎기
        inven.IntoInventory(champName);     // 인벤토리에 챔피언 넣기

        // 스스로 Set false 하기
        gameObject.SetActive(false);
    }
}
