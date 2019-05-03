using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class MainMenu : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if (Directory.Exists(GameManager.GameFolderPath))
        {
            //all good :)
        }
        else
        {
            Directory.CreateDirectory(GameManager.GameFolderPath);
        }
        if (Directory.Exists(GameManager.GameFolderPath + "/Maps"))
        {
            //all good :)
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

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadEditor()
    {
        SceneManager.LoadScene("Editor");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}