using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(transform.gameObject);
        Application.LoadLevel("Menu");

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
