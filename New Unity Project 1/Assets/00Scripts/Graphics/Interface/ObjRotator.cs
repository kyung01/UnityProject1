using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ObjRotator
{
    GameObject obj;
    public GameObject Obj { get { return obj; } }
    Vector3 from, to, center;
    float time, timeMax;
    public ObjRotator(GameObject obj, Vector3 from, Vector3 to, Vector3 center, float timeTakes)
    {
        this.obj = obj;
        this.from = from;
        this.to = to;
        this.center = center;
        this.time = 0;
        this.timeMax = timeTakes;
    }
    public bool update()
    {
        time += Time.deltaTime;
        float prog =Math.Min(time / timeMax ,1);
        obj.transform.position = from + (to - from) * prog;
        obj.transform.rotation = Quaternion.LookRotation( obj.transform.position-center);
        return prog < 1;
    }
}