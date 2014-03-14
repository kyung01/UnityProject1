using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;


public class Board : EasyGameObject
{
    public GameObject PREFAB_TileEdge;
    public GameObject PREFAB_TileContent;

    List<Tile> tiles = new List<Tile>();
    TileEdge[,] EdgeHorz, EdgeVert;
    Tile[,] tile2D;
	// Use this for initialization
    void Start()
    {
        renderer.enabled = false;
        var map = MapGenerator.getRandom(5, 5);
        var edge = map.getEdgeSize();
        initEmptyBoard(edge, map.size);
        //init(map.size.x, map.size.y);

        initEdge(EdgeHorz, map.matchH);
        initEdge(EdgeVert, map.matchV,false);
        //initEdge(EdgeVert, map.matchV);
		//(arr [0] as GameObject).transform.position = (arr [0] as GameObject).transform.position - new Vector3 (-1f, 0, 0);
	}
    void initEmptyBoard(Vector2 sizeEdge, Vector2 sizeContent)
    {
        Vector2 total = sizeEdge + sizeContent;
        Debug.Log(sizeEdge);
        Debug.Log(sizeContent);
        Vector3 sizeCell = transform.localScale.divide(new Vector3(total.x, total.y, 1));
        if (sizeCell.x < sizeCell.y) sizeCell.y = sizeCell.x;
        else sizeCell.x = sizeCell.y;
        Vector3 p = getPosTopLeft() + sizeCell.mult(new Vector3(1f, -1f, 0)) * .5f,
                posContent =p + sizeCell.XY().mult(1.0f, -1.0f).mult(sizeEdge).XYZ(),
                posLineHorz = posContent + sizeCell.mult(-.5f,.5f,0 ) + new Vector3(0,0,-.2f);
        //for (int i = 0; i < 10; i++) 
        //    DrawHelper.drawLine(p + new Vector3(5,5,0), p + sizeCell.mult(1, 1 + i, 0), .1f);
        transform.localScale = new Vector3(1, 1, 1); //prevent rescaling

        initTileContent<TileEdge>(PREFAB_TileEdge, ref EdgeVert, p + sizeCell.mult(sizeEdge.x, 0, 0), sizeCell, (int)sizeContent.x, (int)sizeEdge.y);
        initTileContent<TileEdge>(PREFAB_TileEdge, ref EdgeHorz, p + sizeCell.mult(0.0f, -sizeEdge.y, 0), sizeCell, (int)sizeEdge.x, (int)sizeContent.y);
        initTileContent<Tile>(PREFAB_TileContent, ref tile2D, posContent, sizeCell, (int)sizeContent.x, (int)sizeContent.y);

        initSeparatingLines(posLineHorz, sizeCell.mult(sizeContent.XYZ(1.0f)),sizeCell, 
            (int)sizeContent.x, (int)sizeContent.y);
        for (int x = 0; x < sizeContent.x; x++) for (int y = 0; y < sizeContent.y; y++)
            {
               // tile2D[x, y].setColor(x*.1f, y*.1f, 0);
            }
    }
    void initSeparatingLines(Vector3 at, Vector3 size, Vector3 sizeCell, int w , int h)
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
    Tile helperInstantiateTile(GameObject instance, Vector3 at, Vector3 size, int x, int y)
    {
		var p =  at + size.mult(  new Vector3( x,- y, 0) ) ;
        var obj = Instantiate(instance, p, transform.rotation) as GameObject;
		obj.transform.localScale = size;
		obj.transform.parent = transform;
        
		return obj.GetComponent("Tile") as Tile;
	}
	void initTiles(Vector3 at, Vector3 size,float w, float h){
	}
	// Update is called once per frame
	void Update () {
	
	}
}
/**
 * 
		List<Tile> t;
        tile2D = new List<List<Tile>> ();
        //boundry; edge numbers.
		for (int x = 0; x<w; x++) {
            tile2D.Add(new List<Tile>());
            tile2D[x].Add(helperInstantiateTile(PREFAB_TileEdge,at, size, x, 0));
		}
        t = tile2D[0];
        for (int y = 1; y < h; y++) t.Add(helperInstantiateTile(PREFAB_TileEdge,at, size, 0, y));
        //board
		for (int x = 1; x< w; x++) {
            t = tile2D[x];
            for (int y = 1; y < h; y++) 
                t.Add(helperInstantiateTile(PREFAB_TileContent, at, size, x, y));
		}
**/