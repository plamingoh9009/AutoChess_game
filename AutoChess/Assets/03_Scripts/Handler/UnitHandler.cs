using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    TileHandler tileHandler;        // 타일 처리 클래스
    GameObject unit;                // 내 유닛 오브젝트
    TileHandler.TileInfo tileInfo;  // 유닛이 서 있던 타일
    GameObject tileFinder;          // 타일을 찾아주는 오브제
    Vector3 firstUnitPos;           // 드래그 중에 사용할 유닛이 처음 서 있던 자리
    private void Awake()
    {
        unit = transform.parent.gameObject;
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.TILE_CONTAINER).GetComponent<TileHandler>();
        tileFinder = unit.transform.Find("TileFinder").gameObject;
        firstUnitPos = default;
        if(tileFinder)
            tileFinder.SetActive(true);
    }
    private void Start()
    {
        tileInfo = tileHandler.FindTile(tileHandler._squareInstances, unit.transform.position);
    }

    private void OnMouseDown()
    {
        firstUnitPos = tileInfo.tile.transform.position;
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
        //LandToTile();

        // 놔두면 비활성화
        tileHandler.TileOff();
        firstUnitPos = default;
    }
    void FollowMouse()
    {
        // 마우스 좌표를 스크린 -> 월드 좌표로 바꾼 후 리턴
        float correctionY = 1f;
        float correctionZ = 2.5f;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        unit.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        
        if(tileFinder)
        {
            Vector3 tileFinderPos = new Vector3(mousePos.x, mousePos.y, 20);
            Vector3 temp;
            temp = tileFinder.transform.position = Camera.main.ScreenToWorldPoint(tileFinderPos);
            Debug.Log((temp.x, temp.y, temp.z));
        }
        
    }
    void LandToTile()
    {
        Vector3 unitPos = unit.transform.position;
        Vector3 landingPos = new Vector3(unitPos.x, firstUnitPos.y, unitPos.z);
        // 랜딩 위치를 잡아준다.
        unit.transform.position = landingPos;
    }
}
