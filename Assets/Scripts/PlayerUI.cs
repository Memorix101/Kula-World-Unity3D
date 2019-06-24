using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Restart()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Camera.main.GetComponent<SceneCamera>().tarY = 0f;
        //Camera.main.GetComponent<SceneCamera>().time = 0f;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.parent.GetComponent<Player>().RestartLevel();
    }

    public void NextLevel()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Camera.main.GetComponent<SceneCamera>().tarY = 0f;
        //Camera.main.GetComponent<SceneCamera>().time = 0f;

        transform.parent.GetComponent<Player>().NextLevel();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
