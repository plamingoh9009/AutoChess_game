using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    TileHandler tileHandler;                // 타일 처리 클래스
    GameObject unitObj;                     // 내 유닛 오브젝트
    public ChampionPool.ChampInstance unit; // 내 유닛 인스턴스
    TileHandler.TileInfo landingTile;       // 유닛이 착지할 타일
    Inventory inven;

    GameObject tileFinderObj;           // 타일을 찾아주는 오브젝트
    TileFinder tileFinder;
    Vector3 benchmarkPos;               // Tile finder 연산의 기준이 되는 좌표
    float posCorrectionX;               // Tile finder 연산의 기준이 되는 상수
    float posCorrectionZ;
    float cameraCorrectionX;
    float cameraCorrectionZ;
    private void Awake()
    {
        unitObj = transform.parent.parent.gameObject;
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        landingTile = new TileHandler.TileInfo();
        tileFinderObj = unitObj.transform.Find("TileFinder").gameObject;
        posCorrectionZ = 2.4f;
        posCorrectionX = 1.5f;
        cameraCorrectionX = 0.05f;
        cameraCorrectionZ = 0.0415f;
    }
    private void Start()
    {
        benchmarkPos = tileHandler.squareInstances[4].tile.transform.position;
        tileFinder = tileFinderObj.GetComponent<TileFinder>();
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
        unit = inven.FindChampFromInstance(unitObj);
    }
    #region Mouse control
    private void OnMouseDown()
    {
        unit = inven.FindChampFromInstance(unitObj);
        if (unit != default && unit.isClickOk)
        {
            tileFinderObj.SetActive(true);
        }
    }
    private void OnMouseDrag()
    {
        if(unit != default && unit.isClickOk)
        {
            if ((GameManager.instance.myTurn == GameManager.TurnType.FIGHT) &&
            (unit.standingTile.type == TileHandler.TileType.HEXAGON)) { }
            else
            {
                // 드래그 할 때 모든 타일 활성화
                tileHandler.TileOn();
                // 마우스 좌표 받아오기
                FollowMouse();
            }// if: Fight 턴에 필드 드래그 불가
        }
        
    }
    private void OnMouseUp()
    {
        if(unit != default && unit.isClickOk)
        {
            if ((GameManager.instance.myTurn == GameManager.TurnType.FIGHT) &&
            (unit.standingTile.type == TileHandler.TileType.HEXAGON)) { }
            else
            {
                LandingUnit();
                // 놔두면 비활성화
                tileHandler.TileOff();
                tileFinderObj.SetActive(false);
            }// if: Fight 턴에 필드 드래그 불가
        }
    }
    void FollowMouse()
    {
        // 마우스 좌표를 스크린 -> 월드 좌표로 바꾼 후 리턴
        Vector3 unitMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        unitObj.transform.position = Camera.main.ScreenToWorldPoint(unitMousePos);

        if (tileFinder)
        {
            Vector3 tileMousePos = new Vector3(unitMousePos.x, unitMousePos.y, 18);
            Vector3 tilePos = Camera.main.ScreenToWorldPoint(tileMousePos);
            ComputeTileFinderPos(tilePos);
        }
    }
    #endregion
    #region Landing()
    void LandingUnit()
    {
        // 랜딩 위치를 잡아준다.
        SetupLandingTile();

        if (landingTile != default)
        {
            if (GameManager.instance.myTurn == GameManager.TurnType.FIGHT &&
                landingTile.type == TileHandler.TileType.HEXAGON)
            {
                ReturnRightPos();
            }
            else
            {
                LandingUnitRightPos();
            }// if: Fight 턴에 필드로 낼 수 없다.
        }// if: 착지할 위치가 타일 형태로 존재한다면
        else
        {
            ReturnRightPos();

            // 상자에 드래그 하면 판다.
            if (tileFinder.isChest)
            {
                inven.SellChampToList(unit);
            }
        }
    }
    void LandingUnitRightPos()
    {
        if (inven.IsEmptyTile(landingTile))
        {
            LandToTile();
        }
        else
        {
            SwapTwoTiles();
        }
    }
    void LandToTile()
    {
        Vector3 landingPos;

        if (landingTile == unit.standingTile)
        {
            ReturnRightPos();
        }
        else
        {
            landingPos = landingTile.tile.transform.position;
            unitObj.transform.position = landingPos;
            inven.MoveChamp(unitObj, landingTile);
        }
    }
    void SwapTwoTiles()
    {
        // 자리를 swap 하기 위해서 무조건 되돌아간다.
        ReturnRightPos();
        inven.SwapChamp(landingTile, unit.standingTile);
    }
    #endregion

    #region finished work
    void SetupLandingTile()
    {
        landingTile = tileFinder.detectedTile;
    }
    void ReturnRightPos()
    {
        Vector3 landingPos = unit.standingTile.tile.transform.position;
        unitObj.transform.position = landingPos;
    }
    void ComputeTileFinderPos(Vector3 before)
    {
        // distance와 correction 상수를 곱해서 적당한 거리를 계산한다.
        // distance: 기준점에서 오브젝트가 얼마나 떨어져 있는지 계산한 수 (단위는 Unit)
        // correction: 노가다로 찾아낸 상수, distance만으로 모자란 값을 보정한다.
        // forwardWeight: 마우스 Vector3을 고려했을 때 연산한 값. Z축 가중치다.
        Vector3 tilePos = before;
        float distanceX = (before.x < benchmarkPos.x ?
            (benchmarkPos.x - before.x) * -1 : before.x - benchmarkPos.x);
        float distanceZ = before.z - benchmarkPos.z;
        float forwardWeight = distanceZ / posCorrectionZ;
        tilePos.x = tilePos.x +
            (distanceX * posCorrectionX) * (forwardWeight * cameraCorrectionX);
        tilePos.y = benchmarkPos.y + 1;
        tilePos.z = tilePos.z +
            (distanceZ * posCorrectionZ) * (forwardWeight * cameraCorrectionZ);
        tileFinder.transform.position = tilePos;
    }
    #endregion
}
