using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SwingApplier : EasyGameObject
{
    public List<GameObject> swingers;
    public void Start(){
        float swing = 5;
        foreach (var s in swingers) s.transform.rotation = Quaternion.Euler(0, 0, (swing+=5));
    }
}