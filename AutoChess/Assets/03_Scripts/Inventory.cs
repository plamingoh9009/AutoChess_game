using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<ChampionPool.ChampInstance> _inven;
    TileHandler _tileHandler;
    List<GameObject> _squareTiles;
    List<GameObject> _HexaTiles;
    int maxSize;
    private void Awake()
    {
        _inven = new List<ChampionPool.ChampInstance>();
        maxSize = 9;
    }
    private void Start()
    {
        _tileHandler = GameObject.Find("FixedObject/Tiles").GetComponent<TileHandler>();
        _squareTiles = _tileHandler._squareInstances;
        _HexaTiles = _tileHandler._hexaInstances;
    }

    public bool IntoInventory(string champName)
    {
        ChampionPool.ChampInstance champ;
        if (_inven.Count >= maxSize)
        {
            return false;
        }
        // 풀에서 요청 -> 인벤에 삽입
        champ = ChampionPool.instance.GetChamp(champName);
        champ.champion.transform.parent = transform;
        _inven.Add(champ);
        SortingChamp();
        return true;
    }
    void SortingChamp()
    {
        int count = _inven.Count;
        GameObject champ;
        for(int i=0; i<count; i++)
        {
            champ = _inven[i].champion;
            champ.transform.position = _squareTiles[i].transform.position;
            champ.SetActive(true);
        }
    }
}
