using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PlayIcon, EditIcon;
    public bool Playmode;
    public static EditState editState;
    public GameObject PauseUI;
    private bool InEditor;

    public static string GameFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kula Roll Away Unity");

    public enum EditState
    {
        Edit,
        Play
    }

    public EditState getEditState
    {
        get { return editState; }
    }

    public bool getInEditor
    {
        get { return InEditor; }
    }

    void Awake()
    {
        if (!Playmode)
        {
            editState = EditState.Edit;
        }
        else
        {
            editState = EditState.Play;
        }

        if (Playmode)
        {
            GameObject emptyGO = new GameObject("emptyGO");
            PlayIcon = emptyGO;
            EditIcon = emptyGO;
        }
    }

    // Use this for initialization
    void Start()
    {        
        if(!GameObject.FindObjectOfType<EditorManager>())
        {
            InEditor = false;
        }
        else
        {
            InEditor = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        buildState();

        if(Input.GetKeyDown(KeyCode.Escape) && !InEditor)
        {
            Debug.Log("asdf");
            PauseUI.SetActive(true);
        }
    }

    public void SetEditMode(string mode)
    {
        if (mode.Equals("Play"))
        {
            editState = EditState.Play;
        }
        else if (mode.Equals("Edit"))
        {
            editState = EditState.Edit;
        }
    }

    void buildState()
    {
        if (editState == EditState.Edit)
        {
            EditIcon.gameObject.SetActive(true);
        }
        else
        {
            EditIcon.gameObject.SetActive(false);
        }

        if (editState == EditState.Play)
        {
            PlayIcon.gameObject.SetActive(true);
        }
        else
        {
            PlayIcon.gameObject.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}