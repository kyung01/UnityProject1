using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TileEdge : Tile
{
    public EasyTextMesh num;
    public TileEdge()
    {
       
    }
    void init()
    {
        renderer.enabled = false;

    }
    void Awake()
    {
        renderer.enabled = false;
    }
    void Start()
    {
        if (num == null) init();
    }
    void Update()
    {
        //if (Input.touchCount == 1) ;
    }
    void OnMouseDown()
    {
        //Debug.Log( "DOWN");
    }
    public void display(int n)
    {
        //Debug.Log("Requested to display " + n);
        num.display(""+n);
    }

}