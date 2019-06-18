using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    bool move = false;
    Vector3 tarDir;
    Vector3 rollDir;
    bool stuck;
    [HideInInspector]
    public bool jump;
    bool jumped;
    private bool reached_endpoint;
    float timer = 0f;
    bool pressed = false;
    Vector3 endpoint;

    public Transform ballMesh;
    public Text CoinsUI;
    public GameObject Key_UI;
    public GameObject Key_UI_Parent;
    public Text TimerUI;
    private int coins = 0;
    private int keys = 0;
    private int keys_found = 0;
    public Sprite key_pickup;
    private GameObject[] keysGO;

    public GameObject FinishUI;
    public GameObject RetryUI;

    public AudioClip snd_death;
    public AudioClip snd_jump;
    public AudioClip snd_bounce;

    public static bool StageClear;
    bool levelFinished;
    Vector3 starPos;

    private Camera uiCamera;

    private float level_timer;

    private GameManager gm;
    public List<Vector3> BlockList;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        levelFinished = false;
        reached_endpoint = false;
        jumped = false;
        starPos = transform.position;
        timer = 0;

        RetryUI.SetActive(false);
        FinishUI.SetActive(false);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();

        BlockList = new List<Vector3>();
        //BlockList = gm.BlockList.ToList();

        uiCamera = transform.GetChild(5).GetComponent<Camera>();

        level_timer = 90 + 1; // 90 seconds + 1s overtime 

        FindKeys();

        if (BlockList.Count == 0)
        {
            GameObject stage = GameObject.Find("Stage");
            Transform[] t = new Transform[stage.transform.childCount];

            for (int i = 0; i < stage.transform.childCount; i++)
            {
                t[i] = stage.transform.GetChild(i);
            }

            foreach (var g in t)
            {
                if (g.gameObject.tag == "Block")
                {
                    BlockList.Add(g.transform.position);
                }
                
            }
        }
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

    void FindKeys()
    {
        keysGO = GameObject.FindGameObjectsWithTag("Key");

        if (keysGO.Length > 0)
        {
            keys_found = keysGO.Length;

            foreach (var k in keysGO)
            {
                GameObject go = Instantiate(Key_UI, Key_UI_Parent.transform.position, Quaternion.identity);
                go.transform.parent = Key_UI_Parent.transform;
            }
        }
        else
        {
            keys_found = 0;
        }
    }

    void Timer()
    {
        if (!levelFinished)
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
        if (!move && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W) ||
            !move && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.UpArrow))
        {
            if (!stuck)
            {
                tarDir = Camera.main.transform.forward;
                rollDir = Camera.main.transform.forward;
                endpoint = new Vector3(
                               tarDir.x > 0.5f ? 1f : tarDir.x < -0.5f ? -1f : 0f,
                               0f,
                               tarDir.z > 0.5f ? 1f : tarDir.z < -0.5f ? -1f : 0f
                               ) + transform.position; //<-------- LOLZ ^o^

                // check if next move is possible
                for (int i = 0; i < BlockList.Count; i++)
                {
                    var t = transform.position;
                    var n = endpoint;
                    if (BlockList[i] == new Vector3((int)n.x, (int)n.y - 1, (int)n.z)) //new Vector3(t.x + 1, t.y - 1, t.z)
                    {
                        pressed = true;
                        move = true;
                        //Debug.Log("MATCH");
                    }
                }

                //Debug.Log("endpoint: " + new Vector3(endpoint.x, (int)endpoint.y - 1, endpoint.z));
            }
        }

        if (!move && Input.GetKeyDown(KeyCode.Space))
        {
            if (!jump && !jumped)
            {
                pressed = true;
                move = true;
                jumped = true;
                tarDir = Camera.main.transform.forward;
                rollDir = Camera.main.transform.forward;
                endpoint = new Vector3(
                               tarDir.x > 0.5f ? 2f : tarDir.x < -0.5f ? -2f : 0f,
                               0,
                               tarDir.z > 0.5f ? 2f : tarDir.z < -0.5f ? -2f : 0f
                           ) + transform.position; //<-------- LOLZ ^o^


                anim.Play("Jump");

                //Debug.Log("jump endpoint: " + new Vector3(endpoint.x, (int)endpoint.y, endpoint.z));

                GameObject soundObj = new GameObject("JumpSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = snd_jump;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!jump && !jumped)
            {
                //pressed = true;
                //move = true;
                jumped = true;

                anim.Play("Jump");

                //Debug.Log("jump endpoint: " + new Vector3(endpoint.x, (int)endpoint.y, endpoint.z));

                GameObject soundObj = new GameObject("JumpSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = snd_jump;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);
            }
        }*/
    }

    public void AddCoins()
    {
        coins += 1;
    }

    public void AddKey()
    {
        Key_UI_Parent.transform.GetChild(keys).GetComponent<Image>().sprite = key_pickup;
        keys += 1;
        //Debug.Log(Key_UI_Parent.transform.childCount);
    }

    public void LevelFinished(Transform obj)
    {
        if (!gm.getInEditor && keys == keys_found && reached_endpoint && !levelFinished)
        {
            GameObject soundObj = new GameObject("FinishSound");
            soundObj.AddComponent<AudioSource>();
            soundObj.GetComponent<AudioSource>().playOnAwake = true;
            soundObj.GetComponent<AudioSource>().spread = 360f;
            soundObj.GetComponent<AudioSource>().clip = obj.GetComponent<ActorAction>().Snd;
            soundObj.GetComponent<AudioSource>().Play();
            Destroy(soundObj, 3f);

            levelFinished = true;
            FinishUI.SetActive(true); //GUI
        }
        else if (gm.getInEditor && keys == keys_found && reached_endpoint && !levelFinished)
        {
            GameObject soundObj = new GameObject("FinishSound");
            soundObj.AddComponent<AudioSource>();
            soundObj.GetComponent<AudioSource>().playOnAwake = true;
            soundObj.GetComponent<AudioSource>().spread = 360f;
            soundObj.GetComponent<AudioSource>().clip = obj.GetComponent<ActorAction>().Snd;
            soundObj.GetComponent<AudioSource>().Play();
            Destroy(soundObj, 3f);

            gm.setEditState = GameManager.EditState.Edit;
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
            //Debug.Log("true jump ");
        }
        else
        {
            jump = false;
            //Debug.Log("false jump");
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
            reached_endpoint = true;
            //Debug.Log("Reached");

            if (jumped)
            {
                //transform.position = new Vector3(endpoint.x, endpoint.y, endpoint.z);
                //transform.position = new Vector3(transform.position.x, (int)transform.position.y, transform.position.z);
                jumped = false;
                //Debug.Log("Jumped");
            }
        }
        else
        {
            reached_endpoint = false;
        }
    }

    void FellOff()
    {
        if (timer >= 0)
            timer += Time.deltaTime;

        jumped = true;

        if (timer > 3)
        {
            //Destroy(gameObject);
            //transform.position = starPos;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            timer = -1;
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
            jumped = false;
        }
    }

    public void BounceSound()
    {
        GameObject soundObj = new GameObject("BounceSound");
        soundObj.AddComponent<AudioSource>();
        soundObj.GetComponent<AudioSource>().playOnAwake = true;
        soundObj.GetComponent<AudioSource>().spread = 360f;
        soundObj.GetComponent<AudioSource>().clip = snd_bounce;
        soundObj.GetComponent<AudioSource>().Play();
        Destroy(soundObj, 3f);
    }
}