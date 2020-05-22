using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChampInstance = ChampionPool.ChampInstance;
using ChampInstances = System.Collections.Generic.List<ChampionPool.ChampInstance>;
public class EnemyHandler : MonoBehaviour
{
    TileHandler tileHandler;

    GoldUi goldUi;
    UnitCount unitCount;
    GoldStatue goldStatue;
    Inventory inven;
    RollChampions container;
    private void Awake()
    {
        tileHandler = transform.Find("Tiles").GetComponent<TileHandler>();

        goldUi = transform.Find("EnemyInfo").GetComponent<GoldUi>();
        unitCount = transform.Find("EnemyInfo").GetComponent<UnitCount>();
        goldStatue = transform.Find("GoldDragons").GetComponent<GoldStatue>();
        inven = transform.Find("Inventory").GetComponent<Inventory>();
        container = transform.Find("ChampContainer").GetComponent<RollChampions>();
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

    public void GoToReadyTurn()
    {
        // 레디 턴으로 갈 때 이자 받는다.
        goldUi.AddInterest();
    }
    public void GoToFightTurn()
    {
        // 싸우는 턴으로 가기 전에 리롤을 받아서 합치고 낸다.
        container.Reroll();
        MoveRollToInven(container.currentRoll);
        inven.QualityUpChampion();

        StartCoroutine(inven.AutoThrowChampToField(true));
        StartCoroutine(inven.AutoReturnChamp(true));
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
}
