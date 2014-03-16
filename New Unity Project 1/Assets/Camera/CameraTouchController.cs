using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class CameraTouchController : MonoBehaviour
{
    //have controller rest when not in need
    bool isStable = true;
    CameraMovementSmoother smoother;
    Vector2 posMidInit;
    void Start()
    {
        smoother = transform.gameObject.AddComponent(typeof(CameraMovementSmoother)) as CameraMovementSmoother;

    }
    void moveCamera(Vector3 movement)
    {
        
    }
    void process(Vector2 midNew, Vector2 zoom)
    {
        var move = midNew - posMidInit;
        camera.transform.position += move.XYZ() * Time.deltaTime;
    }
    void Update()
    {
        if(Input.touchCount <= 1){
            if(isStable ) return;
            //do some clean up here
        }
        //get initial values
        Vector2[] touches = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
        Vector2 dis = touches[1] - touches[0];
        Vector2 mid = touches[0] + dis * .5f;
        // if first iteration, don't process and wait for the next tick.
        if (!isStable) process(mid, dis);
        isStable = false;
        posMidInit = mid;


    }
}