using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class PinnedObject : EasyGameObject
{
    const float cof = 7.1f;
    public GameObject pin;

    public void Update()
    {
        Vector3 dis = pin.transform.position - transform.position;
        if (dis.magnitude < .1) { transform.position = pin.transform.position;  return; }
        Vector3 velo = dis * cof;
        if (velo.sqrMagnitude < 1.0f) velo = dis.normalized * 2.0f;
        transform.position += velo * Time.deltaTime;
    }
}