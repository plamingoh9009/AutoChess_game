using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ChampInstance = ChampionPool.ChampInstance;
using ChampInstances = System.Collections.Generic.List<ChampionPool.ChampInstance>;
public class EnemyHandler : MonoBehaviour
{
    TileHandler tileHandler;

    GoldUi goldUi;
    UnitCount unitCount;
    GoldStatue goldStatue;
    RollChampions container;

    Image hpImg;
    float enemyHp;
    float maxHp;

    Inventory inven;
    Inventory playerInven;
    ChampInstances enemys;
    ChampInstances players;
    private void Awake()
    {
        tileHandler = transform.Find("Tiles").GetComponent<TileHandler>();

        goldUi = transform.Find("EnemyInfo").GetComponent<GoldUi>();
        unitCount = transform.Find("EnemyInfo").GetComponent<UnitCount>();
        goldStatue = transform.Find("GoldDragons").GetComponent<GoldStatue>();
        container = transform.Find("ChampContainer").GetComponent<RollChampions>();

        hpImg = transform.Find("EnemyUi/HpBar/ForeImg").GetComponent<Image>();
        maxHp = 100f;
        enemyHp = maxHp;

        inven = transform.Find("Inventory").GetComponent<Inventory>();
        playerInven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        enemys = default;
        players = default;
    }
    private void Start()
    {
        tileHandler.SetupTiles(true);

        goldUi.SetupObjs(goldStatue.gameObject);
        goldStatue.SetupGoldStatues(goldUi);
        inven.SetupComponents(goldUi, unitCount, tileHandler);

        // 적이 랜덤 롤을 받아서 인벤에 넣는다.
        MoveRollToInven();
    }

    public void GotoReadyTurn()
    {
        goldUi.AddInterest();   // 레디 턴으로 갈 때 이자 받는다.
        ComputeHp();            // Hp 업데이트 한다.

        EndFight();
    }
    public void GotoFightTurn()
    {
        // 싸우는 턴으로 가기 전에 리롤을 받아서 합치고 낸다.
        container.Reroll();
        MoveRollToInven(container.currentRoll);
        inven.QualityUpChampion();

        StartCoroutine(inven.AutoThrowChampToField(true));
        StartCoroutine(inven.AutoReturnChamp(true));

        // 낸 다음에 돈이 남으면 레벨 올린다.
        if(goldUi.gold >= 4)
        {
            LevelUp();
        }
        Fight();
    }

    void EndFight()
    {
        StartCoroutine(ActiveAttackColliders(false));
    }

    void Fight()
    {
        StartCoroutine(ActiveAttackColliders(true));
    }

    #region finished works
    IEnumerator ActiveAttackColliders(bool isActive)
    {
        int i;
        for (i = 0; i < unitCount.maxUnit; i++)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (i >= unitCount.maxUnit)
        {
            enemys = inven.field;
            players = playerInven.field;
            foreach (var ele in enemys)
            {
                ele.ActiveAttackCollider(isActive);
                ele.isFightOk = isActive;
            }
        }
    }
    void ComputeHp()
    {
        // Hp를 계산해서 깎는 함수다.
        enemys = inven.field;
        players = playerInven.field;

        if (players != default && enemys != default)
        {
            if (players.Count + enemys.Count > 0)
            {
                if (enemys.Count <= 0)
                {
                    LoseHp(players.Count);
                }
            }// if: 판에 체스말이 있지만, 내 말이 없는 경우 진다.
            else
            {
                LoseHp(players.Count, true);
            }// if: 판에 체스말이 없는 경우 비긴다.
        }
    }
    void LoseHp(int playerCount, bool isDraw = false)
    {
        if (isDraw) { }
        else
        {
            enemyHp -= (5.0f * playerCount);
            hpImg.fillAmount = (enemyHp / maxHp);
        }
    }
    void LevelUp()
    {
        unitCount.IncreaceMax(true);
        goldUi.AddGold(-4);
    }
    void MoveRollToInven()
    {
        ChampInstances currentRoll = default;
        currentRoll = container.currentRoll;
        MoveRollToInven(currentRoll);
    }
    void MoveRollToInven(ChampInstances currentRoll)
    {
        foreach (var ele in currentRoll)
        {
            ele.champion.SetActive(true);
            inven.IntoInventory(ele.name);
        }
    }
    #endregion
}
