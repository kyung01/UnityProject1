#pragma warning disable 0649

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class DrawHelper : MonoBehaviour
{
    //create an instance to have access to all the prefabs. 
    static DrawHelper instance;

    public GameObject PREFAB_rect;

    static GameObject getRect() { return instance.PREFAB_rect; }
    
    public DrawHelper(){ instance = this;}
    public static GameObject drawLine(Vector3 from, Vector3 to, float size, Vector4 color)
    {
        return drawLine(from, to, size, color.x, color.y, color.z, color.w);
    }
    public static GameObject drawRect(Vector3 from, Vector2 size)
    {
        var obj = Instantiate(instance.PREFAB_rect, from, Quaternion.identity) as GameObject;
        obj.transform.localScale = size.XYZ(1);
        return obj;
    }
    public static GameObject drawLine(Vector3 from, Vector3 to, float size, float r = 0, float g = 0, float b = 0, float a = 1)
    {
        var obj = Instantiate(instance.PREFAB_rect, (to + from) * .5f, Quaternion.identity) as GameObject;
        var dir = (to - from);
        obj.transform.localScale = new Vector3(dir.magnitude, size, 1);
        obj.transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), dir);
        (obj.GetComponent(typeof(EasyGameObject) ) as EasyGameObject). setColor(r,g,b,a);
        return obj; 
    }
}