using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    GameObject playImg;
    TimerHandler timerHandler;
    private void Awake()
    {
        playImg = transform.Find("Image").gameObject;
        timerHandler = MyFunc.GetObject(MyFunc.ObjType.TIMER).GetComponent<TimerHandler>();
        VisibleButton();
    }

    public void OnClick()
    {
        if(GameManager.instance.myTurn == GameManager.TurnType.READY)
        {
            timerHandler.ChangeGameTurnType();
            VisibleButton(false);
        }
    }
    public void VisibleButton(bool isVisible = true)
    {
        playImg.SetActive(isVisible);
    }
}
