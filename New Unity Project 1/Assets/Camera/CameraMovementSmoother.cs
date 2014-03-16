using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CameraMovementSmoother: MonoBehaviour
{
    //manage both orthographicSize and fieldOfView
    Vector2 zoomCurrent,zoomInit, zoomTo;
    float prog;
    float timeElapsed, timeMax;
    public CameraMovementSmoother()
    {
        prog = 0;
        timeElapsed = 0; timeMax = 5;
    }
    void setNewCameraAt(float ortho, float fieldView)
    {
        zoomCurrent = new Vector2(ortho, fieldView);
        zoomInit = zoomCurrent;
    }
    void Start()
    {
        setNewCameraAt(transform.camera.orthographicSize, transform.camera.fieldOfView);
    }
    void applyCamera(Vector2 zoom)
    {
        camera.orthographicSize = .0001f + zoom.x;
        camera.fieldOfView = .0001f+zoom.y;
    }

    void Update()
    {
        if (prog>=1) return; //no new progression to be made
        timeElapsed += Time.deltaTime;
        prog = timeElapsed / timeMax;
        prog = Math.Min(prog, 1);
        Vector2 zoomMovement = zoomTo - zoomInit;
        zoomCurrent = zoomInit + zoomMovement * prog;
        applyCamera(zoomCurrent);

    }
}