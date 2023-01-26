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

    [SerializeField] float _explosionForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionUpward;



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BookWeapon") || !other.GetComponentInParent<PlayerStateMachine>().IsAttacking)
            return;

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
