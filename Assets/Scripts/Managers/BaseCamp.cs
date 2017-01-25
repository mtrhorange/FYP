using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BaseCamp : MonoBehaviour {

    public bool leavingTown = false;
    public float fadeTime = 0f;
    public GameObject blackOverlay;

	//Start
	void Start()
    {
        GameManager.instance.SpawnPlayer();
	}
	
    //Awake
    void Awake()
    {
        leavingTown = false;
        fadeTime = 0f;
        blackOverlay = GameObject.Find("Canvas").transform.Find("BlackOverlay").gameObject;
    }

	//Update
	void Update()
    {
        if (leavingTown)
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

    //Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            leavingTown = true;
        }
    }
}
