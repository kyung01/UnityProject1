using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MapData
{
    public delegate void del00(int x, int y, int state);
    public delegate void del01(int x, int y, bool matched);
    public static  del00 EVENT_TileChanged;
    public static  del01 EVENT_LineMatch;
    public Vector2 size;
    public List<List<int>> matchH, matchV;
    public int[,] data;

    public MapData(int w, int h) {
        data = new int[w, h];
        size = new Vector2(w, h);
        initMatches(data);
    }
    
    void initMatches(int[,] data)
    {
        matchH = new List<List<int>>();
        matchV = new List<List<int>>();
        for (int y = 0; y < size.y; y++)
        {
            List<int> line = new List<int>();
            for (int x = 0; x < size.x; x++) line.Add(data[x, y]);
            matchH.Add(helperGetPattern(line));
        }
        for (int x = 0; x < size.x; x++)
        {
            List<int> line = new List<int>();
            for (int y = 0; y < size.y; y++) line.Add(data[x, y]);
            matchV.Add(helperGetPattern(line));
        }

    }
    List<int> helperGetPattern(List<int> l)
    {
        List<int> pattern = new List<int>();
        int count = 0; 
        for(int i = 0 ; i <l.Count;i++){
            if (l[i] == 1) count++;
            if (l[i] != 1  || i == l.Count-1 ){
                if (count != 0) pattern.Add(count);
                count = 0;
            }
        }
        return pattern;
    }
    bool helperCheckH(int y)
    {
        var solution = matchH[y];
        List<int> line = new List<int>();
        for (int x = 0; x < size.x; x++) line.Add(data[x, y]);
        var answer = helperGetPattern(line);
        if (answer.Count == solution.Count)
        {
            for (int i = 0; i < solution.Count; i++)
                if (answer[i] != solution[i]) return false;
            return true;
        }
        return false;
    }
    bool helperCheckV(int x)
    {
        var solution = matchV[x];
        List<int> line = new List<int>();
        for (int y = 0; y < size.y; y++) line.Add(data[x, y]);
       
        var answer = helperGetPattern(line);
        if (solution.Count == answer.Count)
        {
            for (int i = 0; i < solution.Count; i++)
                if (answer[i] != solution[i]) return false;
            return true;
        }   
        return false;
    }
    public Vector2 getEdgeSize()
    {
        Vector2 size = new Vector2();
        List<List<int>>[] arr = { matchH, matchV };
        for (int i = 0; i < 2; i++)
            foreach (List<int> v in arr[i])
                if (v.Count > size[i]) size[i] = v.Count;
        return size;
    }
    public void setRandomMatches(int n = 10)
    {
        for (int i = 0; i < n; i++)
            data[(int)Random.Range(0, size.x - 1), (int)Random.Range(0, size.y - 1)] = 1;
        initMatches(data);
        data = new int[(int)size.x, (int)size.y];
    }
    public void doMark(int x, int y)
    {
        data[x, y] = (data[x, y] + 1) % 2;
        bool[] result = { helperCheckV(x), helperCheckH(y) };
        if (EVENT_TileChanged != null) EVENT_TileChanged(x, y, data[x, y]);
        if (EVENT_LineMatch != null)
        {
            EVENT_LineMatch(result[0] ?  x:-1, result[1] ?  y:-1, true);
            EVENT_LineMatch(!result[0] ? x:-1 , !result[1] ? y:-1, false);
        }
    }
    //Please do not right "tricky-cool" code.
    //keep it simple kyung.
    //I don't like it. It's just too long I can shrink them
    //DONT SHRINK THEM.  readability > pretty code
    public bool doUpdateMatches()
    {
        bool isMatched = false;
        for (int x = 0; x < size.x; x++)
        {
            bool result = helperCheckV(x);
            isMatched = isMatched && result;
            if (result && EVENT_LineMatch != null)
                EVENT_LineMatch(x, -1, result);
        }
        for (int y = 0; y < size.y; y++)
        {
            bool result = helperCheckH(y);
            isMatched = isMatched && result;
            if (result && EVENT_LineMatch != null)
                EVENT_LineMatch(-1, y, result);
        }
        return isMatched;
    }
    
}

