using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject PlayIcon, EditIcon;

    public bool Playmode;

    public static EditState editState;

	public enum EditState
	{
		Edit,
		Play
	}

    void Awake()
    {
        if (!Playmode)
            editState = EditState.Edit;
        else
            editState = EditState.Play;

        if (Playmode)
        {
            GameObject emptyGO = new GameObject("emptyGO");

            PlayIcon = emptyGO;
            EditIcon = emptyGO;
        }
    }

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {

		buildState();

	}


	public void SetEditMode(string mode)
	{
		if(mode.Equals("Play"))
		{
			editState = EditState.Play;
		}
		else if(mode.Equals("Edit"))
		{
			editState = EditState.Edit;
		}
	}

	void buildState()
	{
		if(editState == EditState.Edit)
		{
			EditIcon.gameObject.SetActive(true);
		}
		else
		{
			EditIcon.gameObject.SetActive(false);
		}

		if(editState == EditState.Play)
		{
			PlayIcon.gameObject.SetActive(true);
		}
		else
		{
			PlayIcon.gameObject.SetActive(false);
		}
	}
}
