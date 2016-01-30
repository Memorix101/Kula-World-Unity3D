using UnityEngine;
using System;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

	public Sprite[] sprites; //Sprites, just sprites ....
	private int itemCount; //WE CAN COUNT STUFF, YES !!!
	private float itemF; // This is our man !
	private int item; // float are women and int are men ... they dont understand each other ...
	public float timeRate = 6; // Anything to explain here ?


	// Use this for initialization
	void Start () {

		itemCount = sprites.Length;
	}
	
	// Update is called once per frame
	void Update () {

		if(GameManager.editState == GameManager.EditState.Play)
		{
		   itemF += 1 * Time.deltaTime * timeRate;

		   if(itemF >= (float)itemCount - 1) //converts an int to a float
		   {
			itemF = 0;
		   }

		}
		else if(GameManager.editState == GameManager.EditState.Edit)
		{
			itemF = 0;
		}

		item = Convert.ToInt32(itemF); // Converts float into an int (If real life would be that easy :P )

	//	Debug.Log(item.ToString());
		//Debug.Log(eState.ToString());

		gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Convert.ToInt32(item)];
	
	}
}
