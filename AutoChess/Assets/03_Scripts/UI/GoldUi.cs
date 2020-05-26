using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUi : MonoBehaviour
{
    public int gold;
    int interest;
    Text currentGold;

    GoldStatue goldStatue;
    private void Awake()
    {
        gold = 4;
        interest = 5;
        goldStatue = default;
        currentGold = default;
    }
    public void SetupObjs(GameObject myStatue, GameObject myGoldTextObj = default)
    {
        goldStatue = myStatue.GetComponent<GoldStatue>();
        if (myGoldTextObj != default)
        {
            currentGold = myGoldTextObj.transform.Find("Current").GetComponent<Text>();
        }
        UpdateText();
    }
    public void AddGold(int add)
    {
        gold += add;
        UpdateText();
        goldStatue.SyncStatueWithGold();
    }
    public void AddInterest()
    {
        int myInterest = interest + (int)(gold / 10f);
        gold += myInterest;
        UpdateText();
        goldStatue.SyncStatueWithGold();
    }
    void UpdateText()
    {
        if (currentGold != default)
        {
            currentGold.text = gold.ToString();
        }
    }
}
