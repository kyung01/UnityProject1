﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;
class BehaviorSlide : EasyGameObject
{
    const float RATIO_MIN = .7f;
    public GameObject obj;
    public Vector3 distance;

    bool isFirstTouch = true;
    Vector3 ratio,
            ratioFrom;
    int index = 0;
    void SlideHorz(Vector3 dir)
    {
        if (dir.x < 0) index--;
        else if (dir.x > 0) index++;
        else return;
        index = Mathf.Max(index, 0);

    }
    void helperResetPosition(int index = 0, Vector3? ratio = null)
    {
        obj.transform.position = transform.position + (distance * index);
        if (ratio != null) obj.transform.position += distance.mult(ratio.Value);
    }
    Vector3 helperGetRatio()
    {
        Vector3 ratio = InputManager.helperGetRatio(InputManager.getInputAt(0)) - new Vector3(.5f, .5f, 0);
        ratio = ratio.divide(new Vector3(.5f, .5f, 1));
        return ratio;
    }
    void Update()
    {

        if (InputManager.getInputCount() < 1)
        {
            isFirstTouch = true;
            //Debug.Log(ratio.magnitude + " ");
            if (ratio.magnitude < RATIO_MIN)
            {
                helperResetPosition(index);
            }
            else
            {
                SlideHorz(ratio);
                ratio = new Vector3();
            }
            return;
        }
        if (isFirstTouch)
        {
            ratioFrom = helperGetRatio();
            isFirstTouch = false;
            return;
        }
        ratio = helperGetRatio() - ratioFrom;
        helperResetPosition(index, ratio);
        //Debug.Log(index + " "+  ratio);
    }
 
}