using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControllerGS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(Vector3.zero, Vector3.up, -1);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("scene");
        }
    }
}
