using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFenceSound : MonoBehaviour
{

    [SerializeField] SoundEffectSO _sound;
    [SerializeField] OptionsSO _options;
    AudioSource _audioUtilityPlayer;

    private void Start()
    {
        _audioUtilityPlayer = gameObject.AddComponent<AudioSource>();
        _audioUtilityPlayer.spatialBlend = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Player"))
            return;
        Debug.Log("There");
        _audioUtilityPlayer.PlayOneShot(_sound.Fence(), _options.FenceHitVolume);
    }
}
