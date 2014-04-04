using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class JointBasic : EasyGameObject
{
    enum MyState { Stationary, Updating };
    const float    
                    TH_MOVE_POS_DIFF = .003f,
                    MIN_DIFF = .001f,
                    RATIO_FLEX = 52.5f;
    
    GameObject joint;
    //private
    Vector3 dis, veloSway = new Vector3();
    float disMag;

    void Awake()
    { }
    public void init(GameObject joint)
    {
        this.joint = joint;
        dis = transform.position - joint.transform.position;
        disMag = dis.magnitude;
    }
    void Start()
    {
        
    }
    Vector3 helperGetToPosition() { return joint.transform.position + dis; }
    Vector3 HelperGetDirectionFromTo(Vector3 from, Vector3 to, Vector3 otherwise,float minValue = MIN_DIFF)
    {
        var diff = to - from;
        if (diff.sqrMagnitude < minValue) diff = otherwise;
        return diff.normalized;
    }
    void UpdateKeepLength(Vector3 jointPos, Vector3 dis, float length)
    {
        var diff = transform.position - jointPos;
        if (diff.sqrMagnitude < MIN_DIFF) {transform.position = jointPos + dis; return;}
        var dir = diff.normalized;
        transform.position = jointPos + dir * length;
    }
    void UpdateAlignRotation(Vector3 posJoint)
    {
        var euler = Quaternion.LookRotation(joint.transform.position - transform.position).eulerAngles + new Vector3(90, 0, 0);
        transform.rotation = Quaternion.Euler(euler);
    }
    void UpdateSway(Vector3 posTo)
    {
        var diff = posTo - transform.position;
        if (diff.sqrMagnitude < MIN_DIFF) return;
        veloSway += diff.mult(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z)) * RATIO_FLEX * Time.deltaTime;
    }
    void FixedUpdate()
    {
        transform.position += veloSway * Time.fixedDeltaTime;
        UpdateSway(joint.transform.position + dis);
        UpdateKeepLength(joint.transform.position, dis, disMag);
        UpdateAlignRotation(joint.transform.position);
    }
}