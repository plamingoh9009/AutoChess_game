using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChampInstance = ChampionPool.ChampInstance;
using ChampInstances = System.Collections.Generic.List<ChampionPool.ChampInstance>;
public class PlayerHandler : MonoBehaviour
{
    float playerHp;
    float maxHp;
    Image hpImg;
    UnitCount unitCount;

    Inventory inven;
    Inventory enemyInven;
    ChampInstances enemys;
    ChampInstances players;
    private void Awake()
    {
        maxHp = 100f;
        playerHp = maxHp;
        hpImg = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).
            transform.Find("HpBar/ForeImg").GetComponent<Image>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();

        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        enemyInven = MyFunc.GetObject(MyFunc.ObjType.ENEMY).
            transform.Find("Inventory").GetComponent<Inventory>();
        enemys = default;
        players = default;
    }

    public void GotoReadyTurn()
    {
        ComputeHp();
        EndFight();
    }
    public void GotoFightTurn()
    {
        Fight();
    }

    void EndFight()
    {
        StartCoroutine(ActiveAttackColliders(false));
        BackToField();  // 제자리로 되돌린다.
    }

    void Fight()
    {
        StartCoroutine(ActiveAttackColliders(true));
    }
    #region finished works
    void BackToField()
    {
        foreach (var ele in players)
        {
            ele.champion.transform.position = ele.standingTile.tile.transform.position;
            ele.champion.transform.rotation = ele.standingTile.tile.transform.rotation;
            ele.Revival();
        }
    }
    IEnumerator ActiveAttackColliders(bool isActive)
    {
        int i;
        for (i = 0; i < unitCount.maxUnit; i++)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        players = inven.field;
        enemys = enemyInven.field;
        foreach (var ele in players)
        {
            ele.ActiveAttackCollider(isActive);
            ele.isFightOk = isActive;
        }
    }
    void ComputeHp()
    {
        // Hp를 계산해서 깎는 함수다.
        int playersCount = 0;
        int enemysCount = 0;
        players = inven.field;
        enemys = enemyInven.field;

        if (players != default && enemys != default)
        {
            foreach(var ele in players)
            {
                if(ele.hp > 0)
                {
                    playersCount++;
                }
            }
            foreach(var ele in enemys)
            {
                if(ele.hp > 0)
                {
                    enemysCount++;
                }
            }
            if (playersCount + enemysCount > 0)
            {
                if (playersCount <= 0)
                {
                    LoseHp(enemysCount);
                }
            }// if: 판에 체스말이 있지만, 내 말이 없는 경우 진다.
            else
            {
                LoseHp(enemysCount, true);
            }// if: 판에 체스말이 없는 경우 비긴다.
        }
    }
    void LoseHp(int enemyCount, bool isDraw = false)
    {
        if (isDraw) { }
        else
        {
            playerHp -= (5.0f * enemyCount);
            hpImg.fillAmount = (playerHp / maxHp);
        }
    }
    void InitChampLists()
    {
        players = default;
        enemys = default;
    }
    #endregion
}
