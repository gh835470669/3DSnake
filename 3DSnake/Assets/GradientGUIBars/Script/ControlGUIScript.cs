using UnityEngine;
using System.Collections;

public class ControlGUIScript : MonoBehaviour {

	public float Value = 0.5f;
	public float Fade = 0.01f;

	public hp GBS;

	public Vector2 Offset;

	public Vector2 LabelOffSet;

	public string playText = "Play";
	public bool IsPlaying = false;

	void Start()
	{
		GBS = GetComponent<hp>();
        GBS.Value = 0;
	}


	void Update () 
	{
        GBS.Value += 0.01f;

	}
}
