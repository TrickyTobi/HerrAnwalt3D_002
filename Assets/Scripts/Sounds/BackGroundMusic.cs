using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{

    private static BackGroundMusic s_backGroundMusic;
    [SerializeField] OptionsSO _options;

    void Awake()
    {
        if (s_backGroundMusic == null)
        {
            s_backGroundMusic = this;
            GetComponent<AudioSource>().volume = _options.BackgroundMusicVolume;
            DontDestroyOnLoad(s_backGroundMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
