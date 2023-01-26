using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
     [SerializeField] Image soundOnImage;
     [SerializeField] Image soundOffImage;

    [SerializeField] Slider volumeSlider;

     private bool muted = false;

     void Start()
     {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            LoadMusic();
        }
        else
        {
            LoadMusic();
        }
        UpdateButtonImage();
        AudioListener.pause = muted;

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        else
        {
            LoadVolume();
        }
     }

     public void SoundButton()
     {
        if(muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
           muted = false;
            AudioListener.pause = false;
        }

        SaveMusic();
        UpdateButtonImage();
     }

     private void UpdateButtonImage()
     {
        if (muted == false)
        {
            soundOnImage.enabled = true;
            soundOffImage.enabled = false;
        }
        else
        {
            soundOnImage.enabled = false;
            soundOffImage.enabled = true;
        }
     }

     private void LoadMusic()
     {
         muted = PlayerPrefs.GetInt("muted") == 1; // if muted is 1, muted sets to true, otherwise its false
     }

     private void SaveMusic()
     {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);  //if muted = true, we will safe it as 1, if muted = false, we will safe it as 0
     }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
















































//public static SoundManager Instance;

//[SerializeField] private AudioSource _musicSource, _effectsSource;

//void Awake()
//{
//if (Instance == null)
//{
//    //Instance = this;
//DontDestroyOnLoad(gameObject);
//    }
//else
//{
//Destroy(gameObject);
//    }
//}

//public void PlaySound(AudioClip clip)
//{
//_effectsSource.PlayOneShot(clip);
//}

//public void ChangeMasterVolume(float value)
//{
//AudioListener.volume = value;
//}

//public void ChangeEffects()
//{
//_effectsSource.mute = !_effectsSource;
//}

//public void ChangeMusic()
//{
//_musicSource.mute = !_musicSource;
//}

