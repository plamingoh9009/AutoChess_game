using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    int slotCount;
    private List<GameObject> slots;
    List<ChampionPool.ChampInstance> InitRoll;
    public class ChampInfo
    {
        public string name;
        public int idx;
    }
    #region Start()
    private void Awake()
    {
        slots = new List<GameObject>();
        slotCount = transform.childCount;
        for(int i=0; i<slotCount; i++)
        {
            slots.Add(transform.GetChild(i).gameObject);
        }// loop: 사용하기 편하게 리스트에 담는다.
    }

    public void PushCurrentRoll(List<ChampionPool.ChampInstance> roll)
    {
        // RollChampions로부터 curruntroll을 받아온다
        InitRoll = roll;
    }
    public void InitColliderPos()
    {
        for(int i=0; i<slotCount; i++)
        {
            slots[i].transform.position =
                InitRoll[i].champion.transform.position;
            slots[i].transform.Translate(new Vector3(0, 1, 0));
        }
    }
    #endregion

    public int GetSlotIdx(GameObject slot)
    {
        for(int i=0; i<slots.Count; i++)
        {
            if(slots[i].transform.position == slot.transform.position)
            {
                return i;
            }
        }
        return -1;
    }
    public void ActiveAllColliders()
    {
        foreach(var slot in slots)
        {
            slot.SetActive(true);
        }
    }
}
