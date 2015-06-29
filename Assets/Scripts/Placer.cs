using UnityEngine;
using System.Collections;

public class Placer : MonoBehaviour {

    public GameObject Cube;
    public GameObject Startpoint;
    public GameObject Finishline;
    public GameObject PauseUI;
    private Vector3 store;

    bool Starter, Ender = false; //check if startpoint / finishline already set

    bool move = false;

    bool paused = false;

	// Use this for initialization
	void Start () {

        if(GameObject.FindGameObjectWithTag("Startpoint"))
        { 
            GameObject s = GameObject.FindGameObjectWithTag("Startpoint");
            transform.position = s.transform.position;
        }
	
	}
	

    void Inputmeow()
    {

        if (Input.GetAxis("JoyVertical") == 0f && Input.GetAxis("JoyHorizontal") == 0f)
        {
            move = true;
        }

		if (Input.GetAxis("JoyVertical") == 1f || Input.GetKeyDown(KeyCode.W))
        {
            if (move)
            {
                move = false;
                transform.Translate(new Vector3(0, 0, 1));
            }
        }

		if (Input.GetAxis("JoyVertical") == -1f || Input.GetKeyDown(KeyCode.S))
        {
            if (move)
            {
                move = false;
                transform.Translate(new Vector3(0, 0, -1));
            }
        }

		if (Input.GetAxis("JoyHorizontal") == 1f || Input.GetKeyDown(KeyCode.D))
        {
            if (move)
            {
                move = false;
                transform.Translate(new Vector3(1, 0, 0));
            }
        }

		if (Input.GetAxis("JoyHorizontal") == -1f || Input.GetKeyDown(KeyCode.A))
        {
            if (move)
            {
                move = false;
                transform.Translate(new Vector3(-1, 0, 0));
            }
        }

		if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Q))
        {
            transform.Translate(new Vector3(0, -1, 0));
        }


		if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.E))
        {
            transform.Translate(new Vector3(0, 1, 0));
        }

		if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(Cube, transform.position, transform.rotation);
        }

		if (Input.GetKeyDown(KeyCode.Joystick1Button4) && !Starter || Input.GetKeyDown(KeyCode.K) && !Starter)
        {
            Instantiate(Startpoint, transform.position, transform.rotation);
        }

		if (Input.GetKeyDown(KeyCode.Joystick1Button5) && !Ender || Input.GetKeyDown(KeyCode.L) && !Ender)
        {
            Instantiate(Finishline, transform.position, transform.rotation);
        }

		if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Delete))
        {
            store = transform.position;
            transform.position = new Vector3(999, 999, 999);
            transform.position = new Vector3(store.x, store.y, store.z);
        }
    }

	// Update is called once per frame
	void Update () {

        CheckActors();

		if(Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if(paused)
        {
            PauseUI.SetActive(true);
        }
        else
        {
            Inputmeow();
            PauseUI.SetActive(false);
        }

	}

     void OnTriggerStay(Collider other)
    {
		if (Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Delete))
        {
            Destroy(other.gameObject);
        }
    }

    void CheckActors()
     {
        if(GameObject.FindWithTag("Startpoint"))
        {
            Starter = true;
        }
        else {    
            Starter = false;
        }

        if (GameObject.FindWithTag("Finish"))
        {
            Ender = true;
        }
        else
        {
            Ender = false;
        }
     }


}
