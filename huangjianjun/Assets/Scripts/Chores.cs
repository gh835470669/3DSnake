using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chores : MonoBehaviour {
    private int point;
    private float time;
    public bool isCountDownMode = false;
    public float initTimeInCountDownMode = 60;
    public Text timeText;
    public Text pointText;

    // Use this for initialization
    void Start () {
        point = 0;
        if (isCountDownMode)
        {
            time = initTimeInCountDownMode;
        }
        else
        {
            time = 0.0f;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (isCountDownMode)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                return;
            }
        }
        else
        {
            time += Time.deltaTime;
        }
	}

    void OnGUI()
    {
        timeText.text = time.ToString();
        pointText.text = point.ToString();
    }

    public void changePoint(int deltaPoint)
    {
        point += deltaPoint;
    }

    public int getPoint()
    {
        return point;
    }

    public float getTime()
    {
        return time;
    }
}
