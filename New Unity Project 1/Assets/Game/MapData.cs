using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MapData
{
    public Vector2 size;
    public List<List<int>> matchH, matchV;

    public MapData(int w, int h) {
        size = new Vector2(w, h);
        matchH = new List<List<int>>();
        matchV = new List<List<int>>();
        for (int i = 0; i < h; i++) matchH.Add(new List<int>());
        for (int i = 0; i < w; i++) matchV.Add(new List<int>());
    }
    public static int helperGetMaxNumItems(List<List<int>> list)
    {
        int max = 0;
        foreach (var v in list)
            if (v.Count > max) max = v.Count;
        return max;
    }
    public Vector2 getEdgeSize()
    {
        Vector2 size= new Vector2();
        List<List<int>>[] arr = { matchH, matchV };
        for (int i = 0; i < 2; i++)
        {
            foreach (List<int> v in arr[i])
                if (v.Count > size[i]) size[i] = v.Count;

        }
        return size;
    }
}

