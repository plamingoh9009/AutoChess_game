using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<ChampionPool.ChampInstance> inven;
    TileHandler _tileHandler;
    List<TileHandler.TileInfo> _squareTiles;
    List<TileHandler.TileInfo> _HexaTiles;
    int maxSize;
    private void Awake()
    {
        inven = new List<ChampionPool.ChampInstance>();
        maxSize = 9;
    }
    private void Start()
    {
        _tileHandler = GameObject.Find("FixedObject/Tiles").GetComponent<TileHandler>();
        _squareTiles = _tileHandler.squareInstances;
        _HexaTiles = _tileHandler.hexaInstances;
    }

    public bool IntoInventory(string champName)
    {
        ChampionPool.ChampInstance champ;
        if (inven.Count >= maxSize)
        {
            return false;
        }
        // 풀에서 요청 -> 인벤에 삽입
        champ = ChampionPool.instance.GetChamp(champName);
        champ.champion.transform.parent = transform;
        //CollocateChamp(champ);
        inven.Add(champ);
        SortingChamp();
        return true;
    }
    void CollocateChamp(ChampionPool.ChampInstance champ)
    {
        // 챔피언 구매시 빈 인벤에 나란히 배치

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
    public bool IsRemainInven()
    {
        if(inven.Count < maxSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
