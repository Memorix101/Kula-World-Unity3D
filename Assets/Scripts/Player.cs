using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    bool move = false;
    Vector3 tarDir;
    Vector3 rollDir;
    bool stuck;
    bool jump;
    float timer = 0f;
    bool pressed = false;
    Vector3 endpoint;

    public Transform ballMesh;
    public Text CoinsUI;
    public Text TimerUI;
    private int coins = 0;

    public GameObject FinishUI;
    public GameObject RetryUI;

    public AudioClip snd_death;
    public AudioClip snd_jump;

    public static bool StageClear;
    bool levelFinished;
    Vector3 starPos;

    private Camera uiCamera;

    private float level_timer;

    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        levelFinished = false;
        starPos = transform.position;

        RetryUI.SetActive(false);
        FinishUI.SetActive(false);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        uiCamera = transform.GetChild(5).GetComponent<Camera>();

        level_timer = 90 + 1; // 90 seconds + 1s overtime 
    }

    // Update is called once per frame
    void Update()
    {
        StageClear = levelFinished;

        if (GameManager.editState == GameManager.EditState.Edit)
        {
            Destroy(gameObject); // Player will be destroy if not in play mode
        }

        if (!move)
        {
            ColDetection();
        }

        if (stuck)
        {
            Gravity();
        }

        Move();

        CoinsUI.text = coins.ToString();

        Timer();

        //Debug.LogError("stuck" + stuck.ToString());
    }

    void Timer()
    {
        level_timer -= Time.deltaTime;

        if (level_timer > 10)
        {
            TimerUI.color = Color.green;
        }
        else
        {
            TimerUI.color = Color.red;
        }

        if (level_timer < 0)
        {
            level_timer = 0;
            KillMe();
        }

        TimerUI.text = string.Format("{0}", (int)level_timer);
        uiCamera.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
    }

    void InputMove()
    {
        if (!move && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) ||
            !move && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            if (!stuck)
            {
                pressed = true;
                move = true;
                tarDir = Camera.main.transform.forward;
                rollDir = Camera.main.transform.forward;
                endpoint = new Vector3(
                               tarDir.x > 0.5f ? 1f : tarDir.x < -0.5f ? -1f : 0f, 
                               0f, 
                               tarDir.z > 0.5f ? 1f : tarDir.z < -0.5f ? -1f : 0f
                               ) + transform.position; //<-------- LOLZ ^o^
            }
        }

        if (!move && Input.GetKeyDown(KeyCode.Space))
        {
            if (!jump)
            {
                pressed = true;
                move = true;
                tarDir = Camera.main.transform.forward;
                rollDir = Camera.main.transform.forward;
                endpoint = new Vector3(
                               tarDir.x > 0.5f ? 2f : tarDir.x < -0.5f ? -2f : 0f,
                               Mathf.PingPong(Time.time, 1f),
                               tarDir.z > 0.5f ? 2f : tarDir.z < -0.5f ? -2f : 0f
                           ) + transform.position; //<-------- LOLZ ^o^

                GameObject soundObj = new GameObject("JumpSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = snd_jump;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);
            }
        }
    }

    public void AddCoins()
    {
        coins += 1;
    }

    public void LevelFinished()
    {
        if (!gm.getInEditor)
        {
            levelFinished = true;
            FinishUI.SetActive(true); //GUI
        }
    }

    void Gravity()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            move = false;
            transform.Translate(new Vector3(0, -5, 0) * Time.deltaTime);
            pressed = true;
            FellOff();
        }
        else
        {
            timer = 0f;
        }
    }

    void ColDetection()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 1f))
        {
            stuck = true;
        }
        else
        {
            stuck = false;
        }

        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            stuck = true;
        }

        if (Physics.Raycast(transform.position, uiCamera.transform.forward, out hit, 3f))
        {
            Debug.DrawRay(transform.position, uiCamera.transform.forward, Color.red);
            jump = true;
            Debug.Log("true jump ");
        }
        else
        {
            jump = false;
            Debug.Log("false jump");
        }
    }

    void Move()
    {
        float tarY = transform.position.y;

        if (!levelFinished)
        {
            InputMove();
        }

        //Debug.Log("move: " + move + " : " + endpoint);

        if (move)
        {
            ballMesh.transform.Rotate(new Vector3(rollDir.z, 0, -rollDir.x) * 200 * Time.deltaTime, Space.World);
            transform.position = Vector3.MoveTowards(transform.position, endpoint, 5 * Time.deltaTime);
        }

        if (endpoint.x == transform.position.x && endpoint.z == transform.position.z)
        {
            move = false;
            pressed = false;
            rollDir = Vector3.zero;
            //Debug.Log("Reached");
        }
    }

    void FellOff()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            //Destroy(gameObject);
            //transform.position = starPos;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            timer = 0;
            KillMe();
        }
    }

    void KillMe()
    {
        if (!gm.getInEditor)
        {
            RetryUI.SetActive(true);
            levelFinished = true;

            GameObject soundObj = new GameObject("DeathSound");
            soundObj.AddComponent<AudioSource>();
            soundObj.GetComponent<AudioSource>().playOnAwake = true;
            soundObj.GetComponent<AudioSource>().spread = 360f;
            soundObj.GetComponent<AudioSource>().clip = snd_death;
            soundObj.GetComponent<AudioSource>().Play();
            Destroy(soundObj, 3f);
        }
        else
        {
            transform.position = starPos;
        }
    }
}