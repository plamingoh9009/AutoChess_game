using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChampType = ChampionPool.ChampType;
using AttackType = ChampionPool.AttackType;
using ChampInstance = ChampionPool.ChampInstance;
public class ChampInfo : MonoBehaviour
{
    static public void SetupChampInfo(ChampInstance champ)
    {
        SetupChampType(champ);
        SetupAttackType(champ);
    }
    static void SetupChampType(ChampInstance champ)
    {
        string key = champ.name.Split(' ')[1];
        switch(key)
        {
            case "Archer":
                champ.champType = ChampType.ARCHER;
                champ.maxHp = 100;
                champ.hp = champ.maxHp;
                champ.range = 10;
                champ.damage = 5;
                break;
            case "Wizzard":
                champ.champType = ChampType.WIZZARD;
                champ.maxHp = 100;
                champ.hp = champ.maxHp;
                champ.range = 5;
                champ.damage = 10;
                break;
            case "Knight":
                champ.champType = ChampType.KNIGHT;
                champ.maxHp = 200;
                champ.hp = champ.maxHp;
                champ.range = 2;
                champ.damage = 5;
                break;
        }
    }
    static void SetupAttackType(ChampInstance champ)
    {
        string key = champ.name.Split(' ')[0];
        switch(key)
        {
            case "Fire":
                champ.attackType = AttackType.FIRE;
                break;
            case "Frost":
                champ.attackType = AttackType.FROST;
                break;
            case "Nature":
                champ.attackType = AttackType.NATURE;
                break;
        }
    }
}
