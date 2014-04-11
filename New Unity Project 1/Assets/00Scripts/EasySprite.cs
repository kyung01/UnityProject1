using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EasySprite : EasyGameObject
{
    public int sortingOrder;
    void Awake()
    {
        renderer.sortingOrder = sortingOrder;
    }
}