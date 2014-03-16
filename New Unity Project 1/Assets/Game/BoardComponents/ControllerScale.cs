using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class ControllerScale : EasyGameObject
{
    public GameObject cam;

    bool isStable = true;
    Vector2 posMidInit;
    void Start()
    {
        
    }
    void moveCamera(Vector3 movement)
    {

    }
    void process(Vector2 midNew, Vector2 zoom)
    {
        var move = midNew - posMidInit;
        
        transform.position += move.XYZ() * Time.deltaTime;
    }
    void Update()
    {
        //Debug.Log(Input.touchCount);
        if (Input.touchCount < 2)
        {
            if (isStable) return;
            //do some clean up here
            isStable = true;
            return;
        }
        //get initial values
        Vector2[] touches = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
        Vector2 dis = touches[1] - touches[0];
        Vector2 mid = touches[0] + dis * .5f;
        // if not first iteration, process. otherwise, wait for next tick.
        if (!isStable) process(mid, dis);
        isStable = false;
        posMidInit = mid;
    }
}