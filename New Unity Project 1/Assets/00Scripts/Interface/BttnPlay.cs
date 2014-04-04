using UnityEngine;
using System.Collections;

public class BttnPlay : EasyGameObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        Debug.Log("hiPlayButton");
        SceneManager.EVENT_TranslationSceneInit2Game();
    }

}
