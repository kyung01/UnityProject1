using UnityEngine;
using System.Collections;

public class MeshVertices : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    gameObject.AddComponent("MeshFilter");
		gameObject.AddComponent("MeshRenderer");
		var mesh= (GetComponent(typeof(MeshFilter)) as MeshFilter ).mesh;
		mesh.Clear();
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0),new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
		mesh.triangles = new int[]{0,3,1, 0, 2, 3};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
