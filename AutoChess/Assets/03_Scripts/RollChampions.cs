using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollChampions : MonoBehaviour
{
    #region Variable
    ChampionPool _pool;
    List<int> _rollChampions;
    List<ChampionPool.ChampInstance> _currentRoll;
    List<ChampionPool.ChampInstance> _nextRoll;
    public bool isLocked { get; set; }
    #endregion
    #region Start()
    private void Awake()
    {
        _rollChampions = new List<int>();
        _currentRoll = new List<ChampionPool.ChampInstance>();
        _nextRoll = new List<ChampionPool.ChampInstance>();
        isLocked = false;
    }

    private void Start()
    {
        _pool = ChampionPool.instance;
        Roll();
    }
    #endregion

    #region Roll / Reroll
    void Roll()
    {
        RollChampToList(_currentRoll);
        RollChampToList(_nextRoll);
        SetupChampActive(_currentRoll, true);
    }
    public void Reroll()
    {
        if (isLocked) { }
        else
        {
            MyFunc.Swap(ref _currentRoll, ref _nextRoll);
            RollChampToList(_nextRoll);
            SetupChampActive(_currentRoll, true);
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
}
