using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileHandler : MonoBehaviour
{
    #region Var
    public enum TileType
    {
        EMPTY,
        SQUARE,
        HEXAGON
    }
    GameObject _squareTileContainer;
    GameObject _hexaTileContainer;
    GameObject tileExplorerObj;
    TileFinder tileExplorer;
    public List<TileInfo> squareInstances { get; private set; }
    public List<TileInfo> hexaInstances { get; private set; }
    public class TileInfo
    {
        public GameObject tile;
        public int idx;
        public TileType type;
        public bool isEmpty;
    }
    #endregion
    private void Awake()
    {
        _squareTileContainer = transform.Find("SquareTiles").gameObject;
        _hexaTileContainer = transform.Find("HexaTiles").gameObject;
        tileExplorerObj = transform.Find("TileFinder").gameObject;
        tileExplorer = tileExplorerObj.GetComponent<TileFinder>();
        squareInstances = new List<TileInfo>();
        hexaInstances = new List<TileInfo>();
    }
    public void SetupTiles(bool isReverse = false)
    {
        SetupSquareTiles(isReverse);
        SetupHexagonTiles(isReverse);
    }
    #region Tile setup
    void SetupSquareTiles(bool isReverse = false)
    {
        int rows = 9;
        float distance = 2.5f;

        squareInstances = MakeTiles(TileType.SQUARE, distance, rows, 1, isReverse);
    }
    void SetupHexagonTiles(bool isReverse = false)
    {
        int rows = 7;
        int cols = 3;
        float distance = 3f;

        hexaInstances = MakeTiles(TileType.HEXAGON, distance, rows, cols, isReverse);
    }
    #endregion
    List<TileInfo> MakeTiles(TileType type, float distance, int rows, int cols = 1, bool isReverse = false)
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
                break;
            case TileType.HEXAGON:
                objName = "indicator hexa.prefab";
                parrent = _hexaTileContainer;
                break;
            default:
                break;
        }
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path + objName);
        instanceList = new List<TileInfo>();
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
                        if(isReverse)
                        {
                            rowPos *= -1;
                            colPos *= -1;
                        }
                        tileInfo.tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        tileInfo.type = TileType.HEXAGON;
                        tileInfo.idx = i * rows + k;
                        break;
                    default:
                        rowPos = distance * k;
                        colPos = distance * i;
                        if (isReverse)
                        {
                            rowPos *= -1;
                            colPos *= -1;
                        }
                        tileInfo.tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        tileInfo.type = TileType.SQUARE;
                        tileInfo.idx = i * rows + k;
                        break;
                }
                tileInfo.isEmpty = true;
                tileInfo.tile.SetActive(false);
                instanceList.Add(tileInfo);
            }
        }// loop: 행, 열 매개변수를 참고해서 타일을 만든다.
        return instanceList;
    }
    public TileInfo FindTile(List<TileInfo> tileList, GameObject tile)
    {
        foreach (var ele in tileList)
        {
            if (ele.tile == tile)
            {
                return ele;
            }
        }
        return default;
    }

    #region Tile on / off
    public void TileOn()
    {
        MyFunc.SetActiveAll(squareInstances);
        MyFunc.SetActiveAll(hexaInstances);
    }
    public void TileOff()
    {
        MyFunc.SetActiveAll(squareInstances, false);
        MyFunc.SetActiveAll(hexaInstances, false);
    }
    #endregion
}
