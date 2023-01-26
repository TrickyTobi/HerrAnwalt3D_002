using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    public Light spot1;

    public float minTime;
    public float maxTime;
    public float Timer;

    public AudioSource flicklerSound;

    void Start()
    {
        Timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        FlickerLight();
    }

    public void FlickerLight()
    {
        if (Timer > 0)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            flicklerSound.Play();
            StartCoroutine(changeLight());
            Timer = Random.Range(minTime, maxTime);
        }
    }

    IEnumerator changeLight()
    {
        spot1.enabled = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        spot1.enabled = true;
    }

}
