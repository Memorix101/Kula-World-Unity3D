using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SplashScreen : MonoBehaviour {
	
	private float timer;
	private float duration = 5f;
	public  string LevelToLoad  = "Level";
	public GameObject logo;
    float alpha = 0f;

	void Start()
    {
        PrepareLevels();
    }
	
	void Update(){
		
		timer += Time.deltaTime;

		if(timer >= duration){
            DisplayScene();
		}

        if (timer < duration / 2f)
        {
            alpha += Time.deltaTime;
        }
        else if (timer > duration / 2f)
        {
            alpha -= Time.deltaTime;
        }

        logo.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }

    void DisplayScene(){

		SceneManager.LoadScene(LevelToLoad);
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
