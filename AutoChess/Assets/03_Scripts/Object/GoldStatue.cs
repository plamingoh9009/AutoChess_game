using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldStatue : MonoBehaviour
{
    List<GameObject> goldStatues;
    GoldUi goldUi;
    private void Awake()
    {
        goldStatues = new List<GameObject>();
    }
    public void SetupGoldStatues(GoldUi myGoldUi)
    {
        goldUi = myGoldUi;
        for (int i = 0; i < transform.childCount; i++)
        {
            goldStatues.Add(transform.GetChild(i).gameObject);
        }
    }
    public void SyncStatueWithGold()
    {
        int statueCnt = 0;
        statueCnt = (int)(goldUi.gold / 10.0);

        foreach (var ele in goldStatues)
        {
            if (statueCnt > 0)
            {
                ele.SetActive(true);
            }
            else
            {
                ele.SetActive(false);
            }
            statueCnt--;
        }
    }
}
