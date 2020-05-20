using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ChampionPool.ChampInstance> inven;
    public List<ChampionPool.ChampInstance> field;
    GameObject invenObj;
    GameObject fieldObj;

    TileHandler tileHandler;
    List<TileHandler.TileInfo> _squareTiles;
    List<TileHandler.TileInfo> _hexaTiles;
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
        _hexaTiles = tileHandler.hexaInstances;
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
        return true;
    }
    public void SellChampToList(string champName, List<ChampionPool.ChampInstance> list)
    {
        ChampionPool.ChampInstance returnChamp = default;
        foreach(var ele in list)
        {
            if(ele.name.CompareTo(champName) == 0)
            {
                returnChamp = ele;
                list.Remove(ele);
                break;
            }
        }
        gold.AddGold(2);
        unitCount.UpdateFieldCnt(field.Count);
        ChampionPool.instance.GetBackChamp(returnChamp);
    }
    #region Auto function()
    public IEnumerator AutoThrowChampToField()
    {
        int loopCnt = 0;
        ChampionPool.ChampInstance throwChamp = new ChampionPool.ChampInstance();
        TileHandler.TileInfo emptyField = new TileHandler.TileInfo();
        if (unitCount.currentField < unitCount.maxUnit)
        {
            loopCnt = unitCount.maxUnit - unitCount.currentField;
            for (int i = 0; i < loopCnt; i++)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                foreach (var ele in inven)
                {
                    throwChamp = ele;
                    break;
                }
                foreach (var ele in _hexaTiles)
                {
                    if (ele.isEmpty)
                    {
                        emptyField = ele;
                        break;
                    }
                }
                // 인벤을 다 찾아도 챔피언이 없다면 break
                if (throwChamp != default)
                {
                    MoveChamp(throwChamp.champion, emptyField);
                }
                else { break; }
            }// loop: 모자란 만큼 챔피언을 필드에 던진다.
        }// if: 현재 필드가 모자라다면
    }
    public IEnumerator AutoReturnChamp()
    {
        int loopCnt = 0;
        ChampionPool.ChampInstance returnChamp = new ChampionPool.ChampInstance();
        TileHandler.TileInfo emptyInven = new TileHandler.TileInfo();
        if (unitCount.currentField > unitCount.maxUnit)
        {
            loopCnt = unitCount.currentField - unitCount.maxUnit;
            for (int i = 0; i < loopCnt; i++)
            {
                emptyInven = default;
                yield return new WaitForSeconds(Time.deltaTime);
                foreach (var ele in field)
                {
                    returnChamp = ele;
                    break;
                }
                foreach (var ele in _squareTiles)
                {
                    if (ele.isEmpty)
                    {
                        emptyInven = ele;
                        break;
                    }
                }
                // 인벤을 다 찾아도 빈곳이 없다면 챔피언을 강제로 판다
                if (emptyInven != default)
                {
                    MoveChamp(returnChamp.champion, emptyInven);
                }
                else
                {
                    SellChampToList(returnChamp.name, field);
                }
            }// loop: 넘치는 만큼 챔피언을 인벤에 되돌린다.
        }// if: 현재 필드가 넘친다면
    }
    #endregion
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

        // 리스트 요소를 바꾼다.
        if (champ.standingTile.type == landingTile.type) { }
        else
        {
            ChangeInvenList(champ);
            ChangeFieldList(champ);
        }// if: [인벤 -> 필드, 필드 -> 인벤] 같은 경우에는 리스트를 옮긴다.
        // 필드 카운트를 업데이트 한다.
        unitCount.UpdateFieldCnt(field.Count);

        // 위치를 바꾼다.
        champ.champion.transform.position = landingTile.tile.transform.position;
        champ.standingTile.isEmpty = true;
        champ.standingTile = landingTile;
        champ.standingTile.isEmpty = false;
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
