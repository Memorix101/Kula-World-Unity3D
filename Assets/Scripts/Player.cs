using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    bool move = false;
    Vector3 tarDir;
    Vector3 rollDir;
    bool stuck;
    float timer = 0f;
    bool pressed = false;
    Vector3 endpoint;

    public Transform ballMesh;
    public Text CoinsUI;
    private int coins = 0;

    public GameObject FinishUI;
    public GameObject RetryUI;

    public AudioClip snd_death;

    public static bool StageClear;
    bool levelFinished;
    Vector3 starPos;

    public GameManager gm;

    // Use this for initialization
    void Start()
    {
        levelFinished = false;
        starPos = transform.position;

        RetryUI.SetActive(false);
        FinishUI.SetActive(false);

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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

        //Debug.LogError("stuck" + stuck.ToString());
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
                endpoint = new Vector3(tarDir.x > 0.5f ? 1 : tarDir.x < -0.5f ? -1 : 0, 0, tarDir.z > 0.5f ? 1 : tarDir.z < -0.5f ? -1 : 0) + transform.position; //<-------- LOLZ ^o^
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
            Debug.Log("Reached");
        }
    }

    void KillMe()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            //Destroy(gameObject);
            //transform.position = starPos;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            timer = 0;

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
}
