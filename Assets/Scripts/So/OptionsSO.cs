
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = ("OptionsSO"))]
public class OptionsSO : ScriptableObject
{
    [Space(20)]
    [Header("Mouse")]
    [Space(20)]

    public float mouseSensetivity = 100f;

    float _mouseVolume; public float MouseVolume { get { return _mouseVolume; } set { _mouseVolume = value; } }

    [Space(20)]
    [Header("Game")]
    [Space(20)]
    [SerializeField] bool _showLoadingScreen; public bool ShowLoadingScreen { get => _showLoadingScreen; set => _showLoadingScreen = value; }
    [SerializeField] bool _debug; public bool Debug { get => _debug; set => _debug = value; }
    [SerializeField] float _volume; public float Volume { get { return _volume; } set { _volume = value; UpdateVolume(); } }

    [SerializeField] float _testVolume;
    [SerializeField] bool _muted = false; public bool Muted { get { return _muted; } set { _muted = value; UpdateVolume(); } }

    [Space(40)]
    [Header("Sound")]
    [Space(20)]
    [SerializeField] float _backgroundMusicVolume; public float BackgroundMusicVolume { get => _backgroundMusicVolume; set => _backgroundMusicVolume = value; }
    [SerializeField] float _fightMusicVolume; public float FightMusicVolume { get => _fightMusicVolume; set => _fightMusicVolume = value; }
    [SerializeField] float _endvideoVolume; public float EndvideoVolume { get => _endvideoVolume; set => _endvideoVolume = value; }
    [SerializeField] float _loadingScreenVolume; public float LoadingScreenVolume { get => _loadingScreenVolume; set => _loadingScreenVolume = value; }

    [SerializeField] float _laptopVolume; public float LaptopVolume { get => _laptopVolume; set => _laptopVolume = value; }

    [SerializeField] float volumeCorrectionMultiplier;

    [Space(20)]

    [Tooltip("Volume of all footsteps")]
    [SerializeField] float _stepVolume; public float StepVolume { get { return _stepVolume; } set { _stepVolume = value; } }

    [Tooltip("Volume of standard attacksound")]
    [SerializeField] float _genericAttackVolume; public float GenericAttackVolume { get { return _genericAttackVolume; } set { _genericAttackVolume = value; } }

    [Tooltip("Volume of standard attack swoosh")]
    [SerializeField] float _attackSwooshVolume; public float AttackSwooshVolume { get { return _attackSwooshVolume; } set { _attackSwooshVolume = value; } }

    [Tooltip("Volume of attorney hits")]
    [SerializeField] float _attornyHitVolume; public float AttornyHitVolume { get { return _attornyHitVolume; } set { _attornyHitVolume = value; } }

    [Tooltip("Volume of blocking an attack")]
    [SerializeField] float _attorneyBlockVolume; public float AttorneyBlockVolume { get { return _attorneyBlockVolume; } set { _attorneyBlockVolume = value; } }

    [Tooltip("Volume when the block breaks due to hard attacks")]
    [SerializeField] float _attorneyBlockBreakVolume; public float AttorneyBlockBreakVolume { get { return _attorneyBlockBreakVolume; ; } set { _attorneyBlockBreakVolume = value; } }

    [Tooltip("Volume attorney getting hit")]
    [SerializeField] float _attorneyGotHitVolume; public float AttorneyGotHitVolume { get { return _attorneyGotHitVolume; } set { _attorneyGotHitVolume = value; } }

    [Tooltip("Volume attorney heavy breathing")]
    [SerializeField] float _attorneyHeavyBreathVolume = 0; public float AttorneyHeavyBreathVolume { get => _attorneyHeavyBreathVolume; set => _attorneyHeavyBreathVolume = value; }


    [Tooltip("Volume of child screaming for help")]
    [SerializeField] float _childHelpVolume; public float ChildHelpVolume { get { return _childHelpVolume; } set { _childHelpVolume = value; } }

    [Tooltip("Volume of child cheering")]
    [SerializeField] float _childCheeringVolume; public float ChildCheeringVolume { get { return _childCheeringVolume; } set { _childCheeringVolume = value; } }



    [Tooltip("Volume of running into a fence")]
    [SerializeField] float _fenceHitVolume; public float FenceHitVolume { get => _fenceHitVolume; set => _fenceHitVolume = value; }

    [Tooltip("Volume teacher got hit")]
    [SerializeField] float _teacherGotHitVolume; public float TeacherGotHitVolume { get => _teacherGotHitVolume; set => _teacherGotHitVolume = value; }

    [Tooltip("Volume of teacher taunting")]
    [SerializeField] float _teacherTauntingVolume; public float TeacherTauntingVolume { get => _teacherTauntingVolume; set => _teacherTauntingVolume = value; }

    [Tooltip("Volume of teacher attacks")]
    [SerializeField] float _teacherAttackVolume; public float TeacherAttackVolume { get => _teacherAttackVolume; set => _teacherAttackVolume = value; }

    [Tooltip("Volume of teacher hitting")]
    [SerializeField] float _teacherHitVolume; public float TeacherHitVolume { get => _teacherHitVolume; set => _teacherHitVolume = value; }

    [Tooltip("Volume of teacher dying")]
    [SerializeField] float _teacherDeathVolume; public float TeacherDeathVolume { get => _teacherDeathVolume; set => _teacherDeathVolume = value; }


    void UpdateVolume()
    {
        AudioListener.volume = _volume * volumeCorrectionMultiplier;
        _testVolume = AudioListener.volume;

    }

    private void OnEnable()
    {
        _showLoadingScreen = true;
    }
}
