using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeadScript : MonoBehaviour {

    public Image black;
    public Image cover;
    public GameObject de;
    private bool fadedIn;
    private bool ded;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.twoPlayers)
        {
            if(GameManager.instance.player1.Health <= 0 && GameManager.instance.player2.Health <= 0)
            {
                ded = true;
            }
        }
        else
        {
            if (GameManager.instance.player1.Health <= 0)
            {
                ded = true;
            }
        }


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

    public void ReturnToTown()
    {
        LevelManager.instance.LoadGame();
    }
}
