using UnityEngine;
using System.Collections;

public class Tile : EasyGameObject {

    public delegate void del00(Tile t);
    public del00 EVENT_Click;
    public int[] index;
	// Use this for initialization
	void Start () {
		//setColor (Random.Range(1,10) * .1f,Random.Range(1,10) * .1f,Random.Range(1,10) *.1f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        if (EVENT_Click != null) EVENT_Click(this);
    }
}
