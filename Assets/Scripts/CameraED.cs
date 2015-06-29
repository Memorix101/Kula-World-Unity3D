using UnityEngine;
using System.Collections;

public class CameraED : MonoBehaviour {


    private Vector3 vCam;
    private float xv = 0f;
    private float yv = 0f;

    public Transform Player;
    Quaternion endpos;
    float tarY = 0;

	private Vector3 mouseOrigin;

    public enum CamPos 
    {
        fwd,
        back,
        left,
        right
    }

    enum CamTurn
    {
        left,
        right,
        idle
    }

    private CamTurn camTurn;

    public static CamPos camPos;

	// Use this for initialization
	void Start () {
        

	}

	
	void Update()
    {
        if (GameManager.state == GameManager.GameState.Editor)
        {
            EditorCam();
        }
        
       if(GameManager.state == GameManager.GameState.Play)
        {

            if (!Player)
            {
             Player = GameObject.FindWithTag("Player").transform;
             transform.eulerAngles = new Vector3(30, 0, 0);
             tarY = 0f;
             camPos = CamPos.fwd;
            }

            Follow();
            Turn();
        }

   //    Debug.Log(endpos);
    }

    void Follow()
    {
 
		if(Input.GetKeyDown(KeyCode.Joystick1Button4) && camTurn == CamTurn.idle || Input.GetKeyDown(KeyCode.D) && camTurn == CamTurn.idle)
        {
            CamPosNxt();
            camTurn = CamTurn.right;
            tarY += 90f;
        }

		if (Input.GetKeyDown(KeyCode.Joystick1Button5) && camTurn == CamTurn.idle || Input.GetKeyDown(KeyCode.A) && camTurn == CamTurn.idle)
        {
            CamPosBck();
            camTurn = CamTurn.left;
            tarY -= 90f;
        }

        transform.position = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y) * 5 + 
		                                 Player.transform.position.x, Player.transform.position.y + 4, 
		                                 Player.transform.position.z - 5 * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y));
    }

    void Turn()
    {

        endpos = Quaternion.Euler(30, tarY, 0);
       // Debug.Log("endpos cam rot " + endpos);

        if(endpos == transform.rotation)
        {
            camTurn = CamTurn.idle;
        }

        if (camTurn == CamTurn.right)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endpos, 200 * Time.deltaTime);
        }

        if (camTurn == CamTurn.left)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endpos, 200 * Time.deltaTime);
        }
    
    }


    void CamPosNxt()
    {
        if (camPos == CamPos.fwd)
            camPos = CamPos.right;
        else if (camPos == CamPos.right)
            camPos = CamPos.back;
        else if (camPos == CamPos.back)
            camPos = CamPos.left;
        else if (camPos == CamPos.left)
            camPos = CamPos.fwd;

    }

    void CamPosBck()
    {
        if (camPos == CamPos.fwd)
            camPos = CamPos.left;
        else if (camPos == CamPos.left)
            camPos = CamPos.back;
        else if (camPos == CamPos.back)
            camPos = CamPos.right;
        else if (camPos == CamPos.right)
            camPos = CamPos.fwd;

    }

	void EditorCam () {

		//controller
		if (Input.GetAxis("JoyThumbSecV") <= 0.5 )
       {
          // transform.RotateAround(Vector3.zero, -Vector3.up, 90 * Time.deltaTime);
           transform.Rotate(Vector3.up * 75 * Time.deltaTime, Space.World);
       }

		 if (Input.GetAxis("JoyThumbSecV")  >= -0.5 )
       {
        //   transform.RotateAround(Vector3.zero, Vector3.up, 90 * Time.deltaTime);
           transform.Rotate(Vector3.down * 75 * Time.deltaTime, Space.World);
       }

		 if (Input.GetAxis("JoyThumbSecH") <= 0.5 )
       {
           transform.Rotate(Vector3.right * 75 * Time.deltaTime);
       }

		 if (Input.GetAxis("JoyThumbSecH") >= -0.5 )
       {
           transform.Rotate(Vector3.left * 75 *Time.deltaTime);
       }

       transform.Translate(new Vector3(0, 0, -Input.GetAxis("JoyTrigger")) * 5 * Time.deltaTime);
    //    transform.eulerAngles = new Vector3(-vCam.y, vCam.x, 0) * 25 * Time.deltaTime;

		///keyboard

		Debug.Log(mouseOrigin.x.ToString() + "-" + mouseOrigin.y.ToString());
		 mouseOrigin = Input.mousePosition;

		if (mouseOrigin.x >= Screen.width - 10 && Input.GetKey(KeyCode.Mouse1))
		{
			// transform.RotateAround(Vector3.zero, -Vector3.up, 90 * Time.deltaTime);
			transform.Rotate(Vector3.up * 75 * Time.deltaTime, Space.World);
		}
		
		if ( mouseOrigin.x <= 0 && Input.GetKey(KeyCode.Mouse1))
		{
			//   transform.RotateAround(Vector3.zero, Vector3.up, 90 * Time.deltaTime);
			transform.Rotate(Vector3.down * 75 * Time.deltaTime, Space.World);
		}
		
		if (mouseOrigin.y <= 0  && Input.GetKey(KeyCode.Mouse1))
       {
           transform.Rotate(Vector3.right * 75 * Time.deltaTime);
       }

		if ( mouseOrigin.y >= Screen.height - 10&& Input.GetKey(KeyCode.Mouse1))
       {
           transform.Rotate(Vector3.left * 75 *Time.deltaTime);
       }

		transform.Translate(new Vector3(0, 0, -Input.GetAxis("Vertical")) * 5 * Time.deltaTime);

	}

}
