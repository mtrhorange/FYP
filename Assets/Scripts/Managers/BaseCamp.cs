using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BaseCamp : MonoBehaviour {

    private bool leavingTown = false;
    private bool fadeOut = true;
    private float fadeTime = 0f;
    private GameObject blackOverlay;

	//Start
	void Start ()
    {
        GameManager.instance.SpawnPlayer();
        blackOverlay = GameObject.Find("Canvas").transform.Find("BlackOverlay").gameObject;
	}
	
	//Update
	void Update ()
    {
        if (leavingTown)
        {
            if (fadeOut)
            {
                fadeTime += Time.deltaTime;

                //Camera Black overlay fade in
                blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, fadeTime);

                if (fadeTime > 1)
                {
                    GameManager.instance.SavePlayers();
                    //load into game (dungeon)
                    SceneManager.LoadScene(1);
                }
            }
        }
	}

    //Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            leavingTown = true;
        }
    }
}
