using UnityEngine;
using System.Collections;

public class CameraRatioCorrecter : MonoBehaviour {
    public GameObject dummy;
    float   field,
            zoomInit;
    Vector2 fieldCosSin;
	void correctResolution(){
        float ratio = 1 / camera.aspect;
        float x = fieldCosSin.x * ratio; 
        if(ratio >1 ) camera.fieldOfView = Mathf.Atan(x/fieldCosSin.y) * (180 / 3.14f) * 2.0f;
        else camera.fieldOfView = field;
        camera.orthographicSize = zoomInit * ratio;
	}
	// Use this for initialization
	void Awake () {
        float angle = camera.fieldOfView * .0174f*.5f;
        fieldCosSin = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        field = camera.fieldOfView;
		zoomInit = camera.orthographicSize;
		correctResolution ();
	}
	// Update is called once per frame
    void Update()
    {
        correctResolution();
	
	}
}
