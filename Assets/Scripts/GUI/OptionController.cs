using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionController : MonoBehaviour
{
    [SerializeField] OptionsSO _optionsSO;

    public Slider volume;
    bool _muted = false;
    public Sprite _mutedSprite;
    public Sprite _unMutedSprite;
    [SerializeField] Image _muteImage;

    private void Start()
    {
        volume.value = _optionsSO.Volume;
    }


    public void AdjustVolume()
    {
        _optionsSO.Volume = volume.value;
        _muted = false;
        _muteImage.GetComponent<Image>().sprite = _unMutedSprite;
    }

    public void HandleMute()
    {
        if (_muted)
        {
            _muted = false;
            _muteImage.GetComponent<Image>().sprite = _unMutedSprite;
            _optionsSO.Volume = volume.value;
        }
        else
        {
            _muted = true;
            _muteImage.GetComponent<Image>().sprite = _mutedSprite;

            _optionsSO.Volume = 0;
        }
    }


}
