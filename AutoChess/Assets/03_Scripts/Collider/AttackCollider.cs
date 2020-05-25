using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChampInstance = ChampionPool.ChampInstance;
using ChampInstances = System.Collections.Generic.List<ChampionPool.ChampInstance>;
public class AttackCollider : MonoBehaviour
{
    enum MySide
    {
        PLAYER,
        COMPUTER
    }
    Inventory playerInven;
    Inventory computerInven;
    ChampInstance target;
    ChampInstances enemyList;

    GameObject unit;
    Animation myAnimation;
    MySide mySide;
    // 켜지는 순간 target을 정하고 걸어간다.
    private void Awake()
    {
        playerInven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        computerInven = MyFunc.GetObject(MyFunc.ObjType.ENEMY).
            transform.Find("Inventory").GetComponent<Inventory>();
        target = default;
        enemyList = default;
        myAnimation = transform.parent.Find("character").GetComponent<Animation>();
    }
    private void Start()
    {
        // 내가 누구 편인지 정한다. -> 적을 리스트에 담는다.
        SetupMySide();
        SetupTarget();
    }

    void SetupTarget()
    {
        if(enemyList != default && enemyList.Count > 0)
        {
            foreach(var ele in enemyList)
            {
                if(ele.hp > 0)
                {
                    target = ele;
                    break;
                }
            }
            //transform.LookAt(target.champion.transform.)
        }
    }
    void SetupMySide()
    {
        foreach(var ele in playerInven.field)
        {
            if(transform.parent.gameObject == ele.champion)
            {
                mySide = MySide.PLAYER;
                enemyList = computerInven.field;
                break;
            }
        }
        foreach(var ele in computerInven.field)
        {
            if(transform.parent.gameObject == ele.champion)
            {
                mySide = MySide.COMPUTER;
                enemyList = playerInven.field;
                break;
            }
        }
    }
}
