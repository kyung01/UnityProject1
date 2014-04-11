using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class IcnLevel : EasyGameObject
{
    EasyTextMesh icnLevel,icnProgress;
    public void display(int level, int levelSize, int levelCompleted = 0)
    {
        icnLevel.display("" + level);
        icnProgress.display("" + levelCompleted + " / " + levelSize);
    }
}