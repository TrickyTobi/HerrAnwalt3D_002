using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDestroyable : MonoBehaviour
{
    [SerializeField] GameObject _lock;
    [SerializeField] GameObject _doorCell;
    [SerializeField] int _doorLife;

    [SerializeField] PlayerStatsSO _playerStatsSO;

    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;

    AudioSource _audioSource;

    [SerializeField] float _explosionForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionUpward;


    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {


        if (!other.CompareTag("BookWeapon"))
            return;

        PlayerStateMachine _player = other.GetComponentInParent<PlayerStateMachine>();

        if (!_player.IsAttacking)
            return;

        if (_player.HitTarget)
            return;

        _player.HitTarget = true;
        _doorLife--;
        _audioSource.PlayOneShot(_soundEffectSO.MetalGateHit(), _optionsSO.AttornyHitVolume);


        if (_doorLife > 0)
            return;


        // change sound, for  when gate is destroyed
        _audioSource.PlayOneShot(_soundEffectSO.MetalGateHit(), _optionsSO.AttornyHitVolume);
        _doorCell.SetActive(true);
        this.gameObject.SetActive(false);
        Rigidbody[] rbs = _doorCell.GetComponentsInChildren<Rigidbody>();
        Rigidbody lockRB = _lock.GetComponent<Rigidbody>();

        lockRB.useGravity = true;
        lockRB.isKinematic = false;

        foreach (Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(_explosionForce, other.transform.position - new Vector3(0.5f, 0, 0), _explosionRadius, _explosionUpward);
        }



    }
}
