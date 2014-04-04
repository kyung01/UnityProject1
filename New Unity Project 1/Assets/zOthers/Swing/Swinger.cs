using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;
class Swinger : EasyGameObject
{
    static float COEF_RECOVER = .1f;

    public Vector3 dirFace = new Vector3(0, 1, 0);
    Vector3 dir, velo = new Vector3();
    Quaternion rotInit;
    void Start()
    {
        dir = transform.rotation * dirFace;
    }
    void helperApplyRotation(Vector3 rotChange)
    {
        
    }
    Vector3 helperGetCloserAngle(Vector3 v)
    {
        for (int i = 0; i < 3; i++){
            float opposite = v[i] + 360 ;
            if (Mathf.Abs(opposite) < Mathf.Abs(v[i])) 
                v[i] = opposite;
        }
        return v;
    }
    void helperResetRotation()
    {
        var angle = Quaternion.LookRotation(dir).eulerAngles - transform.rotation.eulerAngles;
        angle = helperGetCloserAngle(angle);
        velo *= 1.0f - (.1f * Time.fixedDeltaTime);
        velo += angle.sqrAbs() * COEF_RECOVER * Time.fixedDeltaTime;
       // Debug.Log(angle + "        " + move);
       // Vector3 posBefore = transform.position;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + velo * Time.fixedDeltaTime);
       // transform.position = posBefore;
    }
    void FixedUpdate()
    {
        helperResetRotation();
    }
}