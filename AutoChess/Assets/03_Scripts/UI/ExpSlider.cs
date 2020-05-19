using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    float currentExp;
    float maxExp;
    int add;
    Image slider;

    GoldUi goldUi;
    UnitCount unitCount;
    private void Awake()
    {
        currentExp = 0;
        maxExp = 1;
        add = 4;
        slider = GetComponent<Image>();
        goldUi = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();
    }

    public void OnClickButton()
    {
        if (goldUi.gold >= add)
        {
            ExpAdd();
        }
    }
    void ExpAdd()
    {
        currentExp += add;
        goldUi.AddGold(add * -1);

        StartCoroutine(SyncSliderToExp());
    }
    IEnumerator SyncSliderToExp()
    {
        float speed = 0.03f;
        float amount = 0.08f;

        while (slider.fillAmount < (currentExp / maxExp))
        {
            yield return new WaitForSeconds(speed);
            slider.fillAmount += amount;

            if (slider.fillAmount >= 1)
            {
                CheckExp();
            }
        }
    }
    bool CheckExp()
    {
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
            return true;
        }
        return false;
    }
    void LevelUp()
    {
        maxExp *= 2;
        unitCount.IncreaceMax();

        slider.fillAmount = 0f;
    }
}
