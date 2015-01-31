using UnityEngine;
using System.Collections;

public class TriggerActor : MonoBehaviour {

    public GameObject Player;


    public enum Actor
    {
        Start,
        Finish
    }

    public Actor actor;


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
        if(actor == Actor.Start)
        {
            Starppoint();
        }

        if (actor == Actor.Finish)
        {
            Finishline();
        }

	}

    void Starppoint()
    {

        if(!GameObject.FindGameObjectWithTag("Player") && GameManager.state == GameManager.GameState.Play)
        {
            Instantiate(Player, transform.position, transform.rotation);
         //   Debug.Log("Play");
        }

    }

    void Finishline()
    {

    }
}
