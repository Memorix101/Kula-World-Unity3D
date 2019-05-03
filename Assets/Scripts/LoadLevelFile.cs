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

    void Start()
    {
        LoadLevel();

    }

    void Update()
    {

    }

    public void LoadLevel()
    {
        string[] data;

        // FileStream fileStream = File.OpenRead("Assets/Resources/Maps/test.mlvl");
        FileStream fileStream = File.OpenRead(GameManager.GameFolderPath + "/Maps/test.mlvl");

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
                GameObject.Instantiate(Cube, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
            }
            else if (cords[0].Contains("Coin"))
            {
                GameObject.Instantiate(Coin, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
            }
            else if (cords[0].Contains("Start"))
            {
                GameObject.Instantiate(Startline, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
            }
            else if (cords[0].Contains("Finish"))
            {
                GameObject.Instantiate(Finish, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
            }
        }

        Debug.Log("loaded");
    }
}