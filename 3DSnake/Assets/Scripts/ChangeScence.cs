using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScence : MonoBehaviour {

	public void changeTo(string nameOfScence)
    {
        SceneManager.LoadScene(nameOfScence);
    }
}
