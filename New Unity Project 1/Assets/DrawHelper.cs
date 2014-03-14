using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DrawHelper : MonoBehaviour
{
    //create an instance to have access to all the prefabs. 
    static DrawHelper instance;
    public GameObject PREFAB_rect;

    static GameObject getRect() { return instance.PREFAB_rect; }
    
    public DrawHelper(){ instance = this;}
    public static GameObject drawLine(Vector3 from, Vector3 to, float size)
    {
        var obj = Instantiate(instance.PREFAB_rect, (to + from) * .5f, Quaternion.identity) as GameObject;
        var dir = (to - from);
        obj.transform.localScale = new Vector3(dir.magnitude, size, 1);
        obj.transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), dir);
        return obj;
    }
}