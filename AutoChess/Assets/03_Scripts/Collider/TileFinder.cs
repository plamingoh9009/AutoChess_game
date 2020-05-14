using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFinder : MonoBehaviour
{
    TileHandler tileHandler;
    List<TileHandler.TileInfo> invenTiles;
    List<TileHandler.TileInfo> fieldTiles;
    TileHandler.TileInfo targetTile;

    private void Awake()
    {
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        targetTile = new TileHandler.TileInfo();
    }
    private void Start()
    {
        invenTiles = tileHandler._squareInstances;
        fieldTiles = tileHandler._hexaInstances;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("InvenTile"))
        {
            targetTile = tileHandler.FindTile(invenTiles, other.transform.position);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("FieldTile"))
        {
            targetTile = tileHandler.FindTile(fieldTiles, other.transform.position);
        }
        Debug.Log("Type: " + targetTile.type);
        Debug.Log("Idx: " + targetTile.idx);
    }
    private void OnTriggerExit(Collider other)
    {
        targetTile = default;
    }
}
