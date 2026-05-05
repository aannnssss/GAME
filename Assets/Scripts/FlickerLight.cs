using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light lamp;
    public float minIntensity = 0.5f;
    public float maxIntensity = 200f;
    public float speed = 10f;

    void Update()
    {
        lamp.intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            Mathf.PerlinNoise(Time.time * speed, 0f)
        );
    }
}