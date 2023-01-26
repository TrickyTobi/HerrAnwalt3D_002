using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DestroyableItem : MonoBehaviour
{
    [SerializeField] int _itemLife;
    [SerializeField] GameObject destroyedPrefab;
    bool _broken = false;

    [SerializeField] float _explosionPower;
    [SerializeField] float _explosionRadius;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BookWeapon"))
        {
            Debug.Log("Hit Destroyable");

            _itemLife--;

            if (_itemLife <= 0)
            {
                if (destroyedPrefab != null && !_broken) //muss erst noch richtig getestet werden. Grobe implementierung
                {
                    _broken = true;
                    var replacement = Instantiate(destroyedPrefab, transform.position, transform.rotation);

                    var rbs = replacement.GetComponentsInChildren<Rigidbody>();

                    foreach (var rb in rbs)
                    {
                        if (!rb.gameObject.CompareTag("Destroyed"))
                            return;


                        rb.AddExplosionForce(_explosionPower, other.transform.position, _explosionRadius);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}


