using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    #region Start()
    int _timer;
    UnityEngine.UI.Text _uiText;
    
    private void Awake()
    {
        _timer = 30;
        _uiText = GameObject.Find("Count").GetComponent<UnityEngine.UI.Text>();
    }
    private void Start()
    {
        StartCoroutine(PastOneSecond());
    }
    #endregion

    IEnumerator PastOneSecond()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _timer--;
            if (_timer <= 0)
            {
                ChangeGameTurnType();
            }// if: 이전 턴이 끝났다면 다음 턴을 시작한다
            // 마지막으로 숫자를 렌더한다.
            _uiText.text = _timer.ToString();
        }
    }

    void ChangeGameTurnType()
    {
        switch(GameManager.instance.myTurn)
        {
            case GameManager.TurnType.READY:
                // 게임매니저의 턴타입을 바꾼다
                GameManager.instance.myTurn = GameManager.TurnType.FIGHT;
                // 시간과 색을 바꾼다
                _timer = 45;
                _uiText.color = new Color(255/255f, 134/255f, 78/255f);
                break;
            case GameManager.TurnType.FIGHT:
                // 게임매니저의 턴타입을 바꾼다
                GameManager.instance.myTurn = GameManager.TurnType.READY;
                // 시간과 색을 바꾼다
                _timer = 30;
                _uiText.color = new Color(142/255f, 236/255f, 57/255f);
                break;
            default:
                break;
        }
    }
}
