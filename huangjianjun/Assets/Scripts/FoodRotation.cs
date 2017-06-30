using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRotation : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(this.transform.up, 5, Space.Self);
	}
}
