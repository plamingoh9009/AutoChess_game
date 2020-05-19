using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    TileHandler tileHandler;            // 타일 처리 클래스
    GameObject unit;                    // 내 유닛 오브젝트
    TileHandler.TileInfo standingTile;  // 유닛이 서 있던 타일
    TileHandler.TileInfo landingTile;   // 유닛이 착지할 타일
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
        unit = transform.parent.gameObject;
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        standingTile = new TileHandler.TileInfo();
        landingTile = new TileHandler.TileInfo();
        tileFinderObj = unit.transform.Find("TileFinder").gameObject;
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
    }
    #region Mouse control
    private void OnMouseDown()
    {
        tileFinderObj.SetActive(true);
        StartCoroutine(SetupStandingTile());
    }
    private void OnMouseDrag()
    {
        // 드래그 할 때 모든 타일 활성화
        tileHandler.TileOn();
        // 마우스 좌표 받아오기
        FollowMouse();
    }
    private void OnMouseUp()
    {
        LandingUnit();
        // 놔두면 비활성화
        tileHandler.TileOff();
        tileFinderObj.SetActive(false);
    }
    void FollowMouse()
    {
        // 마우스 좌표를 스크린 -> 월드 좌표로 바꾼 후 리턴
        Vector3 unitMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        unit.transform.position = Camera.main.ScreenToWorldPoint(unitMousePos);

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

        if (inven.IsEmptyTile(landingTile))
        {
            LandToTile();
        }
        else
        {
            SwapTwoTiles();
        }
        inven.ShowInven();
        inven.ShowField();
    }
    bool LandToTile()
    {
        Vector3 landingPos;
        if (landingTile != default && landingTile.isEmpty)
        {
            landingPos = landingTile.tile.transform.position;
            unit.transform.position = landingPos;
            standingTile.isEmpty = true;
            landingTile.isEmpty = false;
            inven.MoveChamp(landingTile, standingTile);
            return true;
        }// if: 랜딩 포지션 값이 정상범위라면 그 곳으로 이동한다.

        // 랜딩 포지션 값이 이상하면 원래 서있던 자리로 되돌아간다.
        landingPos = standingTile.tile.transform.position;
        unit.transform.position = landingPos;
        return false;
    }
    bool SwapTwoTiles()
    {
        Vector3 landingPos;

        // 자리를 swap 하기 위해서 무조건 되돌아간다.
        landingPos = standingTile.tile.transform.position;
        unit.transform.position = landingPos;

        if (landingTile != default && !landingTile.isEmpty)
        {
            standingTile.isEmpty = false;
            landingTile.isEmpty = false;
            inven.SwapChamp(landingTile, standingTile);
            return true;
        }// if: 랜딩할 곳에 다른 유닛이 있다면 둘의 위치를 바꾼다.

        return false;
    }
    #endregion

    #region Setup per evety click
    IEnumerator SetupStandingTile()
    {
        // finder가 타일을 클릭 후에 찾기 시작 하기 때문에 1프레임 대기한다.
        yield return new WaitForSeconds(Time.deltaTime);
        if (tileFinder.detectedTile != null &&
            tileFinder.detectedTile.tile != null)
        {
            standingTile = tileFinder.detectedTile;
        }// if: 정상 타일인 경우
    }
    void SetupLandingTile()
    {
        landingTile = tileFinder.detectedTile;
    }
    #endregion
    public void InitStandingTile(TileHandler.TileInfo standingTile_)
    {
        standingTile = standingTile_;
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
}
