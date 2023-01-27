using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandlerLaptop : MonoBehaviour
{
    [SerializeField] OptionsSO _options;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = _options.LaptopVolume;
    }

}
