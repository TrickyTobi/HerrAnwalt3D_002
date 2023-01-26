using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrundgesetzAudio : MonoBehaviour
{
    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;
    PlayerStateMachine _player;
    AudioSource _audioSource;
    bool _alreadyHit;


    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _player = GetComponentInParent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (!_player.IsAttacking)
            _alreadyHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Teacher") || other.CompareTag("PostProcess"))
            return;

        if (!_player.IsAttacking)
            return;

        if (_alreadyHit)
            return;

        _alreadyHit = true;
        _audioSource.PlayOneShot(_soundEffectSO.Punch(), _optionsSO.AttornyHitVolume);
    }


}
