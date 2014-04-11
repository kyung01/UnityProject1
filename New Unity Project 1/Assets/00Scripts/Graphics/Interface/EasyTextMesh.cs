using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;

public class EasyTextMesh : EasyGameObject {
	// Use this for initialization
    public TextMesh mesh;
    public string data = "0";
	void Awake () {
        renderer.enabled = false;
        display(data);
	}

	void Update () {
	}
    public void display(string s)
    {
        Quaternion rotBefore = transform.rotation;
        transform.rotation = Quaternion.identity;

        mesh.transform.parent = null;
        mesh.transform.localScale = new Vector3(1, 1, 1);
        mesh.text = s;
        Vector3 scale = ScaleWorld.divide(mesh.renderer.bounds.size);
        float ratio = ((scale.x < scale.y) ? scale.x : scale.y);
        mesh.transform.localScale *= ratio;

        mesh.transform.position = 
            transform.position + new Vector3(
            -Mathf.Abs(mesh.renderer.bounds.size.x) * .5f,
            Mathf.Abs(mesh.renderer.bounds.size.y) * .5f, 0);

        mesh.transform.parent = transform;
        transform.rotation = rotBefore;
    }
}
