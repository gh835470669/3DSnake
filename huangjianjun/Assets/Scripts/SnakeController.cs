using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour {
    public Snake snake;

    private int frame = 5; //in order to make sure snake update in this World update interal
    private int interal;

    private Vector3 inputBuffer = Vector3.zero;

    void Start()
    {
        interal = FieldManager.WorldSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        frame++;
        if (frame == interal)
        {
            frame = 0;
            if (inputBuffer == Vector3.zero)
                inputBuffer = snake.head.transform.forward;
            snake.move(inputBuffer);
            inputBuffer = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputBuffer = snake.head.transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //snake.move(-Camera.main.transform.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            inputBuffer = -snake.head.transform.right;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            inputBuffer = snake.head.transform.right;
        }
    }
}
