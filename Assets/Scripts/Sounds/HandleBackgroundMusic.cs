using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBackgroundMusic : MonoBehaviour
{
    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] AudioSource _backgroundMusic;
    AudioSource _fightMusic;
    bool _fightMusicPlaying;
    bool _pauseScreenActive = false;



    private void Start()
    {
        _backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        _fightMusicPlaying = false;
        _fightMusic = GetComponent<AudioSource>();
        _fightMusic.volume = 0;

        EventChannelSO.OnGameOver += GameChecker;
        EventChannelSO.OnGameEnded += GameChecker;
        EventChannelSO.OnPauseScreen += PauseFightMusic;
        EventChannelSO.OnPauseScreenOff += ContinueFightMusic;
    }

    private void Update()
    {
        GameObject[] teachers = GameObject.FindGameObjectsWithTag("Teacher");

        foreach (GameObject _teacher in teachers)
        {

            if (_teacher.GetComponent<TeacherLogic>().Chasing)
            {
                _fightMusicPlaying = true;
                break;
            }
            _fightMusicPlaying = false;
        }


        if (_pauseScreenActive)
            return;


        if (_fightMusicPlaying)
        {
            _backgroundMusic.volume = Mathf.Lerp(_backgroundMusic.volume, 0, Time.deltaTime * 0.6f);
            _fightMusic.volume = Mathf.Lerp(_fightMusic.volume, _optionsSO.FightMusicVolume, Time.deltaTime * 0.6f);
        }
        else
        {
            _backgroundMusic.volume = Mathf.Lerp(_backgroundMusic.volume, _optionsSO.BackgroundMusicVolume, Time.deltaTime * 1f);
            _fightMusic.volume = Mathf.Lerp(_fightMusic.volume, 0, Time.deltaTime * 1f);
        }

    }

    void GameChecker()
    {
        _fightMusic.volume = 0;
        this.enabled = false;
    }

    void PauseFightMusic()
    {
        _fightMusic.volume = 0;
        _backgroundMusic.volume = _optionsSO.BackgroundMusicVolume;
        _pauseScreenActive = true;
    }

    void ContinueFightMusic()
    {
        _pauseScreenActive = false;
    }

    private void OnDisable()
    {

        EventChannelSO.OnGameOver -= GameChecker;
        EventChannelSO.OnGameEnded -= GameChecker;
        EventChannelSO.OnPauseScreen -= PauseFightMusic;
        EventChannelSO.OnPauseScreenOff -= ContinueFightMusic;

    }



}
