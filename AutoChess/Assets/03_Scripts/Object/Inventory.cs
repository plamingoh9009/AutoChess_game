using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public List<ChampionPool.ChampInstance> inven;
    public List<ChampionPool.ChampInstance> field;
    GameObject invenObj;
    GameObject fieldObj;

    TileHandler tileHandler;
    List<TileHandler.TileInfo> _squareTiles;
    List<TileHandler.TileInfo> _HexaTiles;
    int maxSize;

    GoldUi gold;
    UnitCount unitCount;
    private void Awake()
    {
        inven = new List<ChampionPool.ChampInstance>();
        field = new List<ChampionPool.ChampInstance>();
        invenObj = transform.Find("Inventory").gameObject;
        fieldObj = transform.Find("Field").gameObject;
        maxSize = 9;
    }
    private void Start()
    {
        tileHandler = GameObject.Find("FixedObject/Tiles").GetComponent<TileHandler>();
        _squareTiles = tileHandler.squareInstances;
        _HexaTiles = tileHandler.hexaInstances;
        gold = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();
    }

    public bool IntoInventory(string champName)
    {
        ChampionPool.ChampInstance champ;
        if (IsRemainInven() == false)
        {
            return false;
        }
        // 풀에서 요청 -> 인벤에 삽입
        champ = ChampionPool.instance.GetChamp(champName);
        champ.champion.transform.parent = invenObj.transform;
        CollocateChamp(champ);
        inven.Add(champ);
        // Unit count update
        int unitCnt = inven.Count + field.Count;
        unitCount.UpdateUnit(unitCnt);
        return true;
    }
    #region Swap, Move
    public void SwapChamp(TileHandler.TileInfo landingTile,
        TileHandler.TileInfo standingTile)
    {
        ChampionPool.ChampInstance sour = FindChampFromTile(standingTile);
        ChampionPool.ChampInstance dest = FindChampFromTile(landingTile);
        ChangeChampTransform(sour, landingTile);
        ChangeChampTransform(dest, standingTile);
    }
    public void MoveChamp(GameObject champion,
        TileHandler.TileInfo landingTile)
    {
        ChampionPool.ChampInstance sour = FindChampFromInstance(champion);
        ChangeChampTransform(sour, landingTile);
    }
    void ChangeChampTransform(ChampionPool.ChampInstance champ,
        TileHandler.TileInfo landingTile)
    {
        switch (landingTile.type)
        {
            case TileHandler.TileType.SQUARE:
                champ.champion.transform.parent = invenObj.transform;
                break;
            case TileHandler.TileType.HEXAGON:
                champ.champion.transform.parent = fieldObj.transform;
                break;
        }// switch: 부모 오브젝트를 바꾼다.

        // 위치를 바꾼다.
        champ.champion.transform.position = landingTile.tile.transform.position;
        champ.standingTile = landingTile;

        // 리스트 요소를 바꾼다.
        ChangeInvenList(champ);
        ChangeFieldList(champ);
    }
    #endregion

    #region finished works
    /// <summary>
    /// 챔피언 구매시 빈 인벤토리 슬롯에 챔프를 배치하는 함수
    /// </summary>
    /// <param name="champ">상점에서 구입한 챔프</param>
    void CollocateChamp(ChampionPool.ChampInstance champ)
    {
        UnitHandler unitHandler = champ.champion.GetComponentInChildren<UnitHandler>();
        // 빈 인벤토리 슬롯 찾아서 챔프를 옮긴다.
        foreach (var ele in _squareTiles)
        {
            if (ele.isEmpty)
            {
                champ.champion.transform.position = ele.tile.transform.position;
                champ.standingTile = ele;
                champ.champion.SetActive(true);
                ele.isEmpty = false;
                break;
            }
        }
    }
    void SortingChamp()
    {
        int count = inven.Count;
        GameObject champ;
        for (int i = 0; i < count; i++)
        {
            champ = inven[i].champion;
            champ.transform.position = _squareTiles[i].tile.transform.position;
            champ.SetActive(true);
        }
    }
    bool ChangeInvenList(ChampionPool.ChampInstance champ)
    {
        foreach (var ele in inven)
        {
            if (ele == champ)
            {
                inven.Remove(ele);
                return true;
            }
        }// loop: 존재하면 지운다.
        // 없으면 추가한다.
        inven.Add(champ);
        return true;
    }
    bool ChangeFieldList(ChampionPool.ChampInstance champ)
    {
        foreach (var ele in field)
        {
            if (ele == champ)
            {
                field.Remove(ele);
                return true;
            }
        }
        field.Add(champ);
        return true;
    }
    public ChampionPool.ChampInstance FindChampFromTile(TileHandler.TileInfo standingTile)
    {
        foreach (var ele in inven)
        {
            if (ele.standingTile == standingTile)
            {
                return ele;
            }
        }
        foreach (var ele in field)
        {
            if (ele.standingTile == standingTile)
            {
                return ele;
            }
        }
        return default;
    }
    public ChampionPool.ChampInstance FindChampFromInstance(GameObject champion)
    {
        foreach (var ele in inven)
        {
            if (ele.champion == champion)
            {
                return ele;
            }
        }
        foreach (var ele in field)
        {
            if (ele.champion == champion)
            {
                return ele;
            }
        }
        return default;
    }
    public bool IsEmptyTile(TileHandler.TileInfo tileInfo)
    {
        foreach (var ele in inven)
        {
            if (ele.standingTile == tileInfo)
            {
                return false;
            }
        }
        foreach (var ele in field)
        {
            if (ele.standingTile == tileInfo)
            {
                return false;
            }
        }
        return true;
    }
    public void ShowInven()
    {
        Debug.Log("### Inven ###");
        ShowChampList(inven);
    }
    public void ShowField()
    {
        Debug.Log("### Field ###");
        ShowChampList(field);
    }
    /// <summary>
    /// 인벤토리에 자리가 남는지 확인하는 함수
    /// </summary>
    public bool IsRemainInven()
    {
        if (inven.Count < maxSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void ShowChampList(List<ChampionPool.ChampInstance> list)
    {
        foreach (var ele in list)
        {
            Debug.Log((ele.name, ele.champion));
        }
    }
    #endregion
}
