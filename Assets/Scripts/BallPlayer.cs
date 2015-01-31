using UnityEngine;
using System.Collections;

public class BallPlayer : MonoBehaviour {

    bool move = false;
    bool mfwd, mbwd, mleft, mright;
    float tarX;
    float tarZ;
    float rollx;
    float rollz;

    bool respawn;

    float timer = 0f;

    bool pressed = false;

    public Transform ballMesh;

	// Use this for initialization
	void Start () {
        respawn = true;
	}
	
	// Update is called once per frame
	void Update () {

      //  print(timer);

        if (GameManager.state == GameManager.GameState.Editor)
        {
            Destroy(gameObject);
            Debug.Log("I can live with an editor");
        }

        Move();
        ColDetection();

        if (!move)
        {
           Gravity();
        }
	
	}

    void Gravity()
    {
           RaycastHit hit;

           if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
           {
              // transform.eulerAngles = new Vector3(0, 0, 0);
             //  transform.rotation = Quaternion.identity;
               transform.Translate(new Vector3(0, -5, 0) * Time.deltaTime);
               pressed = true;
               KillMe();
           }
            else
           {
               timer = 0f;
           }
        
    }


    void ColDetection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f))
        {
            mfwd = true;
        }
        else
        {        
            mfwd = false;
        }

        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1f))
        {
            mbwd = true;
        }
        else
        {
            mbwd = false;
        }

        if (Physics.Raycast(transform.position, Vector3.left, out hit, 1f))
        {
            mleft = true;
        }
        else
        {
            mleft = false;
        }
        

        if (Physics.Raycast(transform.position, Vector3.right, out hit, 1f))
        {
            mright = true;
        }
        else
        {
            mright = false;
        }
    }


    void Move()
    {

        float tarY = transform.position.y;

        Vector3 endpoint = new Vector3(tarX, tarY, tarZ);

         if(respawn)
        {
            respawn = false;
            tarX = transform.position.x;
            tarZ = transform.position.z;
        }

        //Debug.Log("move: " + move + " : " + endpoint);

        if (move)
        {

            ballMesh.transform.Rotate(new Vector3(rollz, 0, -rollx) * 200 * Time.deltaTime, Space.World);
            transform.position = Vector3.MoveTowards(transform.position, endpoint, 5 * Time.deltaTime);
        }

        if (endpoint == transform.position)
        {
            move = false;
            rollx = 0f;
            rollz = 0f;
            //Debug.Log("Reached");
        }

        if (Input.GetAxis("JoyVertical") == 0f && Input.GetAxis("JoyHorizontal") == 0f)
        {
            pressed = false;
        }

        if (CameraED.camPos == CameraED.CamPos.fwd && Input.GetAxis("JoyVertical") == 1f && !move && !pressed)
        {
           
            if (!mfwd)
            {
                pressed = true;
                move = true;
                tarZ += 1;
                rollz = 1;
            }

        }

        if (CameraED.camPos == CameraED.CamPos.back && Input.GetAxis("JoyVertical") == 1f && !move && !pressed)
        {
            if (!mbwd)
            {
                pressed = true;
                move = true;
                tarZ -= 1;
                rollz =  -1;
            }
        }

        if (CameraED.camPos == CameraED.CamPos.right && Input.GetAxis("JoyVertical") == 1f && !move && !pressed)
        {
            if (!mright)
            {
                pressed = true;
                move = true;
                tarX += 1;
                rollx = 1;
            }
        }

        if (CameraED.camPos == CameraED.CamPos.left && Input.GetAxis("JoyVertical") == 1f && !move && !pressed)
        {
            if (!mleft)
            {
                pressed = true;
                move = true;
                tarX -= 1;
                rollx = -1;
            }
        }


        //backwards
        if (CameraED.camPos == CameraED.CamPos.fwd && Input.GetAxis("JoyVertical") == -1f && !move && !pressed)
        {

            if (!mbwd)
            {
                pressed = true;
                move = true;
                tarZ  -= 1;
                rollz = -1;
            }

        }

        if (CameraED.camPos == CameraED.CamPos.back && Input.GetAxis("JoyVertical") == -1f && !move && !pressed)
        {
            if (!mfwd)
            {
                pressed = true;
                move = true;
                tarZ += 1;
                rollz = 1;
            }
        }

        if (CameraED.camPos == CameraED.CamPos.right && Input.GetAxis("JoyVertical") == -1f && !move && !pressed)
        {
            if (!mleft)
            {
                pressed = true;
                move = true;
                tarX -= 1;
                rollx = -1;
            }
        }

        if (CameraED.camPos == CameraED.CamPos.left && Input.GetAxis("JoyVertical") == -1f && !move && !pressed)
        {
            if (!mright)
            {
                pressed = true;
                move = true;
                tarX += 1;
                rollx = 1;
            }
        }
    }

    void KillMe()
    {
        timer += 1 * Time.deltaTime;

        if(timer > 3)
        {
            Destroy(gameObject);
        }
    }
}
