using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    public string nextScene;
	// Use this for initialization
	void Start () {
		
	}

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
