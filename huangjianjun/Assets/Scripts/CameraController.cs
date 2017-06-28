using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Snake snake;
    private Vector3 lastSnakeUpDir;
    public Camera camera;
    public FieldManager fieldManager;

    private float distance;
    public float horizontalSpeed = 10.0F;
    public float verticalSpeed = 10.0F;

    // Use this for initialization
    void Start () {
        lastSnakeUpDir = Vector3.back;
        distance = (float)fieldManager.len / 2 + 10;
        transform.position = fieldManager.WorldCenter + lastSnakeUpDir * distance;
        transform.LookAt(fieldManager.WorldCenter);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            this.transform.RotateAround(fieldManager.WorldCenter, transform.up, h);
            this.transform.RotateAround(fieldManager.WorldCenter, transform.right, -v);
        }
    }
}
