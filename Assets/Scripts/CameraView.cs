using UnityEngine;
using System.Collections;

public class CameraView : MonoBehaviour {

    public GameObject target;
    private Vector3 axis;
    private float angle = 5f;
    private float time;

    // Use this for initialization
    void Start()
    {
        RndRot();
    }
	
	// Update is called once per frame
	void Update () {

        if (!target)
        {
            target = GameObject.FindWithTag("Player");
            Debug.Log("No Player");
            return;

        }

        time += 1 * Time.deltaTime;

        if (time >= 15f)
        {
            RndRot();
            time = 0;
        }


       transform.RotateAround(target.transform.position, axis, angle * Time.deltaTime);

    }


    void RndRot()
    {
        float x_input = Random.Range(1, 90);
        float y_input = Random.Range(1, 90);
        axis = new Vector3(x_input, y_input, 0);
    }
}
