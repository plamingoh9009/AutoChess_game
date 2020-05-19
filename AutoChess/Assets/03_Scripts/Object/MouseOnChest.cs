using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOnChest : MonoBehaviour
{
    #region Start()
    Camera _mainCam;        // 마우스 정보를 얻어올 메인카메라
    GameObject[] _chests;   // 상자 오브젝트
    Ray _mouseRay;          // 마우스에서 쏘는 레이
    RaycastHit _mouseHit;   // 마우스 레이 히트
    bool _isMouseOnChest;   // 마우스가 상자를 가리키는지 식별하는 변수

    GameObject _shop;       // 상점을 열 때 쓰는 변수
    private void Awake()
    {
        _mainCam = Camera.main;
        _chests = null;
        _mouseRay = default(Ray);
        _mouseHit = default(RaycastHit);
        _isMouseOnChest = false;
    }
    private void Start()
    {
        _shop = GameObject.Find("Shop");
        GetChests();
    }
    #endregion
    #region Collider_compute
    private void OnMouseOver()
    {
        // 레이캐스트로 마우스가 오브젝트를 가리키는지 검사한다.
        _mouseRay = _mainCam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_mouseRay, out _mouseHit);
        if (IsMouseOnChest(_mouseHit))
        {
            OnOutlines();
            _isMouseOnChest = true;
        }// if: 마우스가 상자를 가리키고 있으면 outline을 렌더한다.

        if (_isMouseOnChest && Input.GetMouseButtonDown(0))
        {
            _shop.transform.Find("ShopObj").gameObject.SetActive(true);
        }// if: 상자를 클릭했다면 상점을 연다.
    }
    private void OnMouseExit()
    {
        // 마우스가 콜라이더를 벗어나면 아웃라인을 끈다
        OffOutlines();
        _isMouseOnChest = false;
    }
    #endregion

    void GetChests()
    {
        int count = transform.childCount;
        if (count <= 0) { }
        else
        {
            _chests = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                _chests[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    #region Outline On/Off
    void OnOutlines()
    {
        foreach (GameObject chest in _chests)
        {
            if (chest.GetComponent<Outline>().enabled == false)
            {
                chest.GetComponent<Outline>().enabled = true;
            }
        }
    }
    void OffOutlines()
    {
        foreach (GameObject chest in _chests)
        {
            if (chest.GetComponent<Outline>().enabled == true)
            {
                chest.GetComponent<Outline>().enabled = false;
            }
        }
    }
    #endregion

    bool IsMouseOnChest(RaycastHit hit)
    {
        Collider target = hit.collider;
        if (target == null) { }
        else
        {
            if (transform.position == target.transform.position)
            {
                return true;
            }
        }
        return false;
    }
}
