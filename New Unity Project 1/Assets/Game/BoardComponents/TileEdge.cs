using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TileEdge : Tile
{
    public Number num;
    public TileEdge()
    {
       
    }
    public void init()
    {
        num = transform.GetComponentInChildren<Number>();// as Number;
    }
    void Start()
    {
        if (num == null) init();
    }
    void Update()
    {
        if (Input.touchCount == 1) ;
    }
    void OnMouseDown()
    {
        Debug.Log( "DOWN");
    }

}