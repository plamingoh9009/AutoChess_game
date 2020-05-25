using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ChampionPool : MonoBehaviour
{
    #region Variables
    public static ChampionPool instance = null;
    public enum ChampType
    {
        ARCHER,
        WIZZARD,
        KNIGHT
    }
    public enum AttackType
    {
        FIRE,
        FROST,
        NATURE
    }
    public class ChampInstance
    {
        public string name;
        public GameObject champion;
        public TileHandler.TileInfo standingTile;
        public int quality;
        public bool isClickOk;

        public float hp;
        public float maxHp;
        public float range;
        public float damage;
        public ChampType champType;
        public AttackType attackType;

        private GameObject hpBar;
        private GameObject attackCollider;
        public bool isFightOk;
        public void SetupDefault()
        {
            // Attack collider Range
            attackCollider = champion.transform.Find("AttackCollider").gameObject;
            attackCollider.transform.localScale = new Vector3(range, 0.1f, range);
            ActiveAttackCollider(false);
            // Hp bar
            hpBar = champion.transform.Find("character/HpBar").gameObject;
            VisibleHpBar(false);
            // bool
            isFightOk = false;
        }
        public void VisibleHpBar(bool isVisible)
        {
            hpBar.SetActive(isVisible);
        }
        public void ActiveAttackCollider(bool isActive)
        {
            attackCollider.SetActive(isActive);
        }
    }
    public List<string> championNames { get; private set; }
    public Dictionary<string, GameObject> championPrefabs { get; private set; }

    List<ChampInstance> championPool;      // 오브젝트 풀
    string prefabPath;                     // 챔피언 프리팹 경로
    int addObjectCnt;                      // 풀에 오브젝트를 얼마나 추가할 지
    int maxPoolCnt;                        // 오브젝트풀의 최대 크기

    #endregion

    #region Start()
    private void Awake()
    {
        SetupSingleton();
        addObjectCnt = 3;
        maxPoolCnt = 50;

        SetupPrefabs();
        InitObjPool();
    }
    void SetupSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    #region Add Prefabs()
    void SetupPrefabs()
    {
        prefabPath = "Assets/04_Prefabs/Champions/";

        championNames = new List<string>();
        championNames.Add("Fire Archer");
        championNames.Add("Fire Knight");
        championNames.Add("Fire Wizzard");
        championNames.Add("Frost Archer");
        championNames.Add("Frost Knight");
        championNames.Add("Frost Wizzard");
        championNames.Add("Nature Archer");
        championNames.Add("Nature Knight");
        championNames.Add("Nature Wizzard");

        championPrefabs = new Dictionary<string, GameObject>();
        string fullPrefabPath;
        GameObject myPrefab;
        for (int i = 0; i < championNames.Count; i++)
        {
            fullPrefabPath = prefabPath + championNames[i] + ".prefab";
            myPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPrefabPath);

            championPrefabs.Add(championNames[i], myPrefab);

            // 치명적인 에러처리
            if (myPrefab == null)
            {
                Debug.LogError("Prefab path wrong");
                break;
            }
        }
        championPool = new List<ChampInstance>();
    }
    void InitObjPool()
    {
        for (int i = 0; i < championPrefabs.Count; i++)
        {
            // 프리팹 인스턴스화 + 오브젝트 풀에 넣기
            MakeChamp(championNames[i]);
        }
    }
    #endregion

    #region Instance Add/Delete()
    void MakeChamp(string name)
    {
        ChampInstance myInstance;
        GameObject myChamp;
        for (int i = 0; i < addObjectCnt; i++)
        {
            championPrefabs.TryGetValue(name, out myChamp);
            myInstance = new ChampInstance();
            myInstance.name = name;
            myInstance.quality = 1;
            myInstance.isClickOk = false;
            myInstance.champion = Instantiate(myChamp, Vector3.zero, Quaternion.identity, transform);
            myInstance.champion.SetActive(false);
            ChampInfo.SetupChampInfo(myInstance);
            myInstance.SetupDefault();
            championPool.Add(myInstance);
        }// loop: 정해진 크기만큼 미리 인스턴스를 생성한다

        // 오브젝트풀의 크기가 maxPoolCnt 보다 커지면 넘어간 만큼 지운다.
        if (championPool.Count > maxPoolCnt)
        {
            RemoveChamp(championPool.Count - maxPoolCnt);
        }
    }
    void RemoveChamp(int count)
    {
        ChampInstance temp;
        for (int i = 0; i < count; i++)
        {
            temp = championPool[i];
            Destroy(temp.champion);
        }
        championPool.RemoveRange(0, count);
    }
    #endregion

    public ChampInstance GetChamp(string name)
    {
        ChampInstance champ = default(ChampInstance);

        // name이 유효한 값인지 검사한다.
        if (IsVaildChampName(name) == false)
        {
            Debug.LogError(name + " is invalid value..");
            return champ;
        }

        for (int i = 0; i < championPool.Count; i++)
        {
            if (name.CompareTo(championPool[i].name) == 0)
            {
                champ = championPool[i];
                championPool.RemoveAt(i);
                return champ;
            }
        }// loop: name과 일치하는 오브젝트를 찾는다.

        if (champ == default(ChampInstance))
        {
            MakeChamp(name);
            champ = GetChamp(name);
        }// if: 없는 오브젝트를 새로 만들어서 리턴한다.

        return champ;
    }
    public void GetBackChamp(ChampInstance champ)
    {
        // 되돌려 받으면 초기화한다.
        champ.champion.transform.parent = transform;
        champ.champion.transform.position = new Vector3(0, 0, 0);
        champ.champion.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        champ.champion.transform.localScale = new Vector3(1, 1, 1);
        if (champ.standingTile != default)
        {
            champ.standingTile.isEmpty = true;
            champ.standingTile = default;
        }
        champ.quality = 1;
        champ.isClickOk = false;
        champ.champion.gameObject.SetActive(false);
        championPool.Add(champ);
    }

    bool IsVaildChampName(string name)
    {
        foreach (var ele in championNames)
        {
            if (ele.CompareTo(name) == 0)
            {
                return true;
            }
        }
        return false;
    }
}
