using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    int slotCount;
    private List<GameObject> slots;
    List<ChampionPool.ChampInstance> currentRoll;
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
        currentRoll = roll;
    }
    public void InitColliderPos()
    {
        for(int i=0; i<slotCount; i++)
        {
            slots[i].transform.position =
                currentRoll[i].champion.transform.position;
            slots[i].transform.Translate(new Vector3(0, 1, 0));
        }
    }
}
