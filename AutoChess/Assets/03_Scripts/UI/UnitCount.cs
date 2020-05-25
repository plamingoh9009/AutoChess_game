using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCount : MonoBehaviour
{
    public int currentField;
    public int maxUnit;
    public List<GameObject> objList;

    private void Awake()
    {
        currentField = 0;
        maxUnit = 2;
        objList = default;
    }
    public void SetupList()
    {
        GameObject parrent = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI);

        objList = new List<GameObject>();
        objList.Add(parrent.transform.Find("MaxUnit").gameObject);
        objList.Add(parrent.transform.Find("ExpBall").Find("MaxUnit").gameObject);
    }

    #region finished works
    public void UpdateFieldCnt(int count)
    {
        currentField = count;
        UpdateTexts();
    }
    public void AddFieldCnt(int add)
    {
        currentField += add;
        UpdateTexts();
    }
    public void IncreaceMax(bool isSkipText = false)
    {
        maxUnit++;
        if (isSkipText) { }
        else
        {
            UpdateTexts();
        }
    }
    void UpdateTexts()
    {
        Text uiText;
        foreach (var ele in objList)
        {
            uiText = ele.transform.Find("Current").GetComponent<Text>();
            uiText.text = currentField.ToString();

            uiText = ele.transform.Find("Max").GetComponent<Text>();
            uiText.text = "/" + maxUnit.ToString();
        }
    }
    #endregion
}
