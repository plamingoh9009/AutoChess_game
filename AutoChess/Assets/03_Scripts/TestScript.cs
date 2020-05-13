using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public static List<ChampionPool.ChampInstance> testPool;
    private void Awake()
    {
        testPool = new List<ChampionPool.ChampInstance>();
    }
    public void OnTest1Click()
    {
        ChampionPool.ChampInstance myChamp;
        myChamp = ChampionPool.instance.GetChamp("Nature Knight");
        testPool.Add(myChamp);
    }
    public void OnTest2Click()
    {
        ChampionPool.ChampInstance myChamp;
        myChamp = testPool[0];
        testPool.Remove(myChamp);
        ChampionPool.instance.GetBackChamp(myChamp);
    }
}
