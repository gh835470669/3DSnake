using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooselevel : MonoBehaviour {
    int t;
    int []x = {0, 20, 20, 0, 10};
    int[] z = { 0, 0, 20, 20, 10 };
    int max;
    string[] s = { "关卡1", "关卡2", "关卡3", "关卡4", "关卡5", "关卡6", "关卡7", "关卡8", "关卡9", "关卡10", };
    bool move = false;
    Vector3 rotation;
	// Use this for initialization
	void Start () {
        max = 2;
        t = 0;
        rotation = transform.localEulerAngles; 
	}
	
	// Update is called once per frame
	void Update () {
        if (!move)
        {
            gameObject.transform.RotateAround(new Vector3(x[t], 0, z[t]), new Vector3(0, 1, 0), 30 * Time.deltaTime); 
        }
        float step = 50 * Time.deltaTime;
        if (move)
            if (gameObject.transform.localPosition != new Vector3(x[t], 1, -10 + z[t]))
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(x[t], 1, -10+z[t]), step);
            else
                move = false;
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (t < max)
            {
                transform.position = new Vector3(x[t], 1, -10+z[t]);
                transform.localEulerAngles = rotation;
                t += 1;
                move = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (t > 0)
            {
                transform.position = new Vector3(x[t], 1, -10 + z[t]);
                transform.localEulerAngles = rotation;
                t -= 1;
                move = true;
            }  
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            startgame(t);
        }  
	}
/*    void OnGUI()
    {
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.normal.textColor = new Color(0, 0, 0);
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, Screen.height * 0.7f, Screen.width, Screen.height * 0.1f), s[t]);
    }*/
    void startgame(int t)
    {
        if (t == 0) Application.LoadLevel("scene1");

        if (t == 1) Application.LoadLevel("scene2");

        if (t == 2) Application.LoadLevel("scene3");
    }
}
