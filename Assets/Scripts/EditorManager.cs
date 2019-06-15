using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Coin;
    public GameObject Startline;
    public GameObject Finish;
    public GameObject Key;

    public Transform StageGameObject;

    public GameObject SaveDialogUI;
    public GameObject LoadDialogUI;
    public GameObject LoadElementUI;
    public InputField FileNameInputField;
    public Button SaveDialogButton;
    public GameObject LoadDialogContentUI;

    private GameObject gameManager;

    void Awake()
    {
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        if (FileNameInputField.text.Length >= 1)
        {
            SaveDialogButton.interactable = true;
        }
        else
        {
            SaveDialogButton.interactable = false;
        }
    }

    public void OpenLoadDialog()
    {
        SaveDialogUI.SetActive(false);
        LoadDialogUI.SetActive(true);

        string[] files = Directory.GetFiles(GameManager.GameFolderPath + "/Maps", "*mlvl");
        string[] fileNames = new string[files.Length];

        if (LoadDialogContentUI.transform.childCount > 0)
        {
            for (int i = 0; i < LoadDialogContentUI.transform.childCount; i++)
            {
                Destroy(LoadDialogContentUI.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < files.Length; i++)
        {
            fileNames[i] = Path.GetFileName(files[i]);

            GameObject go = Instantiate(LoadElementUI, transform.position, Quaternion.identity);
            go.transform.parent = LoadDialogContentUI.transform;
            string levelName = fileNames[i].Remove(fileNames[i].Length - 5);
            go.transform.GetChild(0).GetComponent<Text>().text = levelName;
            go.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelName)); //https://docs.unity3d.com/ScriptReference/UI.Button-onClick.html
        }
    }

    void LoadLevel(string name)
    {
        LoadDialogUI.SetActive(false);

        if (StageGameObject.transform.childCount > 0)
        {
            for (int i = 0; i < StageGameObject.transform.childCount; i++)
            {
                Destroy(StageGameObject.transform.GetChild(i).gameObject);
            }
        }

        if (Directory.Exists(GameManager.GameFolderPath + "/Maps"))
        {
            //Exists
            string[] data;
            FileStream fileStream = File.OpenRead(GameManager.GameFolderPath + "/Maps/" + name + ".mlvl");
            byte[] bytes = new byte[fileStream.Length];

            fileStream.Read(bytes, 0, bytes.Length);
            data = Encoding.UTF8.GetString(bytes).Split('\n');

            //gameManager.GetComponent<GameManager>().BlockList.Clear();

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
                    GameObject go = Instantiate(Cube, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                    go.transform.parent = StageGameObject.transform;
                    //gameManager.GetComponent<GameManager>().BlockList.Add(new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])));
                }
                else if (cords[0].Contains("Coin"))
                {
                    GameObject go = Instantiate(Coin, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                    go.transform.parent = StageGameObject.transform;
                }
                else if (cords[0].Contains("Start"))
                {
                    GameObject go = Instantiate(Startline, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                    go.transform.parent = StageGameObject.transform;
                }
                else if (cords[0].Contains("Finish"))
                {
                    GameObject go = Instantiate(Finish, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                    go.transform.parent = StageGameObject.transform;
                }
                else if (cords[0].Contains("Key"))
                {
                    GameObject go = Instantiate(Key, new Vector3(float.Parse(cords[1]), float.Parse(cords[2]), float.Parse(cords[3])), Quaternion.identity);
                    go.transform.parent = StageGameObject.transform;
                }
            }

            Debug.Log("Level Loaded");
        }
        else
        {
            Debug.LogError("Directory is not existing");
        }
    }

    public void SaveLevel()
    {
        if (Directory.Exists(GameManager.GameFolderPath + "/Maps"))
        {
            //Exists
            //SaveFile();
            Debug.Log("saved");
        }
        else
        {
            //Needs to be created
            Directory.CreateDirectory(GameManager.GameFolderPath + "/Maps");
            //SaveFile();
        }

        LoadDialogUI.SetActive(false);
        SaveDialogUI.SetActive(true);
    }

    public void SaveDialog()
    {
        if (FileNameInputField.text.Length >= 1)
        {
            SaveFile(FileNameInputField.text);
            SaveDialogUI.SetActive(false);
        }
    }

    void SaveFile(string filename)
    {
        StringBuilder saveBuilder = new StringBuilder();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject startpoint = GameObject.FindGameObjectWithTag("Startpoint");
        GameObject finishpoint = GameObject.FindGameObjectWithTag("Finish");
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");

        saveBuilder.Append("mlvl\n");

        foreach (GameObject cube in cubes)
        {
            saveBuilder.Append(string.Format("[Cube, {0}, {1}, {2}]\n", cube.transform.position.x, cube.transform.position.y, cube.transform.position.z));
        }

        foreach (GameObject coin in coins)
        {
            saveBuilder.Append(string.Format("[Coin, {0}, {1}, {2}]\n", coin.transform.position.x, coin.transform.position.y, coin.transform.position.z));
        }

        foreach (GameObject key in keys)
        {
            saveBuilder.Append(string.Format("[Key, {0}, {1}, {2}]\n", key.transform.position.x, key.transform.position.y, key.transform.position.z));
        }

        saveBuilder.Append(string.Format("[Start, {0}, {1}, {2}]\n", startpoint.transform.position.x, startpoint.transform.position.y, startpoint.transform.position.z));
        saveBuilder.Append(string.Format("[Finish, {0}, {1}, {2}]\n", finishpoint.transform.position.x, finishpoint.transform.position.y, finishpoint.transform.position.z));

        using (BinaryWriter writer = new BinaryWriter(File.Open(GameManager.GameFolderPath + "/Maps/" + filename + ".mlvl", FileMode.Create)))
        {
            writer.Write(saveBuilder.ToString());
        }
    }
    
    public void ExitEditor()
    {
        SceneManager.LoadScene("Menu");
    }
}