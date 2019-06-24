using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SplashScreen : MonoBehaviour {
	
	private float timer;
	private float time = 5f;
	public  string LevelToLoad  = "Level";
	public GameObject logo;
	
	void Start()
    {
        PrepareLevels();
        logo.SetActive(false);
	}
	
	void Update(){
		
		timer += Time.deltaTime;

		if(timer >= time){
            DisplayScene();
		}


        if (timer >= 0.1f){
			logo.SetActive(true);
		}
        else
            logo.SetActive(false);

    }

    void DisplayScene(){

		SceneManager.LoadScene( LevelToLoad );
		//Debug.Log("SplashScreen Over!");
	}

    void PrepareLevels()
    {
        if (Directory.Exists(GameManager.GameFolderPath))
        {
            // all good :)
        }
        else
        {
            Directory.CreateDirectory(GameManager.GameFolderPath);
        }
        if (Directory.Exists(GameManager.GameFolderPath + "/Maps"))
        {
            // all good :)
        }
        else
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Maps", "*mlvl");
            string[] fileNames = new string[files.Length];

            Directory.CreateDirectory(GameManager.GameFolderPath + "/Maps");

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = Path.GetFileName(files[i]);
                File.Copy(files[i], GameManager.GameFolderPath + "/Maps/" + fileNames[i], true);
            }
        }
    }
}
