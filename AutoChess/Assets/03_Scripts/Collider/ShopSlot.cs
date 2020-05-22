using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChampInstance = ChampionPool.ChampInstance;
public class ShopSlot : MonoBehaviour
{
    ShopCollider shopCollider;
    RollChampions shopContainer;
    Inventory inven;

    GoldUi gold;
    UnitCount unitCount;
    MessageBox msg;

    private void Awake()
    {
        shopCollider = transform.parent.GetComponent<ShopCollider>();
        shopContainer = MyFunc.GetObject(MyFunc.ObjType.SHOP_CONTAINER).GetComponent<RollChampions>();
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();

        gold = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();
        msg = MyFunc.GetObject(MyFunc.ObjType.MESSAGE_BOX).GetComponent<MessageBox>();
    }

    private void OnMouseDown()
    {
        int champIdx = shopCollider.GetSlotIdx(gameObject);

        // 골드가 충분하다면, 인벤에 자리가 있다면
        if (inven.IsRemainInven())
        {
            if (gold.gold >= 2)
            {
                BuyChampion(champIdx, inven.inven, inven.gameObject);
            }
            else
            {
                msg.OnMessageBox(MessageBox.MessageType.NOT_ENOUGH_GOLD);
            }
        }
        else
        {
            msg.OnMessageBox(MessageBox.MessageType.NOT_ENOUGH_INVEN);
        }
    }

    void BuyChampion(int champIdx, List<ChampInstance> myInven, GameObject myInvenObj)
    {
        string champName;
        // Roll champions 에서 챔피언 지우기
        champName = shopContainer.ReleaseChamp(champIdx);
        gold.AddGold(-2);                       // 골드 깎기
        inven.IntoInventory(champName, true);   // 인벤토리에 챔피언 넣기

        // 스스로 Set false 하기
        gameObject.SetActive(false);
    }
}
