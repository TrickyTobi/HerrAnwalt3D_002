using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootSoundTrigger : MonoBehaviour
{
    AudioSource _audioUtiletyPlayer;
    [SerializeField] SoundEffectSO _soundEffectSO;
    [SerializeField] OptionsSO _optionsSO;

    [SerializeField] float _stepSound;

    float _stepTime;
    float _stepInterval = 0.5f;

    private void Start()
    {
        _audioUtiletyPlayer = gameObject.AddComponent<AudioSource>();
        _audioUtiletyPlayer.spatialBlend = 1;
        _audioUtiletyPlayer.maxDistance = 15f;

    }

    private void Update()
    {
        _audioUtiletyPlayer.volume = _optionsSO.StepVolume;
        _stepTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer != LayerMask.NameToLayer("Terrain"))
            return;

        if (_stepTime < _stepInterval)
            return;

        _stepTime = 0;
        _audioUtiletyPlayer.PlayOneShot(_soundEffectSO.StepSound(other.gameObject));
    }
}
