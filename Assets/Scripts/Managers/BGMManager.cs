﻿using UnityEngine;
using System.Collections;
public class BGMManager : MonoBehaviour
{

    public static BGMManager instance;

    public float bgmValue = 1;
    public AudioClip normal;
    public AudioClip boss;
    void Awake()
    {
        if (GameObject.Find("BGMManager") != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
	// Use this for initialization
    void Start()
    {
        if (!PlayerPrefs.HasKey("BGM"))
        {
            bgmValue = 1f;
        }
        else
        {
            bgmValue = PlayerPrefs.GetFloat("BGM");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setVolume(float v)
    {
        GetComponent<AudioSource>().volume = v;
    }

    public void setBGM(float f)
    {
        bgmValue =f;
        setVolume(bgmValue);
    }

    public void changeToBoss()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = boss;
        GetComponent<AudioSource>().Play();
    }

    public void changeToNormal()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = normal;
        GetComponent<AudioSource>().Play();
    }

    public void checkNormal()
    {
        if (GetComponent<AudioSource>().clip != normal)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = normal;
            GetComponent<AudioSource>().Play();
        }
    }
}
