using System.Collections;
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
    public List<GameObject> _squareInstances { get; private set; }
    public List<GameObject> _hexaInstances { get; private set; }

    private void Awake()
    {
        _squareTileContainer = GameObject.Find("Tiles/SquareTiles");
        _hexaTileContainer = GameObject.Find("Tiles/HexaTiles");
        _squareInstances = new List<GameObject>();
        _hexaInstances = new List<GameObject>();

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
        List<GameObject> instanceList = default;

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

        GameObject tile;
        float rowPos, colPos;
        for (int i = 0; i < cols; i++)
        {
            for (int k = 0; k < rows; k++)
            {
                tile = Instantiate(prefab, parrent.transform.position, Quaternion.identity, parrent.transform);
                switch (type)
                {
                    case TileType.HEXAGON:
                        rowPos = distance * k;
                        colPos = i * (distance * 0.9f);
                        if (i % 2 == 1)
                        {
                            rowPos -= distance * 0.5f;
                        }
                        tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        break;
                    default:
                        rowPos = distance * k;
                        colPos = distance * i;
                        tile.transform.Translate(new Vector3(rowPos, 0, colPos));
                        break;
                }
                tile.SetActive(false);
                instanceList.Add(tile);
            }
        }// loop: 행, 열 매개변수를 참고해서 타일을 만든다.
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
