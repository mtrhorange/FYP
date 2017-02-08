using UnityEngine;
using System.Collections;

public enum sounds
{
    bite,
    bite2,
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
    explosion,
    iceBall,
    iceSpike,
    iceBlast,
    firePillar,
    lightning,
    levelup,
    healing,
    dragonFire,
    arrowShoot,
    roar,
    click,
    stomp

}
public class SFXManager : MonoBehaviour
{

    public static SFXManager instance;

    private AudioSource sfx;

    public float sfxValue = 1;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (instance && instance != this.gameObject)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("SFX")){
            sfxValue = 1f;
        }
        else
        {
            sfxValue = PlayerPrefs.GetFloat("SFX");
        }

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
            case sounds.click:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/click"));
                break;
            case sounds.bite:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/bite"));
                break;
            case sounds.stomp:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/stomp"));
                break;
            case sounds.roar:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/roar"));
                break;
            case sounds.bite2:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/bite2"));
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
            case sounds.iceBall:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/iceBall"));
                break;
            case sounds.iceBlast:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/iceBlast"));
                break;
            case sounds.iceSpike:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/iceSpike"));
                break;
            case sounds.firePillar:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/firePillar"));
                break;
            case sounds.lightning:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/lightning"));
                break;
            case sounds.levelup:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/levelup"));
                break;
            case sounds.healing:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/healing"));
                break;
            case sounds.dragonFire:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/dragonFire"));
                break;
            case sounds.arrowShoot:
                sfx.PlayOneShot((AudioClip)Resources.Load("Sound/arrowShoot"));
                break;
                
        }
    }

    public void setVolume(float v)
    {
        GetComponent<AudioSource>().volume = v;
    }

    public void setSFX(float f)
    {
        sfxValue = f;
        setVolume(f);
    }
}
