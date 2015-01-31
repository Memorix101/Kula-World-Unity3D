using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

public class EditorManager : MonoBehaviour {

    public GameObject Cube;
    public GameObject Startline;
    public GameObject Finish;

    string pathIO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CuboEd\\Unity");
    string dirIO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CuboEd\\Unity");

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void LoadLevel()
    {

        if (Directory.Exists(pathIO))
        {
            //Exists

        string[] data;

        FileStream fileStream = File.OpenRead(pathIO + "\\test.mlvl");

        byte[] bytes = new byte[fileStream.Length];

        fileStream.Read(bytes, 0, bytes.Length);
        data = System.Text.Encoding.UTF8.GetString(bytes).Split('\n');

        if(!data[0].Contains("mlvl"))
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
        else
        {
            Debug.LogError("Directory is not existing");
        }

    }

    public void SaveLevel()
    {
        if (Directory.Exists(pathIO))
        {
            //Exists
            saveFile();
        Debug.Log("saved");
        }
        else
        {
            //Needs to be created
            Directory.CreateDirectory(dirIO);

            saveFile();
        }
    }

    void saveFile()
    {
        StringBuilder saveBuilder = new StringBuilder();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Block");
        GameObject startpoint = GameObject.FindGameObjectWithTag("Startpoint");
        GameObject finishpoint = GameObject.FindGameObjectWithTag("Finish");

        saveBuilder.Append("mlvl\n");

        foreach (GameObject cube in cubes)
        {
            saveBuilder.Append(string.Format("[Cube, {0}, {1}, {2}]\n", cube.transform.position.x, cube.transform.position.y, cube.transform.position.z));
        }

        saveBuilder.Append(string.Format("[Start, {0}, {1}, {2}]\n", startpoint.transform.position.x, startpoint.transform.position.y, startpoint.transform.position.z));
        saveBuilder.Append(string.Format("[Finish, {0}, {1}, {2}]\n", finishpoint.transform.position.x, finishpoint.transform.position.y, finishpoint.transform.position.z));

        using (BinaryWriter writer = new BinaryWriter(File.Open(pathIO + "\\test.mlvl", FileMode.Create)))
        {
            writer.Write(saveBuilder.ToString());
        }
    }
}
