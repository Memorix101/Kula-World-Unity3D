using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

public class LoadLevelFile : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Startline;
    public GameObject Finish;
    public GameObject Coin;
    public GameObject Key;

    private GameObject levelGO;

    void Start()
    {
        levelGO = new GameObject("Stage");
        LoadLevel();
    }

    void Update()
    {

    }

    public void NextLevel()
    {
        if (levelGO.transform.childCount > 0)
        {
            for (int i = 0; i < levelGO.transform.childCount; i++)
            {
                Destroy(levelGO.transform.GetChild(i).gameObject);
            }
        }

        // TODO get current level id and increment (original only)
    }

    public void LoadLevel()
    {
        string[] data;

        // FileStream fileStream = File.OpenRead("Assets/Resources/Maps/test.mlvl");
        FileStream fileStream = File.OpenRead(GameManager.GameFolderPath + "/Maps/test.mlvl");
        //FileStream fileStream = File.OpenRead(GameManager.GameFolderPath + "/Maps/luna.mlvl");

        byte[] bytes = new byte[fileStream.Length];

        fileStream.Read(bytes, 0, bytes.Length);
        data = System.Text.Encoding.UTF8.GetString(bytes).Split('\n');

        if (!data[0].Contains("mlvl"))
        {
            Debug.LogError("Wrong format !");
        }

        for (int i = 0; i < data.Length; i++)
        {
            string match = Regex.Match(data[i], @"\[([^]]*)\]").Groups[1].Value;
            match = match.Replace(" ", "");

            string[] cords = match.Split(',');

            if (cords[0].Contains("Cube"))
            {
                GameObject GO;
                GO = Instantiate(Cube, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                GO.transform.parent = levelGO.transform;
                //GetComponent<GameManager>().BlockList.Add(new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])));
            }
            else if (cords[0].Contains("Coin"))
            {
                GameObject GO;
                GO = Instantiate(Coin, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                GO.transform.parent = levelGO.transform;
            }
            else if (cords[0].Contains("Start"))
            {
                GameObject GO;
                GO = Instantiate(Startline, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                GO.transform.parent = levelGO.transform;
            }
            else if (cords[0].Contains("Finish"))
            {
                GameObject GO;
                GO = Instantiate(Finish, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                GO.transform.parent = levelGO.transform;
            }
            else if (cords[0].Contains("Key"))
            {
                GameObject GO;
                GO = Instantiate(Key, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                GO.transform.parent = levelGO.transform;
            }
        }

        Debug.Log("Level Loaded");
    }
}