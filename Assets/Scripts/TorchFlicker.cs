using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TorchFlicker : MonoBehaviour {

    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;

    public float minRange = 0.25f;
    public float maxRange = 0.5f;
   // public new Light light;
    private Light[] lights;
    private float[] random;

    void Start()
    {
        lights = gameObject.GetComponentsInChildren<Light>();

         random = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            random[i] = Random.Range(0.0f, 65535.0f);
        }
    }

    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            float noise = Mathf.PerlinNoise(random[i], Time.time);
            lights[i].intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
            lights[i].range = Mathf.Lerp(minRange, maxRange, noise);
        }
    }
}
