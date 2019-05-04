using UnityEngine;
using System.Collections;

public class EditorShape : MonoBehaviour
{
    public GameObject Block;    
    public GameObject Ring;
    public GameObject Startline;
    public GameObject Finishline;
    
    private bool canPlace = true;
    private GameObject pObject; // object that will be placed
    private GameObject temp;

    // Use this for initialization
    void Start()
    {
        pObject = Block; // placing blocks is default
    }

    // Update is called once per frame
    void Update()
    {
        //	Debug.Log(canPlace.ToString());
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            transform.Translate(new Vector3(0, 1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            transform.Translate(new Vector3(0, -1, 0));
        }

        if (Input.GetKeyUp(KeyCode.Space) && canPlace)
        {
            Instantiate(pObject, transform.position, transform.rotation);
        }
    }

    public void SetObject(string sObject)
    {
        if (sObject.Equals("Block"))
        {
            pObject = Block;
        }
        else if (sObject.Equals("Ring"))
        {
            pObject = Ring;
        }
        else if (sObject.Equals("Start"))
        {
            pObject = Startline;
        }
        else if (sObject.Equals("Finish"))
        {
            pObject = Finishline;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        canPlace = false;

        if (Input.GetKeyDown(KeyCode.Space) && !canPlace)
        {
            Destroy(other.gameObject);
            canPlace = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        canPlace = true;
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.LogError("Meep");
        if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Space) && !canPlace)
        {
            Destroy(other.gameObject);
            canPlace = true;
        }
    }
}