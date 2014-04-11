using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;


public class Board : EasyGameObject
{
    int         COUNT_HORZ ,
                COUNT_VERT;
    public GameObject PREFAB_TileEdge;
    public GameObject PREFAB_TileContent;
    GameObject 
                GRP_tileEdges,
                GRP_tileContent,
                GRP_outlinesHorz,
                GRP_outlinesVert;
    GameObject[] GRP_all;
    public Material materialBoard;

    Vector3 sizeBoard,
            sizeCell;
    MapData map;
    List<TileEdge>[,] edgeHorz, edgeVert;
    TileContent[,] tiles;
    ControllerBoard controller; 
    //colors
    Vector4 colorLineLight = new Vector4(0, 0, 0, 1.0f);

    //helpers
    GameObject helperInstantiateTile(GameObject instance, Vector3 at, Vector3 size, Transform parent = null)
    {
        var obj = Instantiate(instance, at, transform.rotation) as GameObject;
        obj.transform.localScale = size;
        obj.transform.parent = parent;
        return obj;
    }
    Ray helperWrapAsRay(Vector3 screenPos)
    {
        var pos = Camera.main.transform.position;
        return new Ray(
            pos,
            Camera.main.ScreenToWorldPoint(screenPos + new Vector3(0, 0, 1)) - pos);
    }
	
