using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollChampions : MonoBehaviour
{
    #region Variable
    ChampionPool _pool;
    ShopCollider shopCollider;
    List<int> _rollChampions;
    List<ChampionPool.ChampInstance> currentRoll;
    List<ChampionPool.ChampInstance> _nextRoll;
    public bool isLocked { get; set; }
    #endregion
    #region Start()
    private void Awake()
    {
        _rollChampions = new List<int>();
        currentRoll = new List<ChampionPool.ChampInstance>();
        _nextRoll = new List<ChampionPool.ChampInstance>();
        isLocked = false;
    }

    private void Start()
    {
        _pool = ChampionPool.instance;
        shopCollider = MyFunc.GetObject(MyFunc.ObjType.SHOP_COLLIDER).GetComponent<ShopCollider>();
        Roll();
        SetupShopCollider();
    }
    void SetupShopCollider()
    {
        shopCollider.PushCurrentRoll(currentRoll);
        shopCollider.InitColliderPos();
    }
    #endregion

    #region Roll / Reroll
    void Roll()
    {
        RollChampToList(currentRoll);
        RollChampToList(_nextRoll);
        SetupChampActive(currentRoll, true);
    }
    public void Reroll()
    {
        if (isLocked) { }
        else
        {
            MyFunc.Swap(ref currentRoll, ref _nextRoll);
            RollChampToList(_nextRoll);
            SetupChampActive(currentRoll, true);
            // 리롤할 때 shopCollider를 다시 active로 전환한다.
            shopCollider.ActiveAllColliders();
        }
    }
    #endregion

    #region Roll champion / Setup position
    void RollChampToList(List<ChampionPool.ChampInstance> champList)
    {
        int rollIdx;
        string champName;
        ChampionPool.ChampInstance champ;
        
        if(champList.Count > 0)
        {
            foreach(var ele in champList)
            {
                _pool.GetBackChamp(ele);
            }
            champList.Clear();
        }// if: champList에 오브젝트가 있다면 전부 반환한다.

        for (int i = 0; i < 5; i++)
        {
            rollIdx = Random.Range(0, _pool.championNames.Count);
            champName = _pool.championNames[rollIdx];
            champ = _pool.GetChamp(champName);
            champList.Add(champ);
        }// loop: 리스트에 풀에서 가져온 챔피언을 넣는다.

        // 챔피언 위치를 잡아준다.
        SetupChampPositions(champList);
    }
    void SetupChampPositions(List<ChampionPool.ChampInstance> champList)
    {
        float distance = 4.5f;
        int currentPos = -2;
        float result;
        foreach(var champ in champList)
        {
            // 챔피언 위치 부모에게로 옮기기
            champ.champion.transform.parent = transform;
            champ.champion.transform.position = transform.position;
            // 챔피언 거리 띄우기
            result = distance * currentPos;
            champ.champion.transform.Translate(transform.right * result);
            champ.champion.transform.rotation = Quaternion.Euler(new Vector3(-45, 180, 0));
            champ.champion.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            currentPos++;
        }
    }
    void SetupChampActive(List<ChampionPool.ChampInstance> champList, bool isActive = false)
    {
        foreach(var ele in champList)
        {
            ele.champion.SetActive(isActive);
        }
    }
    #endregion

    public string ReleaseChamp(int idx)
    {
        string champName;
        ChampionPool.ChampInstance champ;
        champ = currentRoll[idx];
        champName = champ.name;
        // 리스트에서 빼지 않고, 보이지 않도록 false 처리한다.
        // Reroll 할 때 풀로 반환하기 때문
        champ.champion.SetActive(false);
        return champName;
    }
}
