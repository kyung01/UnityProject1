﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TileContent : Tile
{
    void Start()
    {
        setColor(0, 0, 0, 0);
        //setColor(1, 1, 1, .5f);
    }
    //public void OnMouseOver()
    //{
    //    setColor(Random.Range(1, 10) * .1f, 0, 0);
    //}
    public void displayState(int state)
    {
        //switch (state)
        //{
        //    case 0:
        //        setColor(1, 1, 1, .5f);
        //        break;
        //    case 1:
        //        setColor(1, 0, 0);
        //        break;
        //    case 2:
        //        setColor(0,1, 0);
        //        break;
        //    case 3:
        //        setColor(0, 0, 1);
        //        break;
        //    default :
        //        setColor(0, 0, 0);break;
        //}
    }
}