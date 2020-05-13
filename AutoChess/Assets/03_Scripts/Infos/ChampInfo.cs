using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChampInfo", menuName = "AutoChess/ChampInfo", order = 1)]
public class ChampInfo : ScriptableObject
{
    // 챔피언이 꼭 가져야할 정보
    public string champName;
    public int cost;
    public int health = 100;
    public int damage = 10;
    public int attackRange = 1;
}
