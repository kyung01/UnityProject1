using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;


public class Board : EasyGameObject
{
    public GameObject PREFAB_TileEdge;
    public GameObject PREFAB_TileContent;

    ControllerScale controller; 
    MapData map;
    TileEdge[,] edgeHorz, edgeVert;
    TileContent[,] tiles;
    
    //helpers
    GameObject helperInstantiateTile(GameObject instance, Vector3 at, Vector3 size, int x, int y)
    {
        var p = at + size.mult(new Vector3(x, -y, 0));
        var obj = Instantiate(instance, p, transform.rotation) as GameObject;
        obj.transform.localScale = size;
        obj.transform.parent = transform;

        return obj;
    }
	
    //Events
    void EVENT_LineMatch(int x, int y, bool state)
    {
        Vector4 color = state ? new Vector4(.8f, 1, .8f, 1f) : new Vector4(1, 1, 1, .5f);
        if (y >= 0)
            for (int i = 0; i < edgeHorz.GetLength(0); i++)
                edgeHorz[i, y].setColor(color.x, color.y, color.z, color.w);
        if (x >= 0)
            for (int i = 0; i < edgeVert.GetLength(1); i++)
                edgeVert[x, i].setColor(color.x, color.y, color.z, color.w);
    }
    void EVENT_TileChanged(int x, int y, int state)
    {
        tiles[x, y].displayState(state);
    }
    void EVENT_ClickContent(Tile t)
    {
        Debug.Log("click " + t.index[0] + " " + t.index[1]);
        map.doMark(t.index[0], t.index[1]);
        if (map.isGameOver()) doGameOver();
    }
	// Use this for initialization
    void Start()
    {
        
        renderer.enabled = false;
        map = MapGenerator.getRandom(5, 5);
        controller=  transform.gameObject.AddComponent(typeof(ControllerScale)) as ControllerScale;
        var edge = map.getEdgeSize();
        initEmptyBoard(edge, map.size);


        initEdge(edgeHorz, map.matchH);
        initEdge(edgeVert, map.matchV, false);
        link();
        map.doUpdateMatches();
    }
    
    
    void link()
    {
        linkStaticEvents();
        for (int x = 0; x < tiles.GetLength(0); x++) for (int y = 0; y < tiles.GetLength(1); y++)
        {
            tiles[x,y].index = new int[]{x,y};
            tiles[x, y].EVENT_Click = this.EVENT_ClickContent;

        }
    }
    void linkStaticEvents()
    {
        MapData.EVENT_TileChanged += EVENT_TileChanged;
        MapData.EVENT_LineMatch += EVENT_LineMatch;
    }
    
    void initEmptyBoard(Vector2 sizeEdge, Vector2 sizeContent)
    {
        Vector2 total = sizeEdge + sizeContent;
        Vector3 sizeCell = transform.localScale.divide(new Vector3(total.x, total.y, 1));
        if (sizeCell.x < sizeCell.y) sizeCell.y = sizeCell.x;
        else sizeCell.x = sizeCell.y;
        Vector3 p = getPosTopLeft() + sizeCell.mult(new Vector3(1f, -1f, 0)) * .5f,
                posContent =p + sizeCell.XY().mult(1.0f, -1.0f).mult(sizeEdge).XYZ(),
                posLineHorz = posContent + sizeCell.mult(-.5f,.5f,0 ) + new Vector3(0,0,-.01f);
        //for (int i = 0; i < 10; i++) 
        //    DrawHelper.drawLine(p + new Vector3(5,5,0), p + sizeCell.mult(1, 1 + i, 0), .1f);
        transform.localScale = new Vector3(1, 1, 1); //prevent rescaling

        initTileContent<TileEdge>(PREFAB_TileEdge, ref edgeVert, p + sizeCell.mult(sizeEdge.x, 0, 0), sizeCell, (int)sizeContent.x, (int)sizeEdge.y);
        initTileContent<TileEdge>(PREFAB_TileEdge, ref edgeHorz, p + sizeCell.mult(0.0f, -sizeEdge.y, 0), sizeCell, (int)sizeEdge.x, (int)sizeContent.y);
        initTileContent<TileContent>(PREFAB_TileContent, ref tiles, posContent, sizeCell, (int)sizeContent.x, (int)sizeContent.y);

        initSeparatingLines(posLineHorz, sizeCell.mult(sizeContent.XYZ(1.0f)),sizeCell, 
            (int)sizeContent.x, (int)sizeContent.y);
    }
    void initEdge(TileEdge[,] tiles, List<List<int>> data, bool isHorizontal = true)
    {
        int _x , _y;
        for (int x = 0; x < data.Count; x++) for (int y = 0; y < data[x].Count; y++)
        {
            if (isHorizontal){_x = y; _y = x;}
            else {_x= x;_y = y;}
            tiles[_x, _y].init();
            tiles[_x, _y].num.displayNum(data[x][y]);
        }

    }
    void initTileContent<T>(GameObject obj, ref T[,] arr, Vector3 p, Vector3 size, int w, int h) where T : class
    {
        arr = new T[w, h];
        for (int x = 0; x < w; x++) for (int y = 0; y < h; y++)
            {
                var e = helperInstantiateTile(obj, p, size, x, y).GetComponent(typeof(T)) as T;
                arr[x, y] = e;
            }
    }
    void initSeparatingLines(Vector3 at, Vector3 size, Vector3 sizeCell, int w, int h)
    {
        for (int y = 0; y <= h; y++)
        {
            var pos = at + sizeCell.mult(0, -y, 0);
            var obj = DrawHelper.drawLine(pos, pos + new Vector3(size.x, 0, 0), .05f);
            obj.transform.parent = transform;
        }
        for (int x = 0; x <= w; x++)
        {
            var pos = at + sizeCell.mult(x, 0, 0);
            var obj = DrawHelper.drawLine(pos, pos - new Vector3(0, size.y, 0), .05f);
            obj.transform.parent = transform;
        }
    }

    void doGameOver()
    {
        transform.position += new Vector3(0, 0, -100);
    }
	// Update is called once per frame
	void Update () {
	
	}
}