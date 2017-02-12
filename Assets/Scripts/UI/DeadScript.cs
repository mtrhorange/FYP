using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeadScript : MonoBehaviour {

    public Image black;
    public Image cover;
    public GameObject de;
    public bool fadedIn;
    public bool ded;

	// Use this for initialization
	void Start () {
        
	}
	
    void Awake()
    {
        ded = false;
        fadedIn = false;
    }

	// Update is called once per frame
	void Update () {

        if (ded)
        {
            
            if (black.color.a < 1f && !fadedIn)
            {
                black.color = new Color(0, 0, 0, black.color.a + Time.deltaTime * 0.5f);
                cover.color = new Color(0, 0, 0, black.color.a + Time.deltaTime * 0.5f);
                if (black.color.a > 1f)
                {
                    fadedIn = true;
                }
            }
            else
            {
                de.SetActive(true);

                if (cover)
                {
                    if (cover.color.a < 0f)
                    {
                        Destroy(cover.gameObject);
                    }
                    else
                    {
                        cover.color = new Color(0, 0, 0, cover.color.a - Time.deltaTime * 0.5f);
                    }
                }
            }
        }

    }

    public void ReportDeath()
    {
        if (GameManager.instance.twoPlayers)
        {
            if (GameManager.instance.player1.isDead && GameManager.instance.player2.isDead)
            {
                ded = true;
            }
        }
        else
        {
            if (GameManager.instance.player1.isDead)
            {
                ded = true;
            }
        }
    }

    public void ReturnToTown()
    {
        //if (GameManager.instance.twoPlayers)
        //{
        //    GameManager.instance.player1.ReceiveHeal(10f);
        //    GameManager.instance.player2.ReceiveHeal(10f);
        //}
        //else
        //{
        //    GameManager.instance.player1.ReceiveHeal(10f);
        //}
		if (GameManager.instance.player1.isDead)
			GameManager.instance.player1.PlayerRevive ();
		if (GameManager.instance.twoPlayers && GameManager.instance.player2.isDead)
			GameManager.instance.player2.PlayerRevive ();
        //check if bgm is normal, if not change to normal
        BGMManager.instance.checkNormal();
		LevelManager.instance.LoadGame ();
    }

    public void ReturnToMainMerneor()
    {
        //if (GameManager.instance.twoPlayers)
        //{
        //    GameManager.instance.player1.ReceiveHeal(10f);
        //    GameManager.instance.player2.ReceiveHeal(10f);
        //}
        //else
        //{
        //    GameManager.instance.player1.ReceiveHeal(10f);
        //}
		if (GameManager.instance.player1.isDead)
			GameManager.instance.player1.PlayerRevive ();
		if (GameManager.instance.twoPlayers && GameManager.instance.player2.isDead)
			GameManager.instance.player2.PlayerRevive ();
        //check if bgm is normal, if not change to normal
        BGMManager.instance.checkNormal();
        LevelManager.instance.LoadMainMenu();
    }
}
