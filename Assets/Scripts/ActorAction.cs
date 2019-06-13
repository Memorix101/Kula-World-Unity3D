using UnityEngine;
using System.Collections;

public class ActorAction : MonoBehaviour {

    public AudioClip Snd;
    public AAction aaction;

    public enum AAction
    {
        Finish,
        Coin,
    };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if(aaction == AAction.Coin)
        {
            if (c.gameObject.tag.Equals("Player"))
            {
                GameObject soundObj = new GameObject("CoinSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = Snd;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);

                c.gameObject.GetComponent<Player>().AddCoins();
                Destroy(gameObject);
            }
        }

        if (aaction == AAction.Finish)
        {
            if (c.gameObject.tag.Equals("Player"))
            {
                GameObject soundObj = new GameObject("FinishSound");
                soundObj.AddComponent<AudioSource>();
                soundObj.GetComponent<AudioSource>().playOnAwake = true;
                soundObj.GetComponent<AudioSource>().spread = 360f;
                soundObj.GetComponent<AudioSource>().clip = Snd;
                soundObj.GetComponent<AudioSource>().Play();
                Destroy(soundObj, 3f);

                c.gameObject.GetComponent<Player>().LevelFinished(); //Level Done !
            }
        }
    }
}
