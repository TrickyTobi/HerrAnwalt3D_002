using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDamage : MonoBehaviour
{
    [SerializeField] EventChannelSO _eventChannelSO;
    [SerializeField] SoundEffectSO _soundEffectSO;
    [SerializeField] OptionsSO _optionsSO;
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.PlayOneShot(_soundEffectSO.Fence(), _optionsSO.FenceHitVolume);

        if (collision.gameObject.CompareTag("Player"))
        {
            _eventChannelSO.LoseLife();
            return;
        }

        if (collision.gameObject.CompareTag("Teacher"))
        {
            GetComponent<TeacherLogic>().Health--;
        }
    }
}

