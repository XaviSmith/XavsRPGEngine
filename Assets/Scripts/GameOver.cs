using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GetInput());
	}

    IEnumerator GetInput()
    {
        while(true)
        {
            yield return null;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Overworld");
            }
        }
    }
}
