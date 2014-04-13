using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MapGenerator
{
    static void helperRandomMatch(List<int> l,int totalMax, int maxSlot, int maxEach)
    {
        int total = 0;
        for (int slot = 0; slot < maxSlot; slot++)
        {
            int num = UnityEngine.Random.Range(0, maxEach);
            total += num;
            if (total > totalMax)
            {
                num = totalMax - (total - num); ;
                if (num < 0) break;
                
            }
            if (num != 0)l.Add(num);
        }
    }
    public static MapData getBoard_RandomContent(int w, int h)
    {
        var d = new MapData(w, h);
        d.setRandomMatches(10);
        d.doUpdateMatches();
        return d;
    }
}
