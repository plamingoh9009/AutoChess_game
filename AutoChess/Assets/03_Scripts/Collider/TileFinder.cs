using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFinder : MonoBehaviour
{
    TileHandler tileHandler;
    List<TileHandler.TileInfo> invenTiles;
    List<TileHandler.TileInfo> fieldTiles;
    public TileHandler.TileInfo detectedTile;
    public bool isEmptyTile;

    private void Awake()
    {
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        detectedTile = new TileHandler.TileInfo();
        isEmptyTile = true;
    }
    private void Start()
    {
        invenTiles = tileHandler.squareInstances;
        fieldTiles = tileHandler.hexaInstances;
    }

    private void OnTriggerEnter(Collider other)
    {
        DetectTile(other);
        DetectUnit(other);
    }
    private void OnTriggerStay(Collider other)
    {
        DetectTile(other);
        DetectUnit(other);
    }
    private void OnTriggerExit(Collider other)
    {
        detectedTile = default;
    }
    void DetectTile(Collider other)
    {
        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case "InvenTile":
                detectedTile = tileHandler.FindTile(invenTiles, other.transform.parent.position);
                break;
            case "FieldTile":
                detectedTile = tileHandler.FindTile(fieldTiles, other.transform.parent.position);
                break;
            default:
                detectedTile = default;
                break;
        }
    }
    void DetectUnit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            isEmptyTile = false;
        }
        else
        {
            isEmptyTile = true;
        }
    }
}
