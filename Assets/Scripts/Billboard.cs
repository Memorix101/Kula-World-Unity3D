using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public bool EditOnly;
    public bool CollideEditOnly;
	public GameObject EditMark;
	public GameObject CubeCollider;
//	public Component script;

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {

        if (EditOnly)
        {


            if (GameManager.editState == GameManager.EditState.Edit)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;

                if (EditMark)
                    EditMark.gameObject.GetComponent<MeshRenderer>().enabled = true;

                CubeCollider.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

                if (EditMark)
                    EditMark.gameObject.GetComponent<MeshRenderer>().enabled = false;

                CubeCollider.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (GameManager.editState == GameManager.EditState.Edit && CollideEditOnly)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;

            if (EditMark)
                EditMark.gameObject.GetComponent<MeshRenderer>().enabled = true;

            CubeCollider.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
       else if (GameManager.editState == GameManager.EditState.Play && CollideEditOnly)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            if (EditMark)
                EditMark.gameObject.GetComponent<MeshRenderer>().enabled = false;

            CubeCollider.gameObject.GetComponent<BoxCollider>().enabled = true;

        }

        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position);
	}

}
