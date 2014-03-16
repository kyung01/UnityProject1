using UnityEngine;
using System.Collections;

public class CameraRatioCorrecter : MonoBehaviour {
	
	float	fieldOfView,
			zoomInit;
	void correctResolution(){
		float ratio = 1;
		var cam = transform.root.gameObject.GetComponent ("Camera") as Camera;
		if (cam.aspect > 1)
				cam.orthographicSize = zoomInit;
		else
				ratio = cam.aspect;
		camera.orthographicSize = zoomInit / ratio;
		camera.fieldOfView = fieldOfView / ratio;
	}
	// Use this for initialization
	void Start () {	
		fieldOfView = camera.fieldOfView;
		zoomInit = camera.orthographicSize;
		correctResolution ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
