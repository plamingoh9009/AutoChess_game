using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChampionPool : MonoBehaviour
{
    public static ChampionPool instance = null;
    public class ChampInstance
    {
        public string name;
        public GameObject champion;
    }
    public List<string> championNames { get; private set; }
    public Dictionary<string, GameObject> championPrefabs { get; private set; }
    List<ChampInstance> _championPool;      // 오브젝트 풀
    string _prefabPath;                     // 챔피언 프리팹 경로
    int _addObjectCnt;                      // 풀에 오브젝트를 얼마나 추가할 지

    #region Start()
    private void Awake()
    {
        SetupSingleton();
        _addObjectCnt = 3;
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

    private void Start()
    {
        SetupPrefabs();
        InitObjPool();
    }
    #endregion
    void SetupPrefabs()
    {
        _prefabPath = "04_Prefabs/Champions";

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
        for (int i = 0; i < championNames.Count; i++)
        {
            championPrefabs.Add(championNames[i], AssetDatabase.LoadAssetAtPath<GameObject>(
                _prefabPath + championNames[i] + ".prefab")
                );
        }
        _championPool = new List<ChampInstance>();
    }
    void InitObjPool()
    {
        ChampInstance myInstance;
        GameObject myChamp;

        foreach(var ele in championPrefabs)
        {
            Debug.Log("Prefabs: " + ele.Value);
        }
        //for (int i = 0; i < championPrefabs.Count; i++)
        //{
        //    for (int k = 0; k < _addObjectCnt; k++)
        //    {
        //        // 프리팹 인스턴스화 + 오브젝트 풀에 넣기
        //        championPrefabs.TryGetValue(championNames[i], out myChamp);
        //        myInstance = new ChampInstance();
        //        myInstance.name = championNames[i];
        //        myInstance.champion = MonoBehaviour.Instantiate<GameObject>(myChamp);
        //        _championPool.Add(myInstance);
        //    }
        //}
        //Debug.Log(_championPool.Count);
    }
}
