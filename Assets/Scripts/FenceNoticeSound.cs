using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FenceNoticeSound : MonoBehaviour
{

    [SerializeField] SoundEffectSO _sound;
    [SerializeField] OptionsSO _options;
    AudioSource _audioUtilityPlayer;

    bool _timeOut = false;

    private void Start()
    {
        _audioUtilityPlayer = gameObject.AddComponent<AudioSource>();
        _audioUtilityPlayer.spatialBlend = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Player") || _timeOut)
            return;
        Debug.Log("Here");
        _timeOut = true;
        StartCoroutine(TimeOutTimer());
        _audioUtilityPlayer.PlayOneShot(_sound.FenceStartNotice(), _options.FenceNoticeSound);
    }


    IEnumerator TimeOutTimer()
    {
        yield return new WaitForSeconds(1);
        _timeOut = false;
    }
}
