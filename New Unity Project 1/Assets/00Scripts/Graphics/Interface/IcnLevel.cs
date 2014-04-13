using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class IcnLevel : EasyButton
{
    
    public EasyTextMesh textLevel, textCount;
    public void display(int level, int countMaps,int countCompleted)
    {
        textLevel.display(""+level);
        textCount.display(""+countCompleted+"/" + countMaps);

    }
}