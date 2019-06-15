using UnityEngine;
using System.Collections;

public class ActorAction : MonoBehaviour {

    public AudioClip Snd;
    public AAction aaction;
    private bool collected = false;
    private GameManager gm;

    public enum AAction
    {
        Finish,
        Coin,
        Key,
    };

	// Use this for initialization
	void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gm.getEditState == GameManager.EditState.Edit)
        {
            collected = false;
        }

        transform.GetChild(0).gameObject.SetActive(!collected);
    }

    void OnTriggerEnter(Collider c)
    {
        if(aaction == AAction.Coin)
        {
            if (c.gameObject.tag.Equals("Player") && !collected)
            {
                GameObject soundObj = new GameObject("CoinSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = Snd;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);

                c.gameObject.GetComponent<Player>().AddCoins();
                //Destroy(gameObject);
                collected = true;
            }
        }

        if (aaction == AAction.Key)
        {
            if (c.gameObject.tag.Equals("Player") && !collected)
            {
                GameObject soundObj = new GameObject("KeySound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = Snd;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);

                c.gameObject.GetComponent<Player>().AddKey();
                collected = true; //Destroy(gameObject);
            }
        }

        if (aaction == AAction.Finish)
        {
            if (c.gameObject.tag.Equals("Player"))
            {
                c.gameObject.GetComponent<Player>().LevelFinished(transform); //Level Done !
            }
        }
    }
}
