using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAudio : MonoBehaviour
{
    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;
    TeacherLogic _logic;
    AudioSource _audioSource;
    bool _alreadyHit = false;


    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _logic = GetComponentInParent<TeacherLogic>();

        _alreadyHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyHit)
            return;

        if (!other.CompareTag("Player"))
            return;



        if (!_logic.IsAttacking || _logic.AlreadyHit)
            return;
        _alreadyHit = true;
        _audioSource.PlayOneShot(_soundEffectSO.Punch(), _optionsSO.TeacherHitVolume);
    }
}
