using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] PlayerStatsSO _playerStatsSO;
    public int _enemyHealth;
    [SerializeField] GameObject _paragraphs;
    [SerializeField] int _paragraphAmount;

    [SerializeField] float _explosionForce = 600f;
    [SerializeField] float _explosionRadius = 0.4f;
    [SerializeField] float _explosionUpward = 0.6f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Got Hit by" + other.tag);

        if (other.CompareTag("BookWeapon") && other.GetComponent<PlayerStateMachine>().IsAttacking) // Guckt ob es mit einem Buch geschlagen wurde. Muss eventuell angepasst werden bei mehreren Waffen
        {
            _enemyHealth--;

            if (_enemyHealth <= 0)
            {
                Destroy(gameObject);

                for (int i = 0; i < _paragraphAmount; i++)
                {
                    float randome = Random.Range(-0.4f, 0.4f);

                    GameObject instance = Instantiate(_paragraphs, transform.position + new Vector3(randome, randome, randome), Quaternion.identity);
                    instance.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionUpward);
                }
            }
        }
    }





}
