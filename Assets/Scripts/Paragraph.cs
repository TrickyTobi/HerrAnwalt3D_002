using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paragraph : MonoBehaviour
{
    [SerializeField] EventChannelSO _event;
    [SerializeField] PlayerStatsSO _playerStatsSO;

    [SerializeField] ParticleSystem _particlesCollected; //blueSparkle
    [SerializeField] ParticleSystem _particlesTimeOut; //redSparkle


    bool _collectable = false;





    private void Start()
    {
        StartCoroutine(CollectableTimer());

        StartCoroutine(TimeOutTimer());

    }




    private void OnTriggerEnter(Collider other)
    {
        if (!_collectable)
            return;


        if (!other.gameObject.transform.parent.CompareTag("Player") || _playerStatsSO.PlayerHealth >= _playerStatsSO.PlayerMaxHealth || !_collectable)
            return;
        _event.AddParagraph();
        var effect = Instantiate(_particlesCollected, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        effect.Play();
        _collectable = false;
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 4f); //Zerstört den Rest des Gameobject nach 3 sekunden
    }

    IEnumerator TimeOutTimer()
    {
        yield return new WaitForSeconds(10);

        var effect = Instantiate(_particlesTimeOut, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        effect.Play();
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 4f); //Zerstört den Rest des Gameobject nach 3 sekunden

    }

    IEnumerator CollectableTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _collectable = true;
    }


    void GameChecker()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventChannelSO.OnGameEnded += GameChecker;
        EventChannelSO.OnGameOver += GameChecker;
    }

    private void OnDisable()
    {
        EventChannelSO.OnGameEnded -= GameChecker;
        EventChannelSO.OnGameOver -= GameChecker;
    }

}
