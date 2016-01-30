using UnityEngine;
using System.Collections;

public class TriggerObject : MonoBehaviour {

	public bool Startline;
	public bool Finishline;


	public GameObject Player;

	private bool spawnAtStart = true;

	// Use this for initialization
	void Start () {

	
	
	}
	
	// Update is called once per frame
	void Update () {

		if(GameManager.editState == GameManager.EditState.Play)
		{
			if(Startline && spawnAtStart){

				Instantiate(Player, transform.position, transform.rotation);
				spawnAtStart = false;

			}
				
		}
		else if(GameManager.editState == GameManager.EditState.Edit)
		{
			//Reset values here
			spawnAtStart = true;


		}

	
	}
}
