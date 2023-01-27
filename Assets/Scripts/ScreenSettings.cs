using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSettings : MonoBehaviour
{
    private void OnEnable()
    {
        Screen.SetResolution(1920, 1080, true);
    }

}
