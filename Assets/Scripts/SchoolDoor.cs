using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SchoolDoor : MonoBehaviour
{
    [SerializeField] Transform _door_Left;
    [SerializeField] Transform _door_Right;
    [SerializeField] PlayerStatsSO _playerStats;
    [SerializeField] OptionsSO _options;
    [SerializeField] SoundEffectSO _sounds;
    [SerializeField] float _openingTime;



    AudioSource _audioPlayerSource;
    AudioSource _audioUtilitySource;

    float _timePassed = 0f;
    bool _doorOpen = false;

    int _narratorCount;

    bool _enteredTrigger = false;

    bool _timeOut = false;

    bool _noticeTimeOut = false;

    bool _bumpedIn = false;

    private void Start()
    {
        _narratorCount = 0;
        _audioPlayerSource = gameObject.AddComponent<AudioSource>();
        _audioPlayerSource.spatialBlend = 0;
        _audioPlayerSource.clip = _sounds.ChildNotice(_sounds.childNoticeSound.Length);
        _playerStats.FreedChilds = 0;
        _audioUtilitySource = gameObject.AddComponent<AudioSource>();
        _audioUtilitySource.spatialBlend = 1;
    }

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

        if (!other.CompareTag("Player") || _timeOut)
            return;

        _timeOut = true;

        StartCoroutine(TimeOutTimer());

        _audioPlayerSource.volume = _options.ChildNoticeVolume;

        _narratorCount++;

        if (_playerStats.FreedChilds != _playerStats.ChildsToFree && !_noticeTimeOut)
        {

            _bumpedIn = true;

            switch (_narratorCount)
            {
                case 1:
                    _audioPlayerSource.PlayOneShot(_sounds.ChildNotice(_narratorCount));
                    break;
                case 2:
                    _audioPlayerSource.PlayOneShot(_sounds.ChildNotice(_narratorCount));
                    break;
                case 3:
                    _audioPlayerSource.PlayOneShot(_sounds.ChildNotice(_narratorCount));
                    break;
                case 4:
                    _audioPlayerSource.PlayOneShot(_sounds.ChildNotice(_narratorCount));
                    break;
                default:
                    break;
            }

            if (_narratorCount >= 4)
                _narratorCount = 0;

            _noticeTimeOut = true;
            StartCoroutine(NoticeTimeOutTimer());
        }
        else if (_playerStats.FreedChilds == _playerStats.ChildsToFree && !_doorOpen && _bumpedIn)
            StartCoroutine(OpeningDelay());
        else if (_playerStats.FreedChilds == _playerStats.ChildsToFree && !_doorOpen && !_bumpedIn)
        {
            _audioUtilitySource.PlayOneShot(_sounds.DoorOpen(), _options.DoorOpenVolume);
            _doorOpen = true;
            _enteredTrigger = true;
        }

    }

    IEnumerator OpeningDelay()
    {
        _audioPlayerSource.Play();
        _doorOpen = true;

        yield return new WaitForSeconds(_audioPlayerSource.clip.length - 1f);
        _audioUtilitySource.PlayOneShot(_sounds.DoorOpen(), _options.DoorOpenVolume);
        _enteredTrigger = true;
    }

    IEnumerator TimeOutTimer()
    {
        yield return new WaitForSeconds(3f);
        _timeOut = false;
    }

    IEnumerator NoticeTimeOutTimer()
    {
        yield return new WaitForSeconds(10f);
        _noticeTimeOut = false;
    }
}
