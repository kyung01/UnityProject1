using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;
class InputManager
{
    public static int getInputCount(bool mouse = true)
    {
        int touchCount = Input.touchCount,
            touchNot = 0;
        if (touchCount == 0 && mouse) return Input.GetMouseButton(0) ? 1 : 0;
        for (int i = 0; i < touchCount; i++)
        {
            var phase = Input.GetTouch(i).phase;
            if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled) touchNot++;
        }
        touchCount -= touchNot;
        return touchCount;
    }
    public static Vector3 getInputAt(int n = 0)
    {
        int countTouch = Input.touchCount;
        if (countTouch != 0 && n <= countTouch)
        {
            return Input.GetTouch(n).position.XYZ();
        }
        return Input.mousePosition;
    }
    public static Vector3 helperGetRatio(Vector3 v) {
        return new Vector3(v.x/ Screen.width, v.y/ Screen.height, v.z); 
    }
}