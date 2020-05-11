using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollChampions : MonoBehaviour
{
    ChampionPool _champion;
    List<int> _rollChampions;
    List<GameObject> _currentRoll;
    List<GameObject> _nextRoll;
    bool _setNextRoll;

    private void Awake()
    {
        _champion = ChampionPool.instance;
        _rollChampions = new List<int>();
        _currentRoll = new List<GameObject>();
        _nextRoll = new List<GameObject>();
    }

    private void Start()
    {
        Roll();
    }

    void Roll()
    {
        int rollIdx;

        // 챔피언 이름 리스트에서 랜덤하게 5개를 뽑는다.
        _rollChampions.Clear();
        for (int i = 0; i < 5; i++)
        {
            rollIdx = Random.Range(0, _champion.championNames.Count);
            _rollChampions.Add(rollIdx);
            //Debug.Log(_champion.championNames[rollIdx]);
        }
    }
}
