using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFinder : MonoBehaviour
{
    TileHandler tileHandler;
    List<TileHandler.TileInfo> invenTiles;
    List<TileHandler.TileInfo> fieldTiles;
    public TileHandler.TileInfo detectedTile;

    MouseOnChest chest;
    public bool isChest;

    private void Awake()
    {
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        detectedTile = new TileHandler.TileInfo();

        isChest = false;
    }
    private void Start()
    {
        invenTiles = tileHandler.squareInstances;
        fieldTiles = tileHandler.hexaInstances;
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
        DetectTile(other);
    }
    private void OnTriggerExit(Collider other)
    {
        detectedTile = default;
        isChest = false;
    }
    void DetectTile(Collider other)
    {
        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case "InvenTile":
                detectedTile = tileHandler.FindTile(invenTiles, other.transform.parent.gameObject);
                break;
            case "FieldTile":
                detectedTile = tileHandler.FindTile(fieldTiles, other.transform.parent.gameObject);
                break;
            case "Shop":
                isChest = true;
                break;
        }
    }
}
