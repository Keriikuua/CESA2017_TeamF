using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour {

    public GameObject obj;
    public Camera cam;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        Zoom();
    }

    void Zoom()
    {
        //transform.LookAt(Vector3.SmoothDamp(transform.position,obj.transform.position, 
        //    , 0.5f));
        //float step = 15 * Time.deltaTime;
        Quaternion rotation = Quaternion.RotateTowards(cam.transform.rotation, obj.transform.rotation, 1 * Time.deltaTime);
        cam.transform.rotation = rotation;

        //Quaternion.RotateTowards(obj.transform.rotation, cam.transform.rotation, 15.0f);
        cam.transform.LookAt(obj.transform);
    }
}
