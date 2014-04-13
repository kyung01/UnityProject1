using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class ScreenLevel : EasyGameObject
{
    public IcnLevel     PREFAB_IcnStage;
    public GameObject   PREFAB_IcnBar;
    public EasyTextMesh     icnIndex,
                            icnLevel, 
                            icnProgress,
                            icnLevelSub;
    Vector4 rectIcnIndex = new Vector4(.03f,.03f, .13f,.1f),
            rectIcnLevel = new Vector4(.2f, .01f, .8f, .10f),
            rectIcnProgress = new Vector4(.85f, 0, .95f, .1f);
    DataLevel data;

    void EVENT_CLICK_LEVEL(int n)
    {
        Debug.Log("EVENT CLICK LEVEL "+data.name + " " + n);
    }
    void helperInitMapIcns(float ratioY = .1f, float ratioHeight = .9f)
    {
        float CountTotal = 5.0f;
        var posCorner = getPosTopLeft() + transform.localScale.mult(0, -ratioY, 0);
        var size = transform.localScale.mult(1, ratioHeight, 1);
        var posMid = posCorner + size.mult(.5f, -.5f,0);

        var sizeCellHeight = transform.localScale.y * ratioHeight / CountTotal; //.y / CountTotal;
        var posStartFrom = posMid + new Vector3(0, sizeCellHeight * data.collections.Count * .5f, 0);
        posStartFrom += new Vector3(0, sizeCellHeight*-.5f, 0);
        for (int i = 0; i < data.collections.Count; i++)
        {
            var obj = Instantiate(PREFAB_IcnStage, Vector3.zero, Quaternion.identity) as IcnLevel;
            obj.transform.position = posStartFrom + new Vector3(0, -i * sizeCellHeight,0);
            obj.transform.localScale = new Vector3(sizeCellHeight, sizeCellHeight,1); 
            obj.transform.parent = transform;
            obj.display(1+i,data.collections[i].countAll, data.collections[i].countCompleted);
            obj.id = i;
            obj.EVENT_CLICK += EVENT_CLICK_LEVEL;
        }
    }
    void helperSetPosSize(Transform t, Vector4 rect)
    {
        var pos = getPosTopLeft() + transform.localScale.mult( new Vector3(rect.x,-rect.y,0) );
        var size = transform.localScale.mult(new Vector3(rect.z - rect.x, rect.w - rect.y, 1));
        pos += new Vector3(size.x,-size.y,0) * .5f;
        var p = t.parent; t.parent = null;
        t.position= pos;
        t.localScale = size;
        t.parent = p;
    }
    void helperIcnBar(float y)
    {
        var pos =getPosTopLeft() +  transform.localScale.mult(.1f, -y, 0);
        var size = new Vector3(transform.localScale.x * .8f, transform.localScale.x * .8f * .03f, 1);
        pos += new Vector3(size.x, -size.y, 0) * .5f;
        var bar = Instantiate(PREFAB_IcnBar, pos, Quaternion.identity) as GameObject;
        
        bar.transform.localScale = size;
        bar.transform.parent = transform;   
    }
    void Awake()
    {
        data = new DataLevel();
        helperIcnBar(.1f);
        helperSetPosSize(icnIndex.transform, rectIcnIndex);
        helperSetPosSize(icnLevel.transform, rectIcnLevel);
        helperSetPosSize(icnProgress.transform, rectIcnProgress);
        helperInitMapIcns();
        icnIndex.display("#1");
        icnLevel.display(data.name);
        icnProgress.display("" + data.countCompleted + "/" + data.countMaps);
    }
    public void display(int level, int levelSize, int levelCompleted = 0)
    {
        icnLevel.display("" + level);
        icnProgress.display("" + levelCompleted + "/" + levelSize);
    }
    void Update()
    {
    }
}