    //Events
    void EVENT_LineMatch(int x, int y, bool state)
    {
        //  Vector4 color = state ? new Vector4(.8f, 1, .8f, 1f) : new Vector4(1, 1, 1, .5f);
        //  if (y >= 0)
        //      for (int i = 0; i < edgeHorz.GetLength(0); i++)
        //          edgeHorz[i, y].setColor(color.x, color.y, color.z, color.w);
        //  if (x >= 0)
        //      for (int i = 0; i < edgeVert.GetLength(1); i++)
        //          edgeVert[x, i].setColor(color.x, color.y, color.z, color.w);
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
    void EVENT_Click(Vector3 input)
    {
        Debug.Log("debug EVENT BOARD FLIP AT ");
        var ray = helperWrapAsRay(input);
        RaycastHit hitInfo;
        for (int x = 0; x < tiles.GetLength(0); x++)
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].gameObject.collider.Raycast(ray, out hitInfo, 100))
                {
                    map.doMark(x, y);
                    return;
                }
            }
    }

    void initSizes(Vector2 countEdge, Vector2 countContent, bool isExtend = false)
    {
        Vector2 total = countEdge + countContent;
        if (isExtend) total += countEdge;
        sizeCell = transform.localScale.divide(new Vector3(total.x, total.y, 1));
        if (sizeCell.x < sizeCell.y) sizeCell.y = sizeCell.x;
        else sizeCell.x = sizeCell.y;

        sizeBoard = sizeCell.mult(total.x, total.y, 1);
    }
    void initGameObjects()
    {
        GRP_tileEdges = new GameObject("GRP_tileEdges");
        GRP_tileContent = new GameObject("GRP_tileContent");
        GRP_outlinesHorz = new GameObject("GRP_outlinesHorz");
        GRP_outlinesVert = new GameObject("GRP_outlinesVert");
        GRP_all = new GameObject[] { GRP_tileEdges, GRP_tileContent, GRP_outlinesHorz, GRP_outlinesVert };
        foreach (var o in GRP_all)
        {
            o.transform.parent = transform;
            o.transform.position = transform.position;
        }
    }
    void helperInitializeArray(List<TileEdge>[,] arr)
    {
        for(int i = 0 ; i < arr.GetLength(0);i++)
            for (int j = 0; j < arr.GetLength(1); j++)
                arr[i, j] = new List<TileEdge>();
    } 
    void initArrays(Vector2 countEdge, Vector2 countContent)
    {
        edgeHorz = new List<TileEdge>[(int)countContent.y, (int)countEdge.x];
        edgeVert = new List<TileEdge>[(int)countContent.x, (int)countEdge.y];
        helperInitializeArray(edgeHorz);
        helperInitializeArray(edgeVert);
    }
	//Methods
    void Awake()
    {
        COUNT_HORZ = Random.Range(3, 6);
        COUNT_VERT = Random.Range(3, 6);
        renderer.enabled = false;
        linkStaticEvents();
        initGameObjects();

        map = MapGenerator.getBoard_RandomContent(COUNT_HORZ,COUNT_VERT);
        Vector2 countEdge = map.getEdgeSize(),
                countContent = new Vector2(COUNT_HORZ, COUNT_VERT);
        initArrays(countEdge, countContent);

        bool isExtend = true;
        initSizes(countEdge, countContent, isExtend);
        initInterfaces(getPosTopLeft() + sizeCell.mult(countEdge), sizeCell.mult(countContent));
        initTiles(countEdge, countContent, sizeCell, isExtend);
        initOutlines(countEdge, countContent, sizeCell, isExtend);

        foreach (GameObject t in GRP_all) centerIt(t.transform, countEdge, countContent, sizeCell, isExtend);

        displayNumbers(edgeHorz, map.matchH);
        displayNumbers(edgeVert, map.matchV);
        
        link();
        map.doUpdateMatches();
        setLayersRecursively();
    }

    void centerIt(Transform obj, Vector2 countEdge, Vector2 countContent, Vector2 sizeCell, bool isExtend)
    {
        Vector2 count = countEdge + countContent;
        if (isExtend) count += countEdge;
        Vector3 space = transform.localScale - count.mult(sizeCell).XYZ();
        space = space.mult(1, -1, 0) * .5f;
        obj.position += space;
    }
    void linkStaticEvents()
    {
        MapData.EVENT_TileChanged += EVENT_TileChanged;
        MapData.EVENT_LineMatch += EVENT_LineMatch;
    }
   
    
    void link()
    {
        for (int x = 0; x < tiles.GetLength(0); x++) for (int y = 0; y < tiles.GetLength(1); y++)
        {
            tiles[x,y].index = new int[]{x,y};
            //tiles[x, y].EVENT_Click = this.EVENT_ClickContent;
        }
        controller.EVENT_TouchClick += EVENT_Click;
    }
   
    
    void initInterfaces(Vector3 from, Vector3 size)
    {
        var at =from+ new Vector3(size.x,-size.y,0) * .5f;
        var plane = DrawHelper.drawRect(at, size);
        plane.renderer.enabled = false;
        plane.transform.parent = transform;
        plane.transform.position += new Vector3(0,0,-.2f);
        (plane.GetComponent(typeof(EasyGameObject)) as EasyGameObject).setColor(1.0f, .8f, .5f, .5f);
        controller = (transform.gameObject.AddComponent(typeof(ControllerBoard)) as ControllerBoard)
            .init(this.transform.gameObject, plane);
    }
    void initOutlines(Vector2 countEdge, Vector2 countContent,Vector3 sizeCell,bool isExtend = false)
    {
        //unscaled
        Vector2 countLine = countEdge + countContent;
        if (isExtend) countLine += countEdge;
        Vector3 sizeAll = countLine.XYZ().mult(sizeCell);
        initSeparatingLine_Horizontal(getPosTopLeft(), sizeAll, (int)countLine.y, GRP_outlinesHorz.transform,(int)countEdge.y, 5);
        initSeparatingLine_Vertical(getPosTopLeft(), sizeAll, (int)countLine.x, GRP_outlinesVert.transform, (int)countEdge.x, 5);
    }
    void initTiles(Vector2 countEdge, Vector2 countContent, Vector3 sizeCell,bool isExtend = false)
    {
        Vector3 p = getPosTopLeft(),
                posContent = p + sizeCell.XY().mult(1.0f, -1.0f).mult(countEdge.x, countEdge.y).XYZ();

        tiles = new TileContent[(int)countContent.x, (int)countContent.y];
        initMultipleObjects<TileContent>(PREFAB_TileContent, posContent, sizeCell, (int)countContent.x, (int)countContent.y,
            GRP_tileContent.transform,tiles);


        helperInitMultipleObjs<TileEdge>(PREFAB_TileEdge, p + sizeCell.mult(countEdge.x, 0, 0), sizeCell, (int)countContent.x, (int)countEdge.y,
            GRP_tileEdges.transform, false, edgeVert);
        helperInitMultipleObjs<TileEdge>(PREFAB_TileEdge, p + sizeCell.mult(0.0f, -countEdge.y, 0), sizeCell, (int)countEdge.x, (int)countContent.y,
             GRP_tileEdges.transform, true, edgeHorz);


        
        if (!isExtend) return;

        helperInitMultipleObjs<TileEdge>(PREFAB_TileEdge, p + sizeCell.mult(countEdge.x, -(countEdge.y + countContent.y), 0), sizeCell, (int)countContent.x, (int)countEdge.y,
            GRP_tileEdges.transform, false, edgeVert);
        helperInitMultipleObjs<TileEdge>(PREFAB_TileEdge, p + sizeCell.mult(countEdge.x+ countContent.x, -countEdge.y, 0), sizeCell, (int)countEdge.x, (int)countContent.y,
             GRP_tileEdges.transform, true, edgeHorz);
    }
    void displayNumbers(List<TileEdge>[,] tiles, List<List<int>> data)
    {
        Debug.Log(tiles.GetLength(0) + " " + tiles.GetLength(1));

        for (int i = 0; i < data.Count; i++) for (int j = 0; j < data[i].Count; j++)
        {
            var line = tiles[i, j];
            foreach (var l in line) l.display(data[i][j]);
        }
    }
    void helperInitMultipleObjs<T>(GameObject obj, Vector3 from, Vector3 size, int w, int h,Transform parent = null, 
        bool isFlipped = false, List<T>[,] arr = null) where T : class
    {
        from += new Vector3(size.x, -size.y, 0) * .5f;
        for (int x = 0; x < w; x++) for (int y = 0; y < h; y++)
            {
                var pos = from + size.mult(x, -y, 0);
                var gObj = helperInstantiateTile(obj, pos, size, transform);
                gObj.transform.parent = parent;
                var e = gObj.GetComponent(typeof(T)) as T;
                if (arr != null)
                {
                    if (isFlipped) arr[y, x].Add(e);
                    else arr[x, y].Add(e);
                }
            }
    }
    void initMultipleObjects<T>(GameObject obj, Vector3 from, Vector3 size, int w, int h, Transform parent = null,T[,] arr = null) where T : class
    {
        from += new Vector3(size.x, -size.y, 0) * .5f;
        for (int x = 0; x < w; x++) for (int y = 0; y < h; y++)
            {
                var pos = from + size.mult(x, -y, 0);
                var gObj = helperInstantiateTile(obj, pos, size, transform);
                gObj.transform.parent = parent;
                var e = gObj.GetComponent(typeof(T)) as T;
                if(arr != null )arr[x, y] = e;
            }
    }
    void initSeparatingLine_Horizontal(Vector3 from, Vector3 size, int count, Transform parent, int thickStart = 9999,int thickStep=9999)
    {
        for (int i = 0; i <= count; i++)
        {
            float thickness = .05f;
            if ((i - thickStart) % thickStep == 0 || i == count - thickStart) thickness *=2.0f;
            Vector4 color =colorLineLight;
            var pos = from + sizeCell.mult(0, -i, 0);
            var posTo = pos + new Vector3(size.x, 0, 0);
            var obj = DrawHelper.drawLine(pos, posTo, thickness, color);
            obj.transform.parent = parent;
        }
    }
    void initSeparatingLine_Vertical(Vector3 from, Vector3 size, int count, Transform parent, int thickStart = 9999, int thickStep = 9999)
    {
        for (int i = 0; i <= count; i++)
        {
            float thickness = .05f;
            if ((i - thickStart) % thickStep == 0 || i == count - thickStart) thickness *= 2.0f;
            Vector4 color = colorLineLight;
            var pos = from + sizeCell.mult(i, 0, 0);
            var obj = DrawHelper.drawLine(pos, pos - new Vector3(0, size.y, 0), .05f, color);
            obj.transform.parent = parent;
        }
    }
    void initSeparatingLines(Vector3 at, Vector3 size, Vector3 sizeCell, int w, int h, Transform grpX, Transform grpY)
    {

        for (int x = 0; x <= w; x++)
        {
            var pos = at + sizeCell.mult(x, 0, 0);
            var obj = DrawHelper.drawLine(pos, pos - new Vector3(0, size.y, 0), .05f);
            obj.transform.parent = grpX;
        }
        for (int y = 0; y <= h; y++)
        {
            var pos = at + sizeCell.mult(0, -y, 0);
            var obj = DrawHelper.drawLine(pos, pos + new Vector3(size.x, 0, 0), .05f);
            obj.transform.parent = grpY;
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