using UnityEngine;
using System.Collections;

public class EasyGameObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public Vector3 getPosTopLeft(){
		return transform.position + Vector3.Scale(new Vector3(-1,1,0) ,transform.localScale * .5f);
	}
	public Vector3 getPosBottomLeft(){
		return transform.position - transform.localScale * .5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public GameObject getRoot(){
		return transform.root.gameObject;
	}
	public void setColor(float r, float g, float b,float a = 1.0f){
		renderer.material.color = new Color(r, g, b,a);
	}


}
