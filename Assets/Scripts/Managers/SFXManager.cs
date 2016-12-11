using UnityEngine;
using System.Collections;

public enum sounds
{
    bite,
    cloth,
    door,
    giant,
    ogre,
    shadeLong,
    shadeShort,
    slime,
    slimeDie,
    swing,
    unsheathe,
    woodHit,
    zombie,
    explosion
    
}
public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    private AudioSource sfx;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        sfx = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //testing
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    playSFX(sounds.bite);
        //}
    }

    public void playSFX(sounds s)
    {

        switch (s)
        {
            case sounds.bite:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/bite"));
                break;
            case sounds.cloth:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/cloth"));
                break;
            case sounds.door:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/door"));
                break;
            case sounds.explosion:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/Explosion"));
                break;
            case sounds.giant:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/giant"));
                break;
            case sounds.ogre:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/ogre"));
                break;
            case sounds.shadeLong:
                sfx.PlayOneShot((AudioClip) Resources.Load("Sound/shadeLong"));
                break;
            case sounds.shadeShort:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/shadeShort"));
                break;
            case sounds.slime:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/slime"));
                break;
            case sounds.slimeDie:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/slimeDie"));
                break;
            case sounds.swing:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/swing"));
                break;
            case sounds.unsheathe:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/unsheathe"));
                break;
            case sounds.woodHit:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/woodHit"));
                break;
            case sounds.zombie:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/zombie"));
                break;
        }
    }

    public void setVolume(float v)
    {
        sfx.volume = v;
    }
}
