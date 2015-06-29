using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject EditorGO;
    public GameObject Editor;

    public string file = "Resources/test.map";
    public GameObject Cube;
    public GameObject Startline;
    public GameObject Finish;

    private bool Edit = false;

    public enum GameState
    {
        Play,
        Editor
    }

    public static GameState state;

	// Use this for initialization
	void Start () {

        if(!EditorGO)
        {
            EditorGO = GameObject.Find("Editor");

            if(!GameObject.Find("Editor"))
            {
                Instantiate(Editor, new Vector3(0, 0, 0), Quaternion.identity);
              //  Debug.LogError("Can't find Editor");
                EditorGO = GameObject.Find("Editor(Clone)");
            }

        }

	
	}

	// Update is called once per frame
	void Update () {

        InputStuff();

        if(state == GameState.Play)
        {
            EditorGO.SetActive(false);
        }

        if (state == GameState.Editor)
        {
            EditorGO.SetActive(true);
        }
	}

    void InputStuff()
    {
		if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.F1))
        {
            Edit = !Edit;
        }

        if (Edit)
        {
            state = GameState.Editor;
        }
        else
        {
            state = GameState.Play;
        }
    }

    void OnGUI()
    {
      //  GUI.Box(new Rect(0, 0, 100, 25), m.ToString());
    }
	
}
