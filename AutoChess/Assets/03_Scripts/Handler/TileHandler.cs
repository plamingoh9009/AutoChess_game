﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileHandler : MonoBehaviour
{
    public enum TileType
    {
        SQUARE,
        HEXAGON
    }
    GameObject _squareTileContainer;
    GameObject _hexaTileContainer;
    public List<TileInfo> _squareInstances { get; private set; }
    public List<TileInfo> _hexaInstances { get; private set; }
    public class TileInfo
    {
        public GameObject tile;
        public int idx;
        public TileType type;
    }

    private void Awake()
    {
        _squareTileContainer = GameObject.Find("Tiles/SquareTiles");
        _hexaTileContainer = GameObject.Find("Tiles/HexaTiles");
        _squareInstances = new List<TileInfo>();
        _hexaInstances = new List<TileInfo>();

        SetupSquareTiles();
        SetupHexagonTiles();
    }
    #region Tile setup
    void SetupSquareTiles()
    {
        int rows = 9;
        float distance = 2.5f;

        MakeTiles(TileType.SQUARE, distance, rows);
    }
    void SetupHexagonTiles()
    {
        int rows = 7;
        int cols = 3;
        float distance = 3f;

        MakeTiles(TileType.HEXAGON, distance, rows, cols);
    }
    #endregion
    void MakeTiles(TileType type, float distance, int rows, int cols = 1)
    {
        string path = MyFunc.GetPath(MyFunc.PathType.PREFABS);
        string objName = "";
        GameObject prefab = default;
        GameObject parrent = default;
        List<TileInfo> instanceList = default;

        switch (type)
        {
            case TileType.SQUARE:
                objName = "indicator square.prefab";
                parrent = _squareTileContainer;
                instanceList = _squareInstances;
                break;
            case TileType.HEXAGON:
                objName = "indicator hexa.prefab";
                parrent = _hexaTileContainer;
                instanceList = _hexaInstances;
                break;
            default:
                break;
        }
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + objName);

        float rowPos, colPos;
        for (int i = 0; i < cols; i++)
        {
            for (int k = 0; k < rows; k++)
            {
                TileInfo tileInfo = new TileInfo();
                tileInfo.tile = Instantiate(prefab,
                    parrent.transform.position, Quaternion.identity, parrent.transform);
                switch (type)
                {
                    case TileType.HEXAGON:
                        rowPos = distance * k;
                        colPos = i * (distance * 0.9f);
                        if (i % 2 == 1)
                        {
                            rowPos -= distance * 0.5f;
                        }
                        tileInfo.tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        tileInfo.type = TileType.HEXAGON;
                        tileInfo.idx = i * rows + k;
                        break;
                    default:
                        rowPos = distance * k;
                        colPos = distance * i;
                        tileInfo.tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        tileInfo.type = TileType.SQUARE;
                        tileInfo.idx = i * rows + k;
                        break;
                }
                tileInfo.tile.SetActive(false);
                instanceList.Add(tileInfo);
            }
        }// loop: 행, 열 매개변수를 참고해서 타일을 만든다.
    }
    public TileInfo FindTile(List<TileInfo> tileList, Vector3 position)
    {
        foreach(var ele in tileList)
        {
            if(ele.tile.transform.position == position)
            {
                return ele;
            }
        }
        return default;
    }
    #region Tile on / off
    public void TileOn()
    {
        MyFunc.SetActiveAll(_squareInstances);
        MyFunc.SetActiveAll(_hexaInstances);
    }
    public void TileOff()
    {
        MyFunc.SetActiveAll(_squareInstances, false);
        MyFunc.SetActiveAll(_hexaInstances, false);
    }
    #endregion
}
