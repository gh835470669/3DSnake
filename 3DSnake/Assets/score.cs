using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour {
    public int num = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        num = 100;
	}
    void OnGUI()
    {
        GUI.skin.label.fontSize = 18;
        GUI.skin.label.normal.textColor = new Color(0, 0, 0);
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, 50, Screen.width * 0.15f, Screen.height * 0.1f), "score：" + num);
    }
}
