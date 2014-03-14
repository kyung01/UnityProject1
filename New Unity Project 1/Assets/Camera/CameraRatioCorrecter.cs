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
		correctResolution ();


		//Debug.Log (cam.aspect);
		//Debug.Log ("ASEPCT NOW " + Screen.currentResolution.width + " " + Screen.currentResolution.height);
		//cam.orthographicSize *= 1.01f;
		//cam.aspect *= 0.991f;
		//cam.aspect =(Screen.currentResolution.width) / Screen.currentResolution.height ;
	
	}
}
