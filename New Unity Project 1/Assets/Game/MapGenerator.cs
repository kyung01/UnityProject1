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
    public static MapData getRandom(int w, int h)
    {
        var d = new MapData(w, h);
        foreach (List<int> e in d.matchH)
            helperRandomMatch(e,3, 6, 2);
        foreach (List<int> e in d.matchV)
            helperRandomMatch(e,3, 7, 3);
        return d;
    }
}
