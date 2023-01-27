using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolDoor : MonoBehaviour
{
    [SerializeField] Transform _door_Left;
    [SerializeField] Transform _door_Right;

    [SerializeField] float _openingTime;
    float _timePassed = 0f;

    bool _enteredTrigger = false;


    private void Update()
    {
        if (!_enteredTrigger || _timePassed > _openingTime)
            return;

        _door_Left.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.y, 0), _timePassed / _openingTime);
        _door_Right.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), _timePassed / _openingTime);
        _timePassed += Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
            _enteredTrigger = true;
    }
}
