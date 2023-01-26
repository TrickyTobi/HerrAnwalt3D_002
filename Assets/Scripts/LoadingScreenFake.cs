using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadingScreenFake : MonoBehaviour
{

    [SerializeField] AudioSource _music;
    [SerializeField] PlayerStateMachine _player;
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] PauseMenuController _pauseScreen;
    [SerializeField] OptionsSO _options;
    [SerializeField] EventChannelSO _event;
    bool _playing;


    VideoPlayer _video;
    AudioSource _audio;

    private void Start()
    {
        _music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        _player = GameObject.Find("HerrAnwalt").GetComponent<PlayerStateMachine>();
        _gameUI = GameObject.Find("3D Canvas");
        _video = GetComponent<VideoPlayer>();
        _audio = GetComponent<AudioSource>();


        if (!_options.ShowLoadingScreen)
        {
            StartCoroutine(EndDelay());
            _playing = false;
            return;
        }

        _audio.volume = _options.LoadingScreenVolume;

        _playing = true;

        StartCoroutine(End());
    }

    private void Update()
    {

        if (_playing)
        {
            Time.timeScale = 0;
            _player.DisableInput();
            _pauseScreen.DisableInput();
            _music.volume = 0;
            _gameUI.SetActive(false);
            _options.ShowLoadingScreen = false;


        }


        if (!_playing)
        {
            Time.timeScale = 1;
            _music.volume = Mathf.Lerp(_music.volume, _options.BackgroundMusicVolume, Time.deltaTime * 3);


            if (_music.volume >= _options.BackgroundMusicVolume - 0.05f)
            {
                StartCoroutine(EndDelay());
            }

        }
    }

    IEnumerator End()
    {
        if (!_options.Debug)
            yield return new WaitForSecondsRealtime((float)_video.clip.length - 0.3f);

        yield return new WaitForSecondsRealtime(0.5f);
        _playing = false;
    }

    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _music.volume = _options.BackgroundMusicVolume;
        _gameUI.SetActive(true);
        _player.enabled = true;
        _event.EnableInput();
        _pauseScreen.EnableInput();
        _loadingScreen.SetActive(false);
    }
}
