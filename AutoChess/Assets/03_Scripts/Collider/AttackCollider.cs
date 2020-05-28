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

    GameObject unitObj;
    ChampInstance unit;
    Animator myAni;
    MySide mySide;
    bool isEnemyInRange;
    float moveSpeed;
    float attackSpeed;
    // 켜지는 순간 target을 정하고 걸어간다.
    private void Awake()
    {
        playerInven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        computerInven = MyFunc.GetObject(MyFunc.ObjType.ENEMY).
            transform.Find("Inventory").GetComponent<Inventory>();
        target = default;
        enemyList = default;
        unitObj = transform.parent.gameObject;
        unit = default;
        myAni = transform.parent.Find("character").GetComponent<Animator>();
        isEnemyInRange = false;
        moveSpeed = 1.0f;
        attackSpeed = 1.0f;
    }

    public void FightNow()
    {
        SetupMySide();                  // 내가 누구 편인지 정한다.
        SetupTarget();                  // 리스트에서 적을 하나 특정한다.
        StartCoroutine(AttackEnemy());
    }
    public void FightEnd()
    {
        isEnemyInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            foreach (var ele in enemyList)
            {
                if (other.transform.parent.parent.gameObject == ele.champion)
                {
                    isEnemyInRange = true;
                    switch (mySide)
                    {
                        case MySide.COMPUTER:
                            target = playerInven.FindChampFromInstance(
                                other.transform.parent.parent.gameObject);
                            break;
                        case MySide.PLAYER:
                            target = computerInven.FindChampFromInstance(
                                other.transform.parent.parent.gameObject);
                            break;
                    }
                    break;
                }
            }
        }
    }

    #region finished works

    #region Auto attack
    IEnumerator AttackEnemy()
    {
        while (target != default)
        {
            yield return new WaitForSeconds(attackSpeed);
            if (target != default)
            {
                // 적을 향해 걸어가서 -> 때린다.
                StartCoroutine(WalkToTarget());
            }
        }
    }
    IEnumerator AttackTarget()
    {
        while ((target != default && target.hp > 0) &&
            (unit != default && unit.hp > 0))
        {
            unitObj.transform.LookAt(target.champion.transform);
            myAni.SetTrigger("Attack");
            if (unit.champType == ChampionPool.ChampType.WIZZARD)
            {
                unit.wizzardEffect.Play();
                StartCoroutine(StopWizzardEffect());
            }
            yield return new WaitForSeconds(1.0f);
            try
            {
                target.Hit(unit.damage);

            }
            catch (System.NullReferenceException e)
            {
                break;
            }
        }
        // 피가 다 닳면 죽는다.
        if (target != default && target.hp <= 0)
        {
            SetupTarget();
        }// if: 적이 죽음
        else
        {
            target = default;
        }// if: 내가 죽음
    }
    IEnumerator StopWizzardEffect()
    {
        yield return new WaitForSeconds(1.0f);
        unit.wizzardEffect.Stop();
    }
    IEnumerator WalkToTarget()
    {
        Vector3 moveDirection = default;
        while (isEnemyInRange == false)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            unitObj.transform.LookAt(target.champion.transform);
            moveDirection = Vector3.forward * moveSpeed * Time.deltaTime;
            unitObj.transform.Translate(moveDirection);
            myAni.SetFloat("MoveSpeed", Mathf.Abs(moveSpeed * Time.deltaTime));
        }
        myAni.SetFloat("MoveSpeed", 0);
        StartCoroutine(AttackTarget());
    }
    #endregion

    void SetupTarget()
    {
        if (enemyList != default && enemyList.Count > 0)
        {
            foreach (var ele in enemyList)
            {
                if (ele.hp > 0)
                {
                    target = ele;
                    break;
                }
            }
        }
        else
        {
            target = default;
        }

        if (target.hp <= 0)
        {
            target = default;
        }
    }
    void SetupMySide()
    {
        foreach (var ele in playerInven.field)
        {
            if (transform.parent.gameObject == ele.champion)
            {
                mySide = MySide.PLAYER;
                enemyList = computerInven.field;
                unit = playerInven.FindChampFromInstance(unitObj);
                unit.VisibleHpBar(true);
                unit.hpBar.color = Color.green;
                unit.hpBar.fillAmount = 1.0f;
                break;
            }
        }
        foreach (var ele in computerInven.field)
        {
            if (transform.parent.gameObject == ele.champion)
            {
                mySide = MySide.COMPUTER;
                enemyList = playerInven.field;
                unit = computerInven.FindChampFromInstance(unitObj);
                unit.VisibleHpBar(true);
                unit.hpBar.color = Color.red;
                unit.hpBar.fillAmount = 1.0f;
                break;
            }
        }
    }
    #endregion
}
