using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeControl : MonoBehaviour {

    public float Value = 0.5f;
    public float Fade = 0.01f;

    public time GBS;

    public Vector2 Offset;

    public Vector2 LabelOffSet;

    public string playText = "Play";
    public bool IsPlaying = false;

    void Start()
    {
        GBS = GetComponent<time>();
        GBS.Value = 1;
    }


    void Update()
    {
        GBS.Value = 0.01f;

    }
}
