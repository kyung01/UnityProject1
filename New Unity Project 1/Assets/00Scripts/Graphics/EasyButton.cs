using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class EasyButton : EasyGameObject
{
    public int id = 0;
    public delegate void DEL_CLICK(int id);
    public event DEL_CLICK EVENT_CLICK;// = delegate { };
    void Update()
    {
        //Debug.Log("updayting");
        if (InputManager.getInputCount() == 0){return;}
        RaycastHit hit;

        var ray = Camera.main.ScreenPointToRay(InputManager.getInputAt(0));
        if (Physics.Raycast(ray, out hit)) { DrawHelper.drawLine(ray.origin, ray.origin + ray.direction * 10.0f, 1,new Vector4(1,0,0,1) );
            Debug.Log(ray + " and " + hit.point + " " + hit.collider);
            //transform.localScale *= 10.0f;
            Debug.Log("click at " + id); 
            //EVENT_CLICK(id); 
        }
    }
}