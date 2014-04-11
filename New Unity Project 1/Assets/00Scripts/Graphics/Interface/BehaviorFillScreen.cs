using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BehaviorFillScreen : EasyGameObject
{
    public float zDeapth = 5.0f;
    void Awake()
    {
        Vector3 bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, zDeapth)),
                top = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDeapth)),
                scale = top - bottom;
        var parent = transform.parent;
        transform.parent = null;

        transform.localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs( scale.y), 1);
        transform.parent = parent;
        //Camera.main.ScreenToWorldPoint()
    }
}