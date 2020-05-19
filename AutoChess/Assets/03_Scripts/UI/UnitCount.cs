using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCount : MonoBehaviour
{
    public int currentUnit;
    public int maxUnit;
    List<GameObject> objList;

    private void Awake()
    {
        currentUnit = 0;
        maxUnit = 2;
        SetupList();
    }
    void SetupList()
    {
        GameObject parrent = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI);

        objList = new List<GameObject>();
        objList.Add(parrent.transform.Find("MaxUnit").gameObject);
        objList.Add(parrent.transform.Find("ExpBall").Find("MaxUnit").gameObject);
    }

    #region finished works
    public void UpdateUnit(int count)
    {
        currentUnit = count;
        UpdateTexts();
    }
    public void AddUnit(int add)
    {
        currentUnit += add;
        UpdateTexts();
    }
    public void IncreaceMax()
    {
        maxUnit++;
        UpdateTexts();
    }
    void UpdateTexts()
    {
        Text uiText;
        foreach(var ele in objList)
        {
            uiText = ele.transform.Find("Current").GetComponent<Text>();
            uiText.text = currentUnit.ToString();

            uiText = ele.transform.Find("Max").GetComponent<Text>();
            uiText.text = "/" + maxUnit.ToString();
        }
    }
    #endregion
}
