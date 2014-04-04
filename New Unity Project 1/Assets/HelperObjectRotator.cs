using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class HelperObjectRotator
{
    public static GameObject DD_AlignY(GameObject obj, Vector3 from, Vector3 to)
    {
        var dis = to - from;
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, dis.magnitude, obj.transform.localScale.z);
        var mid = from + dis * .5f;
        obj.transform.position = mid;
        obj.transform.rotation = Quaternion.FromToRotation(new Vector3(0,1,0), to-from);
        //obj.transform.rotation = Quaternion.FromToRotation( )
        return obj;
    }
}