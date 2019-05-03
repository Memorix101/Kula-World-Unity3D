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
        string pathIO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kula Roll Away\\Unity");
        if (Directory.Exists(pathIO))
        {
            //all good :)
        }
        else
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Maps", "*mlvl");
            string[] fileNames = new string[files.Length];

            Directory.CreateDirectory(pathIO);

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = Path.GetFileName(files[i]);
                File.Copy(files[i], pathIO + "/" + fileNames[i], true);
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
