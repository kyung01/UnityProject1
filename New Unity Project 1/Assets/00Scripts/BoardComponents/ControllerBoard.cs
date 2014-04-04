#pragma warning disable 0649

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class ControllerBoard : EasyGameObject
{
    static float touchMinMoveDetect = 10;
    public delegate void delTouch(Vector3 v);
    public event delTouch EVENT_TouchClick = delegate { };
    public event delTouch EVENT_TouchHold = delegate { };
    public event delTouch Event_TouchUp = delegate { };

    enum ControllerState {idl, determineSingleTouch, onHold, freeMove,zoom, centered };
    ControllerState myState = ControllerState.determineSingleTouch;
    GameObject bodyMain;
    GameObject bodySub;

    bool isFirstTick = true;
    Vector2 posMidInit;
    float timeHold, timeHoldMax;

    float inputBeofreNum;
    Vector3 inputBefore; // when moving body, position of the body based off of input 
    public ControllerBoard init(GameObject bodyMain, GameObject bodySub)
    {
        this.bodyMain = bodyMain;
        this.bodySub = bodySub;
        return this;
    }
    
    //float timeHold;
    void Start()
    {
        timeHoldMax = 1.5f;
        EVENT_TouchClick += delegate { Debug.Log("debug touckClick"); };
    }
    void moveCamera(Vector3 movement)
    {

    }
    void process(Vector2 midNew, Vector2 zoom)
    {
        var move = midNew - posMidInit;
        
        transform.position += move.XYZ() * Time.deltaTime;
    }
    void changeState(ControllerState state)
    {
        Event_TouchUp(new Vector3());
        isFirstTick = true;
        timeHold = 0;
        inputBeofreNum = 0;
        inputBefore = new Vector3();
        myState = state;
    }
    void UpdateIdl()
    {
        switch (InputManager.getInputCount())
        {
            case 1:
                changeState(ControllerState.determineSingleTouch);
                break;
            case 2:
                changeState(ControllerState.zoom);
                break;
        }

    }
    void UpdateDetermineAction()
    {
        int count = InputManager.getInputCount();
        Debug.Log("debug count " + count);
        if (count != 1)
        {
            if (count == 0) EVENT_TouchClick(InputManager.getInputAt(0));
            changeState(ControllerState.idl);
            return;
        }
        //remove this once you are ready to publish
        if (Input.touchCount == 0) return;// mouse input, can't process any further due to 

        timeHold += Time.deltaTime;
        if(timeHold >= timeHoldMax) changeState(ControllerState.onHold);
        else if(Input.GetTouch(0).deltaPosition.magnitude > touchMinMoveDetect) 
            changeState(ControllerState.freeMove);
         
    }
    Vector3[] helperGetInputRay(int n, bool isWorld = true)
    {
        Vector3 from = Camera.main.transform.position,
                to = InputManager.getInputAt(n) + new Vector3(0, 0, 100);
        if (isWorld) to = Camera.main.ScreenToWorldPoint(to);
        else from = Camera.main.WorldToScreenPoint(from);
        return new Vector3[] {from,to };
    }
    void UpdateFreeMove(bool isMouse = false)
    {
        if (InputManager.getInputCount(isMouse) != 1) { changeState(ControllerState.idl); }
        var ray = helperGetInputRay(0);
        Vector3 dis = ray[1] - ray[0], // from to "to"
                at = ray[0] + dis * Math.Abs((bodyMain.transform.position.z - ray[0].z) / dis.z);
        if (isFirstTick) {
            isFirstTick = false;
            inputBefore = at - bodyMain.transform.position; 
            return;
        }
        var move = (at - (bodyMain.transform.position+inputBefore)).mult(1, 1, 0);
        bodyMain.transform.position += move * Time.deltaTime;
    }
    void UpdateOnHoldDrag()
    {
        if (InputManager.getInputCount() != 1) { changeState(ControllerState.idl); return; }
        EVENT_TouchHold(InputManager.getInputAt(0));

        timeHold += Time.deltaTime;
        if (timeHold >= timeHoldMax)
        {
            bodyMain.transform.position = new Vector3(.48f, .38f, -4.7f);
            changeState(ControllerState.idl);
        }
    }
    float zoomRate = 10.0f;
    void UpdateZoom()
    {
        if (InputManager.getInputCount(false) < 2) { changeState(ControllerState.freeMove); return; }

        Vector3[] axis = new Vector3[] { InputManager.getInputAt(0), InputManager.getInputAt(1) };
        Vector3 dis = axis[1] - axis[0];
        dis = dis.mult(1 / (float)Screen.width, 1 / (float)Screen.height, 1);
        if (isFirstTick){
            isFirstTick = false;
            inputBefore = dis;
            inputBeofreNum = bodyMain.transform.position.z;
            return;
        }
        float move = (dis.magnitude - inputBefore.magnitude) * zoomRate;
        bodyMain.transform.position = new Vector3(
            bodyMain.transform.position.x, bodyMain.transform.position.y, 
            inputBeofreNum - move);
    }
    void Update()
    {
        //Debug.Log(myState);
        //if(UpdateFreeMove(true))
        //    isFirstTick = false;
        //return;
        
        //if (getInputCount() != 1) { changeState(ControllerState.ready); return; }
        //var at = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, body.transform.position.z));
        //if (mousePosBefore == null) { mousePosBefore = at; return; }
        //body.transform.position += (mousePosBefore.Value - at);
        //Debug.Log("MOVED " + (at - mousePosBefore.Value) + " " + at + " " + mousePosBefore.Value);
        //mousePosBefore = at;
        //return;
        //700 500
        switch (myState)
        {
            case ControllerState.idl:
                UpdateIdl();
                break;
            case ControllerState.determineSingleTouch:
                UpdateDetermineAction();
                break;
            case ControllerState.freeMove:
                UpdateFreeMove();
                break;
            case ControllerState.onHold:
                UpdateOnHoldDrag();
                break;
            case ControllerState.zoom:
                UpdateZoom();
                break;
        }

        //isFirstTick = false;
        
        
    }
}
/**
 * 
        Debug.Log(Input.touchCount);
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
**/