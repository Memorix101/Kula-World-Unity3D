using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	
	private float timer;
	private float time = 5f;
	public  string LevelToLoad  = "Level";
	public GameObject logo;
	
	void Start(){
		
		logo.SetActive(false);

	}
	
	void Update(){
		
		timer += Time.deltaTime;

		if(timer >= time){
            DisplayScene();
		}


        if (timer >= 0.1f){
			logo.SetActive(true);
		}
        else
            logo.SetActive(false);

    }
	
	void OnGUI () {
		
		Color textureColor = logo.GetComponent<Image>().material.color;
		textureColor.a = 0;

		if(timer >= 0.0f){
			textureColor.a = Time.timeSinceLevelLoad * 0.7f;
		}
		
		if(timer >= 2.5f){
			textureColor.a =  2.5f -0.7f * Time.timeSinceLevelLoad;
		}

        //logo.GetComponent<Image>().material.color = textureColor;
        logo.GetComponent<Image>().canvasRenderer.SetColor(textureColor);
        //print("Timer: " + timer + " Alpha: " + textureColor);

    }
	
	
	
	void DisplayScene(){

		SceneManager.LoadScene( LevelToLoad );
		//print ("SplashScreen Over!");
		
	}
	

}
