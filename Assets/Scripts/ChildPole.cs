using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPole : MonoBehaviour
{
    [SerializeField] GameObject _lockBody;
    [SerializeField] GameObject _lockU;
    [SerializeField] GameObject _ropeComplete;
    [SerializeField] GameObject _ropeCell;

    [SerializeField] PlayerStatsSO _playerStatsSO;
    [SerializeField] SoundEffectSO _sound;
    [SerializeField] OptionsSO _option;

    [SerializeField] float _explosionForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionUpward;

    bool _freed = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BookWeapon") || !other.GetComponentInParent<PlayerStateMachine>().IsAttacking || _freed)
            return;

        _freed = true;

        _playerStatsSO.FreedChilds++;

        this.gameObject.AddComponent<AudioSource>().PlayOneShot(_sound.LockDestroy(), _option.LockDestroyVolume);

        _ropeCell.SetActive(true);
        _ropeComplete.SetActive(false);

        _lockBody.GetComponent<Rigidbody>().isKinematic = false;
        _lockU.GetComponent<Rigidbody>().isKinematic = false;

        Rigidbody[] rbs = _ropeCell.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(_explosionForce, other.transform.position - new Vector3(0.5f, 0, 0), _explosionRadius, _explosionUpward);
        }

        GetComponentInChildren<ChildLogic>().Freed();
    }
}
