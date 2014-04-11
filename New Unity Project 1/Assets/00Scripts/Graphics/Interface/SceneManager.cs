using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneManager : MonoBehaviour {
    static SceneManager me;
    static List<Vector3> posPins = new List<Vector3>();
    public int sceneStart = 0;
    public List<GameObject> objPins = new List<GameObject>();
    public List<GameObject> scenes = new List<GameObject>();
    List<ObjRotator> rotatorsActive = new List<ObjRotator>();
    List<ObjRotator> rotatorsDeaActive = new List<ObjRotator>();
	// Use this for initialization
    public static void EVENT_TranslationSceneInit2Game()
    {
        me.createRotation(0, posPins[0], posPins[1], .5f,false);
        me.createRotation(1, posPins[3], posPins[0], .5f);
    }
    void Awake()
    {
        if (scenes.Count == 0) throw new MissingReferenceException("ListComponenetMissing");
        posPins = (from s in objPins select s.transform.position).ToList<Vector3>();
        me = this;

        int index = sceneStart % scenes.Count;
        scenes[index].transform.position = posPins[0];
        for (int i = 1; i < scenes.Count; i++)
        {
            int indexNew = (index + i) % scenes.Count;
            scenes[indexNew].transform.position = posPins[posPins.Count - 1];
        }
	}
    

    void Start()
    {
        //foreach (GameObject e in scenes) e.transform.position = new Vector3(1, 0, 0);
        //createRotation(0, posPins[0], posPins[1],10);
        //createRotation(1, posPins[3], posPins[0], 10);
        //Debug.Log("TotalSaved Scenes : " + scenes.Count);
       // Debug.Log()
    }
    void createRotation(int n, Vector3 from , Vector3 to,float time=3.0f, bool active = true)
    {
        var rotator = new ObjRotator(scenes[n].gameObject, from, to, Camera.main.transform.position, time);
        if (active)
            rotatorsActive.Add(rotator);
        else
            rotatorsDeaActive.Add(rotator);
    }
	// Update is called once per frame
	void Update () {
        for (var i = rotatorsActive.Count - 1; i >= 0; i--)
        {
            if (!rotatorsActive[i].update())
            {
                rotatorsActive.RemoveAt(i);
            }
        }
        for (var i = rotatorsDeaActive.Count - 1; i >= 0; i--)
        {
            if (!rotatorsDeaActive[i].update())
            {
                rotatorsDeaActive[i].Obj.SetActive(false);
                rotatorsDeaActive.RemoveAt(i);
            }
        }
	
	}
}
