using UnityEngine;
using System.Collections;

public class SceneCamera : MonoBehaviour {

	private float CamSpeed = 5f;
	private GameObject ballPlayer;
	private float cameraDistance = 5f;

    Quaternion endpos;
    Vector3 axis;
    float tarY = 0f;

    public static CamPos camPos;
    private bool camTurn;

    float z_val;
    float x_val;
    float cur_posX;
    float cur_posZ;

    private float angle = 5f;
    private float time;

    private GameObject stage;

    public enum CamPos
    {
        fwd,
        back,
        left,
        right
    }


    // Use this for initialization
    void Start () {

        time = 14.9f; //a little trick ;) pssst
        camPos = CamPos.fwd;
	}

	// Update is called once per frame
	void Update () {

		if(GameManager.editState == GameManager.EditState.Play)
		{
             PlayCam();	
		}
		else if(GameManager.editState == GameManager.EditState.Edit)
		{
			EditCam();
        }

        if(!stage)
            stage = GameObject.Find("Stage");
    }

	void PlayCam()
	{
		if(ballPlayer == null){
            ballPlayer = GameObject.FindWithTag("Player");
            //transform.SetParent(ballPlayer.transform);
			Debug.Log("No Player");
		}

        if (ballPlayer != null && !Player.StageClear)
        {

            //Debug.Log("Player Stuff: " + ballPlayer.transform.position);

            Turn();

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // turn left
                camTurn = true;
                tarY += 90f;
                axis = new Vector3(0, 1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                //turn right
                camTurn = true;
                tarY -= 90f;
                axis = new Vector3(0, -1, 0);
            }

            if (tarY >= 360f)
                tarY = 0f;

            if (tarY < 0f)
                tarY = 270f;

            transform.position = ballPlayer.transform.position;
            //transform.RotateAround(ballPlayer.transform.position, axis, 200 * Time.deltaTime);
            Quaternion tarRot = transform.rotation;
            tarRot.eulerAngles = new Vector3(20, tarY, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, 0.25f);
            transform.position += -transform.forward * cameraDistance + Vector3.up;
            //lastCamRot = transform.position;
            //transform.position = new Vector3(ballPlayer.transform.position.x, ballPlayer.transform.position.y,ballPlayer.transform.position.z - cameraDistance);

        }
        else if (ballPlayer != null && Player.StageClear)
        {
            //Debug.Log("HERE" + time);

            time += 1 * Time.deltaTime;

            if (time >= 15f)
            {
                transform.position = new Vector3(5, 10, -15);
                RndRot();
                time = 0;
            }

            transform.RotateAround(stage.transform.position, axis, angle * Time.deltaTime);
            transform.LookAt(stage.transform);
        }
	}

    void Turn()
    {
        float angle = transform.rotation.eulerAngles.y;

        if (Mathf.Abs(Mathf.Abs(angle) - Mathf.Abs(tarY)) < 5) //  if(angle.Equals(tarY))
        {
            camTurn = false;
            axis =  Vector3.zero;
        }
    }

    void EditCam()
	{
        camPos = CamPos.fwd;

        if (Input.GetKey(KeyCode.W))
		{
			transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * CamSpeed);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * CamSpeed);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * CamSpeed);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * CamSpeed);
		}
		else if (Input.GetKey(KeyCode.E))
		{
			transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * CamSpeed);
		}
		else if (Input.GetKey(KeyCode.Q))
		{
			transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * CamSpeed);
		}
	}

    void RndRot()
    {
        float x_input = Random.Range(1, 90);
        float y_input = Random.Range(1, 90);
        axis = new Vector3(x_input, y_input, 0);
    }
}
