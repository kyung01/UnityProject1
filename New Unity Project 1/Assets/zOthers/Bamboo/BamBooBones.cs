#pragma warning disable 0649

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;

public class BamBooBones : MonoBehaviour
{
    public float levelMax = 5, levelHeight = 1,levelWidth = .3f, levelAngle = 90, levelAngleVariation = 1;
    public GameObject bonesBody, bonesBranch,bonesLeaves;
   
    List<Vector3> levels = new List<Vector3>();

    List<Vector3> initLevels()
    {
        List<Vector3> levels = new List<Vector3>();
        float angle = levelAngle * .0174f;
        levels.Add(transform.position);
        for (int i = 0; i < levelMax; i++)
        {
            float a = angle + Random.Range(-levelAngleVariation, levelAngleVariation) * .0174f;
            Vector3 dirAngle = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0);
            levels.Add(levels[i] + dirAngle * levelHeight);
        }
        return levels;
    }
    void initBody(GameObject group, List<Vector3> levels)
    {
        for (var i = 0; i < levels.Count; i++)
        {
            var lv = new GameObject("body_" + i);
            lv.transform.position = levels[i];
            lv.transform.parent = group.transform;
        }
    }
    void helperBB_branchToGameObjectRecursively(BB_branch b, Transform parentTransform)
    {
        var obj = (GameObject)b;
        obj.transform.parent = parentTransform;
        foreach (var v in b.children)
        {
            helperBB_branchToGameObjectRecursively(v, obj.transform);
        }
    }
    void helper_branchGetFinals(ref List<Transform> l, Transform t)
    {
        if (t.childCount == 0) l.Add(t);
        foreach (Transform child in t) helper_branchGetFinals(ref l, child);
    }
    void initBranchShort(GameObject group, Transform from,float length,int genMax,  Vector2 slope, float chance =100)
    {
        var branchInit = new GameObject("BamBooBones_branch");
        var angleInit = slope + new Vector2(Random.Range(180, 360), levelAngle);
        angleInit *= .0174f;

        branchInit.transform.position = from.position + new Vector3(Mathf.Cos(angleInit.x) * levelWidth * .5f, 0, 0);
        branchInit.transform.parent = group.transform;
        var branchSmall = BB_branch.InitPropagateRandom(
            branchInit.transform.position,
            angleInit, length, new Vector2(slope.x, slope.y) * .0174f, .90f, genMax, chance);

        helperBB_branchToGameObjectRecursively(branchSmall, branchInit.transform);
    }
    void initBranches()
    {
        for (int i = 1; i < bonesBody.transform.childCount -1; i++)
        {
            var slope = new Vector2(Random.Range(10, 20), Random.Range(20, 30));
            if (Random.Range(0, 2) == 0) slope.Scale(new Vector2(-1, 1));
            if (Random.Range(0, 2) == 0) slope.Scale(new Vector2(1, -1));
            initBranchShort(bonesBranch, bonesBody.transform.GetChild(i), levelHeight * Random.Range(.5f, .7f), 5, slope, 60);
            initBranchShort(bonesBranch, bonesBody.transform.GetChild(i), levelHeight * Random.Range(.2f, .5f), 3, slope);
        } 
    }
    void initLeaves()
    {
        List<Transform> branchEnd = new List<Transform>();
        foreach (Transform b in bonesBranch.transform) helper_branchGetFinals(ref branchEnd, b);
        foreach (var t in branchEnd)
        {
            if (t.parent.transform == null) continue;
            GameObject  g00 = new GameObject("branchEnd"), 
                        g01 = new GameObject("direction");
            g00.transform.position = t.parent.transform.position;
            g01.transform.position = t.transform.position ;
            g00.transform.parent = bonesLeaves.transform;
            g01.transform.parent = g00.transform;
        }

    }
    void Awake () {
        levels = initLevels();
        initBody(bonesBody, levels);
        initBranches();
        initLeaves();
	}
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
