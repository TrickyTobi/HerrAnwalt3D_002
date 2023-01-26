using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameEndController : MonoBehaviour
{
    public GameObject _endVideoPlayer;
    AudioSource _endAudio;
    VideoPlayer _endVideo;
    public GameObject _endImage;

    AudioSource _backgroundMusic;

    public GameObject _canvas;
    bool _videoPlayed = false;

    [SerializeField] EventChannelSO _event;
    [SerializeField] OptionsSO _options;

    void Start()
    {
        _endVideoPlayer.SetActive(false);
        _endImage.SetActive(false);
        _canvas.SetActive(true);
        _backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        _endVideoPlayer.GetComponentInChildren<AudioSource>().volume = _options.EndvideoVolume;
        _endAudio = _endVideoPlayer.GetComponentInChildren<AudioSource>();
        _endVideo = _endVideoPlayer.GetComponentInChildren<VideoPlayer>();

        _endAudio.volume = _options.EndvideoVolume;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_videoPlayed)
        {
            _event.DisableInput();

            _backgroundMusic.volume = 0f;
            _endVideoPlayer.SetActive(true);
            _canvas.SetActive(false);
            _videoPlayed = true;
            _endVideoPlayer.SetActive(true);
            StartCoroutine(stopEndVideoClip());
            _canvas.SetActive(false);
        }
    }

    IEnumerator stopEndVideoClip()
    {
        yield return new WaitForSeconds((float)_endVideo.clip.length - 0.4f);

        _backgroundMusic.volume = _options.BackgroundMusicVolume;

        yield return new WaitForSeconds(0.4f);

        _endImage.SetActive(true);
        _endVideoPlayer.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
