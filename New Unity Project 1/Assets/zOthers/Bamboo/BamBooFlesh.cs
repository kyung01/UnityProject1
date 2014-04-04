#pragma warning disable 0649

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;

class BamBooFlesh : EasyGameObject
{
    static float CHANCE_LEAF_BRANCH = 30;
    public float bodyThickness = .3f, branchThikness = .3f,leafThickness =.3f;
    public GameObject bonesBody, bonesBranch,bonesLeaf, 
        fleshBody,fleshBranch,fleshLeaves;
    public List<Material> materBodies, materBranch, materLeaf;

    void Start()
    {
        initBodies(fleshBody.transform);
        initBranches(fleshBranch.transform);
        initLeaves(fleshLeaves.transform);
        initLeavesBranchRandom(fleshLeaves.transform);
        setLayersRecursively();
        HelperSetLayersRecursively(fleshLeaves.transform, gameObject.layer);
    }
    void helperGetVertices(GameObject obj, Vector3 from, Vector3 to, float width)
    {
        var dis = to - from;
        var mid = from + dis * .5f;
        var fromRelative = from - mid;
        var toRelative = to - mid;


        obj.AddComponent <MeshFilter>();
        obj.AddComponent <MeshRenderer>();
        var mesh = (obj.GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
        mesh.Clear();
        mesh.vertices = new Vector3[] { 
            fromRelative - new Vector3(width*.5f,0,0), 
            fromRelative + new Vector3(width*.5f,0,0), 
            toRelative - new Vector3(width*.5f,0,0), 
            toRelative + new Vector3(width*.5f,0,0)};
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 3, 1, 0, 2, 3, 0, 1, 3, 0, 3, 2 };
        obj.transform.position = mid;

    }
    GameObject helperGetSquare(GameObject obj, float size)
    {
        Vector3 from = new Vector3(), to = new Vector3(0,size,0);
        var dis = to - from;
        var mid = from + dis * .5f;
        var fromRelative = from - mid;
        var toRelative = to - mid;


        obj.AddComponent("MeshFilter");
        obj.AddComponent("MeshRenderer");
        var mesh = (obj.GetComponent(typeof(MeshFilter)) as MeshFilter).mesh;
        mesh.Clear();
        mesh.vertices = new Vector3[] { 
            fromRelative - new Vector3(size*.5f,0,0), 
            fromRelative + new Vector3(size*.5f,0,0), 
            toRelative - new Vector3(size*.5f,0,0), 
            toRelative + new Vector3(size*.5f,0,0)};
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 3, 1, 0, 2, 3, 0, 1, 3, 0, 3, 2 };
        obj.transform.position = mid;
        return obj;
    }
    
    void initBodies(Transform group)
    {
        var bones = bonesBody.transform;
        for (int i = 0; i < bones.childCount - 1; i++)
        {
            var obj = new GameObject();
            helperGetVertices(obj, bones.GetChild(i).transform.position, 
                bones.GetChild(i+1).transform.position, bodyThickness);
            obj.renderer.material = materBodies[0];
            obj.transform.parent = group;
        }
    }
    void helperRecursivelyBranch(Transform parent, Transform boneFrom)
    {
        foreach (Transform to in boneFrom)
        {
            var obj = new GameObject();
            float mag = Vector3.Distance(to.position , boneFrom.position);
            helperGetVertices(obj, boneFrom.position, to.position, mag*branchThikness);
            obj.renderer.material = materBranch[0];
            helperRecursivelyBranch(obj.transform, to);
            obj.transform.parent = parent;
        }

    }
    void initBranches(Transform group)
    {
        foreach (Transform bone in bonesBranch.transform)
        {
            helperRecursivelyBranch(fleshBranch.transform, bone);
        }
    }
    void helperInitLeafTriple(Transform group, GameObject joint, Vector3 at, Vector3 dir, float thickness)
    {
        GameObject leaves = new GameObject();
        leaves.transform.parent = group;
        Vector3[] dirs = new Vector3[] { 
            (Vector3.Cross(Vector3.up, dir) + dir).normalized, 
            (Vector3.Cross(dir, Vector3.up) + dir).normalized, dir};
        for (int i = 0; i < dirs.Length; i++)
        {
            GameObject leaf = new GameObject();
            helperGetSquare(leaf, thickness).transform.position = at + dirs[i] * thickness * .5f;
            leaf.renderer.material = materLeaf[Random.Range(0, materLeaf.Count)];
            leaf.AddComponent<JointBasic>().init(joint);
            leaf.transform.parent = leaves.transform;
        }
    }
    void helperInitLeafDouble(Transform group, GameObject joint, Vector3 at, Vector3 dir, float thickness)
    {
        GameObject leaves = new GameObject();
        leaves.transform.parent = group;
        Vector3[] dirs = new Vector3[] { 
            (Vector3.Cross(Vector3.up, dir) + dir).normalized, 
            (Vector3.Cross(dir, Vector3.up) + dir).normalized};
        for (int i = 0; i < dirs.Length; i++)
        {
            GameObject leaf = new GameObject();
            helperGetSquare(leaf, thickness).transform.position = at + dirs[i] * thickness * .5f;
            leaf.renderer.material = materLeaf[Random.Range(0, materLeaf.Count)];
            leaf.AddComponent<JointBasic>().init(joint);
            leaf.transform.parent = leaves.transform;
        }
    }
    void helperInitLeafBrachRecursively(Transform group, Transform branch)
    {
        bool isLeafed = false;
        foreach (Transform v in branch)
        {
            helperInitLeafBrachRecursively(group, v);
            if (isLeafed || Random.Range(0, 100) > CHANCE_LEAF_BRANCH)continue;
            isLeafed = true;
            Vector3 dis = v.position - branch.position,
                    dir = dis.normalized;
            float   dirLeaf = Random.Range(-1, 2),
                    magLeaf = dis.magnitude * leafThickness * .7f;
            if (dirLeaf == 0 || magLeaf < .1f) continue;
            helperInitLeafDouble(group, branch.gameObject, branch.position, dir * dirLeaf, magLeaf);
        }
    }
    void initLeavesBranchRandom(Transform group)
    {
        foreach (Transform v in bonesBranch.transform)
            helperInitLeafBrachRecursively(group, v);
       
    }
    void initLeaves(Transform group)
    {
        foreach (Transform child in bonesLeaf.transform)
        {
            if (child.childCount == 0) { Debug.Log("BamBooFlesh initLeaves error"); continue; }
            Transform from = child.transform, to = child.GetChild(0).transform;
            Vector3 dis = to.position - from.position,
                    dir = dis.normalized;
            float dirLeaf = Random.Range(-1, 2);
            if (dirLeaf == 0) continue;
            float mag = dis.magnitude * leafThickness;
            if (mag < .01f) return;

            helperInitLeafTriple(group, to.gameObject, to.transform.position, (dir + new Vector3(0, dirLeaf, 0)).normalized, mag);
        }
    }
}