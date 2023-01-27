using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class ChildLogic : MonoBehaviour
{
    #region References
    Rigidbody _rigidBody; public Rigidbody Rigidbody { get { return _rigidBody; } }
    Animator _animator; public Animator Animator { get => _animator; }

    [SerializeField] PlayerStatsSO _playerStatsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;
    [SerializeField] OptionsSO _optionsSO;


    NavMeshAgent _agent;

    #endregion

    #region Move

    [Space(10)]
    [Header("Walking")]
    [Space(10)]
    [SerializeField] float _maxWalkSpeed; public float MaxWalkSpeed { get => _maxWalkSpeed; }

    [SerializeField] float _maxRunSpeed; public float MaxRunSpeed { get => _maxRunSpeed; }

    Vector3 _moveDirection;

    private bool _moveForward; public bool MoveForward { get => _moveForward; set => _moveForward = value; }


    private bool _moveBackward; public bool MoveBackward { get => _moveBackward; set => _moveBackward = value; }


    private bool _moveLeft; public bool MoveLeft { get => _moveLeft; set => _moveLeft = value; }


    private bool _moveRight; public bool MoveRight { get => _moveRight; set => _moveRight = value; }





    #endregion

    #region Animation

    [Space(10)]
    [Header("Animation")]
    [Space(10)]

    [SerializeField] float _velocityX = 0; public float VelocityX { get => _velocityX; set => _velocityX = value; }

    [SerializeField] float _velocityZ = 0; public float VelocityZ { get => _velocityZ; set => _velocityZ = value; }

    [SerializeField] float _walkAnimMultiplicator; public float WalkAnimMultiplicator { get => _walkAnimMultiplicator; set => _walkAnimMultiplicator = value; }

    [SerializeField] float _acceleration; public float Acceleration { get => _acceleration; set => _acceleration = value; }

    [SerializeField] float _deceleration; public float Deceleration { get => _deceleration; set => _deceleration = value; }

    [SerializeField] float _walkThreshhold; public float WalkThreshhold { get => _walkThreshhold; set => _walkThreshhold = value; }

    [SerializeField] float _runThreshold; public float RunThreshold { get => _runThreshold; set => _runThreshold = value; }

    [SerializeField] float _zeroThreshold; public float ZeroThreshold { get => _zeroThreshold; set => _zeroThreshold = value; }

    [SerializeField] float _defaultAnimationWeight = 0.6f; public float DefaultAnimationWeight { get => _defaultAnimationWeight; }

    [SerializeField] int _animHandsLayer; public int AnimHandsLayer { get => _animHandsLayer; }

    int _isWalkingHash; public int IsWalkingHash { get { return _isWalkingHash; } set { _isWalkingHash = value; } }

    int _isRunnningHash; public int IsRunningHash { get { return _isRunnningHash; } set { _isRunnningHash = value; } }

    int _isJumpHash; public int IsJumpHash { get { return _isJumpHash; } set { _isJumpHash = value; } }


    int _velocityXHash; public int VelocityXHash { get { return _velocityXHash; } set { _velocityXHash = value; } }

    int _velocityZHash; public int VelocityZHash { get { return _velocityZHash; } set { _velocityZHash = value; } }


    int _isFreedHash; public int IsFreedHash { get { return _isFreedHash; } set { _isFreedHash = value; } }

    bool _waving; public bool Waving { get { return _waving; } set { _waving = value; } }


    [SerializeField] int _availableDeaths;



    #endregion

    #region Fight

    [Space(10)]
    [Header("Fight")]
    [Space(10)]

    [SerializeField] float _health;
    #endregion

    #region AILogic

    [Space(10)]
    [Header("AI Logic")]
    [Space(10)]

    [SerializeField] bool _catched;
    [SerializeField] Transform _endWayPoint;

    #endregion

    #region Sound

    float _soundTimer;
    [SerializeField] float _soundInterval;

    AudioSource _audioUtilityPlayer;

    #endregion
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        SetupAnimator();
        _rigidBody.freezeRotation = true;
        _agent.enabled = false;
        _catched = true;
        _animator.SetLayerWeight(_animHandsLayer, 0.01f);
        _rigidBody.isKinematic = true;

        _audioUtilityPlayer = gameObject.AddComponent<AudioSource>();
        _audioUtilityPlayer.spatialBlend = 1;
        _audioUtilityPlayer.maxDistance = 10;

        _soundInterval += Random.Range(0f, 15f);

        EventChannelSO.OnGameEnded += GameChecker;
        EventChannelSO.OnGameOver += GameChecker;

    }

    void Update()
    {


        HandleHands();

        HandleSound();

        if (_catched)
        {
            return;
        }



        Freed();

        _agent.SetDestination(_endWayPoint.position);

        if (_agent.velocity.x != 0 || _agent.velocity.z != 0)
            _animator.SetBool(_isWalkingHash, true);
        else
            _animator.SetBool(_isWalkingHash, false);

        Vector3 localSpeed = transform.InverseTransformDirection(_agent.velocity);
        _animator.SetFloat(_velocityXHash, localSpeed.x * _walkAnimMultiplicator);
        _animator.SetFloat(_velocityZHash, localSpeed.z * _walkAnimMultiplicator);


        Vector3 distanceToWalkPoint = transform.position - _endWayPoint.position;
        if (distanceToWalkPoint.magnitude > 1.5f)
            return;
        Destroy(gameObject, 2f);
    }

    void HandleHands()
    {


        if (_waving)
        {
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 1, Time.deltaTime * 4));
            return;
        }

        _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 0.01f, Time.deltaTime * 4));

    }

    void HandleSound()
    {
        if (_catched)
        {
            if (_soundTimer < _soundInterval)
            {
                _soundTimer += Time.deltaTime;
                return;
            }
            _audioUtilityPlayer.PlayOneShot(_soundEffectSO.ChildHelpVoice(), _optionsSO.ChildHelpVolume);
            _soundTimer = 0;
        }
        else
        {
            if (_soundTimer < _soundInterval)
            {
                _soundTimer += Time.deltaTime;
                return;
            }
            _audioUtilityPlayer.PlayOneShot(_soundEffectSO.ChildCheersVoice(), _optionsSO.ChildCheeringVolume);
            _soundTimer = 0;
        }
    }

    void HandsWaving()
    {
        _waving = true;
    }

    void HandsRunning()
    {
        _waving = false;
    }

    // thanks shouts.
    void HandleRunning()
    {
        ;
    }
    // When child gets Freed, called my destroyed lock
    public void Freed()
    {
        _rigidBody.isKinematic = false;
        _agent.enabled = true;
        _catched = false;
        _animator.SetBool(IsFreedHash, true);
    }


    void Deactivate()

    {
        this.enabled = false;
    }

    void SetupAnimator()
    {
        _animator = GetComponent<Animator>();
        IsWalkingHash = Animator.StringToHash("isWalking");
        IsRunningHash = Animator.StringToHash("isRunning");
        VelocityXHash = Animator.StringToHash("velocityX");
        VelocityZHash = Animator.StringToHash("velocityZ");
        IsFreedHash = Animator.StringToHash("isFreed");

    }

    void GameChecker()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EventChannelSO.OnGameEnded -= GameChecker;
        EventChannelSO.OnGameOver -= GameChecker;
    }


}
