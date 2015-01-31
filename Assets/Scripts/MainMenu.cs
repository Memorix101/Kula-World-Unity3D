using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        Application.LoadLevel("Test");
    }

    public void Editor()
    {
        Application.LoadLevel("Editor");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
