using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFunc
{
    public enum PathType
    {
        PREFABS,
        CHAMP_INFO
    }
    public enum ObjType
    {
        #region Shop
        SHOP_OBJECT,
        SHOP_CONTAINER,
        SHOP_COLLIDER,
        #endregion
        #region Fixed object
        FIXED_OBJECT,
        INVENTORY,
        TILE_CONTAINER,
        CHEST_COLLIDER,
        TIMER,
        #endregion
        ENEMY,
        ENEMY_INFO,
        #region UI
        PLAYER_UI,
        MESSAGE_BOX,
        GOLD,
        PLAY_BUTTON
        #endregion
    }
    public static string GetPath(PathType type)
    {
        string path = default(string);
        switch (type)
        {
            case PathType.PREFABS:
                path = "Assets/04_Prefabs/";
                break;
            case PathType.CHAMP_INFO:
                path = "Assets/05_Infos/Champions/";
                break;
            default:
                break;
        }
        return path;
    }
    public static GameObject GetObject(ObjType type)
    {
        GameObject target = default;
        switch (type)
        {
            #region Shop
            case ObjType.SHOP_OBJECT:
                target = GameObject.Find("Shop/ShopObj");
                break;
            case ObjType.SHOP_CONTAINER:
                target = GetObject(ObjType.SHOP_OBJECT);
                target = target.transform.Find("ChampContainer").gameObject;
                break;
            case ObjType.SHOP_COLLIDER:
                target = GetObject(ObjType.SHOP_OBJECT);
                target = target.transform.Find("ShopCollider").gameObject;
                break;
            #endregion
            #region Fixed object
            case ObjType.FIXED_OBJECT:
                target = GameObject.Find("FixedObject");
                break;
            case ObjType.INVENTORY:
                target = GetObject(ObjType.FIXED_OBJECT);
                target = target.transform.Find("Inventory").gameObject;
                break;
            case ObjType.TILE_CONTAINER:
                target = GetObject(ObjType.FIXED_OBJECT);
                target = target.transform.Find("Tiles").gameObject;
                break;
            case ObjType.CHEST_COLLIDER:
                target = GetObject(ObjType.FIXED_OBJECT);
                target = target.transform.Find("Chests/ChestCollider").gameObject;
                break;
            case ObjType.TIMER:
                target = GetObject(ObjType.PLAYER_UI);
                target = target.transform.Find("Round_info/Timer").gameObject;
                break;
            #endregion
            case ObjType.ENEMY:
                target = GameObject.Find("Enemy");
                break;
            case ObjType.ENEMY_INFO:
                target = GetObject(ObjType.ENEMY);
                target = target.transform.Find("EnemyInfo").gameObject;
                break;
            #region UI
            case ObjType.PLAYER_UI:
                target = GameObject.Find("PlayerUi");
                break;
            case ObjType.MESSAGE_BOX:
                target = GetObject(ObjType.PLAYER_UI);
                target = target.transform.Find("MessageBox").gameObject;
                break;
            case ObjType.GOLD:
                target = GetObject(ObjType.PLAYER_UI);
                target = target.transform.Find("Gold").gameObject;
                break;
            case ObjType.PLAY_BUTTON:
                target = GetObject(ObjType.PLAYER_UI);
                target = target.transform.Find("StageButtons/PlayButton").gameObject;
                break;
            #endregion
        }
        return target;
    }
    #region finished work
    public static void Swap<T>(ref T element1, ref T element2)
    {
        T temp = element1;
        element1 = element2;
        element2 = temp;
    }
    public static void Swap<T>(T element1, T element2)
    {
        T temp = element1;
        element1 = element2;
        element2 = temp;
    }
    public static void SetActiveAll(List<GameObject> list, bool isActive = true)
    {
        foreach (var ele in list)
        {
            ele.SetActive(isActive);
        }
    }
    public static void SetActiveAll(List<TileHandler.TileInfo> list, bool isActive = true)
    {
        foreach (var ele in list)
        {
            ele.tile.SetActive(isActive);
        }
    }
    #endregion
}
