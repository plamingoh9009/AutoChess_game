using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    #region Start()
    int _timer;
    int turnSkip;
    UnityEngine.UI.Text _uiText;
    RollChampions _containerHandler;

    PlayButton playBtn;
    UpgradeBtn upgradeBtn;

    Inventory playerInven;
    Inventory enemyInven;
    GoldUi playerGoldInfo;
    EnemyHandler enemyHandler;
    PlayerHandler playerHandler;

    private void Awake()
    {
        _timer = 30;
        turnSkip = -1;
        _uiText = GameObject.Find("Count").GetComponent<UnityEngine.UI.Text>();
        playBtn = MyFunc.GetObject(MyFunc.ObjType.PLAY_BUTTON).GetComponent<PlayButton>();
        upgradeBtn = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).
            transform.Find("UpgradeBtn").GetComponent<UpgradeBtn>();

        playerInven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        playerGoldInfo = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();

        enemyInven = MyFunc.GetObject(MyFunc.ObjType.ENEMY).transform.Find("Inventory").
            GetComponent<Inventory>();
        enemyHandler = MyFunc.GetObject(MyFunc.ObjType.ENEMY).GetComponent<EnemyHandler>();
        playerHandler = MyFunc.GetObject(MyFunc.ObjType.PLAYER).GetComponent<PlayerHandler>();
    }
    private void Start()
    {
        _containerHandler = GameObject.Find("Shop/ShopObj/ChampContainer").GetComponent<RollChampions>();
        StartCoroutine(PastOneSecond());
    }
    #endregion

    IEnumerator PastOneSecond()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _timer--;
            if(turnSkip == -1)
            {
                if (_timer <= 0)
                {
                    ChangeGameTurnType();
                    turnSkip = -1;
                }// if: 이전 턴이 끝났다면 다음 턴을 시작한다

                if (GameManager.instance.myTurn == GameManager.TurnType.FIGHT)
                {
                    if (playerInven.HowManyAliveField() == 0 ||
                        enemyInven.HowManyAliveField() == 0)
                    {
                        turnSkip = _timer - 5;
                    }
                }
            }
            else
            {
                if (_timer <= turnSkip || _timer <= 0)
                {
                    ChangeGameTurnType();
                    turnSkip = -1;
                }// if: 이전 턴이 끝났다면 다음 턴을 시작한다
            }
            // 마지막으로 숫자를 렌더한다.
            _uiText.text = _timer.ToString();

            upgradeBtn.VisibleButton(playerInven.IsQualityUpPossible());
        }
    }

    public void ChangeGameTurnType()
    {
        switch(GameManager.instance.myTurn)
        {
            case GameManager.TurnType.READY:
                // 게임매니저의 턴타입을 바꾼다
                GameManager.instance.myTurn = GameManager.TurnType.FIGHT;
                // 시간과 색을 바꾼다
                _timer = 45;
                _uiText.color = new Color(255/255f, 134/255f, 78/255f);
                StartCoroutine(playerInven.AutoThrowChampToField());
                StartCoroutine(playerInven.AutoReturnChamp());

                // 스킵 버튼을 안보이게 한다.
                playBtn.VisibleButton(false);
                enemyHandler.GotoFightTurn();
                playerHandler.GotoFightTurn();
                break;
            case GameManager.TurnType.FIGHT:
                // 게임매니저의 턴타입을 바꾼다
                GameManager.instance.myTurn = GameManager.TurnType.READY;
                // 시간과 색을 바꾼다
                _timer = 30;
                _uiText.color = new Color(142/255f, 236/255f, 57/255f);
                _containerHandler.Reroll();

                // 스킵 버튼을 보이게 한다.
                playBtn.VisibleButton(true);
                // 싸움 턴에서 레디 턴으로 갈 때 이자를 받는다.
                playerGoldInfo.AddInterest();
                enemyHandler.GotoReadyTurn();
                playerHandler.GotoReadyTurn();
                break;
            default:
                break;
        }
    }
}
