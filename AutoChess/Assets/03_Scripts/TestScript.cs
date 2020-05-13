using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Inventory inven;
    private void Awake()
    {
        inven = GameObject.Find("FixedObject/Inventory").GetComponent<Inventory>();
    }
    public void OnTest1Click()
    {
        inven.IntoInventory("Frost Archer");
    }
    public void OnTest2Click()
    {
    }
}
