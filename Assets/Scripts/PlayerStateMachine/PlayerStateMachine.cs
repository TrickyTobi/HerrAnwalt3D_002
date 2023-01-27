using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class PlayerStateMachine : MonoBehaviour
{
    #region References
    [Space(10)]
    [Header("References")]
    [Space(10)]
    Rigidbody _rigidBody; public Rigidbody Rigidbody { get { return _rigidBody; } }

    PlayerInput _playerInput;
    Animator _animator; public Animator Animator { get => _animator; }

    [SerializeField] Transform _camera;

    CapsuleCollider _playerCollider;

    [SerializeField] OptionsSO _optionsSO;

    [SerializeField] PlayerStatsSO _playerStatsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;

    [SerializeField] EventChannelSO _event;

    AudioSource _audioUtiletyPlayer;
    AudioSource _audioBreathPlayer; public AudioSource AudioBreathPlayer { get => _audioBreathPlayer; }

    AudioSource _audioHeartBeatPlayer;
    #endregion

    #region PostProcess

    Volume _postProcess;
    float _currentPostProcessWeight;
    [Space(10)]
    [Header("References")]
    [Space(10)]
    [SerializeField] float _1HPPostProcess;
    [SerializeField] float _2HPPostProcess;


    #endregion

    #region StateMachine
    PlayerBaseState _currentState; public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } } //hält den aktuellen SuperState

    PlayerStateFactory _states; //hält die StateFactory
    #endregion

    #region Movevariables


    [Space(10)]
    [Header("Walking")]
    [Space(10)]
    [SerializeField] float _maxWalkSpeed; public float MaxWalkSpeed { get => _maxWalkSpeed; }

    [SerializeField] float _maxRunSpeed; public float MaxRunSpeed { get => _maxRunSpeed; }

    float _walkForce = 100;

    Vector3 _moveDirection;


    private bool _moveForward; public bool MoveForward { get => _moveForward; set => _moveForward = value; }


    private bool _moveBackward; public bool MoveBackward { get => _moveBackward; set => _moveBackward = value; }


    private bool _moveLeft; public bool MoveLeft { get => _moveLeft; set => _moveLeft = value; }


    private bool _moveRight; public bool MoveRight { get => _moveRight; set => _moveRight = value; }


    [Space(10)]
    [Header("Drag")]
    [Space(10)]

    [SerializeField] float _groundDrag;

    [SerializeField] float _airbornDrag;


    [Space(10)]
    [Header("GroundCheck")]
    [Space(10)]
    [SerializeField] bool _isGrounded; public bool IsGrounded { get => _isGrounded; }
    [SerializeField] Transform _groundCheckTransform;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance;
    [SerializeField] float _groundCheckRadius;


    Vector2 _currentMovementInput;

    Vector3 _currentMovement;
    bool _isMovementPressed; public bool IsMovementPressed { get { return _isMovementPressed; } }
    bool _isRunPressed; public bool IsRunPressed { get { return _isRunPressed; } }



    #endregion

    #region Mouse variables

    Vector2 _currentMouseInput;

    float _mouseX;
    float _mouseY;

    float _xRotation = 0;

    [Space(10)]
    [Header("Looking")]
    [Space(10)]

    [SerializeField] float _maxUpLookAngle;
    [SerializeField] float _maxDownLookAngle;

    #endregion

    #region Jump variables   


    bool _isJumpPressed = false; public bool IsJumpPressed { get { return _isJumpPressed; } }

    [Space(10)]
    [Header("Jumping")]
    [Space(10)]
    [SerializeField] float _initialJumpVelocity; public float InitialJumpVelocity { get { return _initialJumpVelocity; } }
    bool _requireNewJumpPress; public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }

    #endregion

    #region Animation

    int _isGroundedHash; public int IsGroundedHash { get { return _isGroundedHash; } set { _isGroundedHash = value; } }
    int _isWalkingHash; public int IsWalkingHash { get { return _isWalkingHash; } set { _isWalkingHash = value; } }

    int _isRunningHash; public int IsRunningHash { get { return _isRunningHash; } set { _isRunningHash = value; } }

    int _isJumpHash; public int IsJumpHash { get { return _isJumpHash; } set { _isJumpHash = value; } }

    int _isAttackHash; public int IsAttackHash { get { return _isAttackHash; } set { _isAttackHash = value; } }

    int _isBlockHash; public int IsBlockHash { get { return _isBlockHash; } set { _isBlockHash = value; } }

    int _velocityXHash; public int VelocityXHash { get { return _velocityXHash; } set { _velocityXHash = value; } }

    int _velocityZHash; public int VelocityZHash { get { return _velocityZHash; } set { _velocityZHash = value; } }

    [Space(10)]
    [Header("Animation")]
    [Space(10)]
    [SerializeField] float _velocityX = 0; public float VelocityX { get => _velocityX; set => _velocityX = value; }

    [SerializeField] float _velocityZ = 0; public float VelocityZ { get => _velocityZ; set => _velocityZ = value; }

    [SerializeField] float _acceleration; public float Acceleration { get => _acceleration; set => _acceleration = value; }

    [SerializeField] float _deceleration; public float Deceleration { get => _deceleration; set => _deceleration = value; }

    [SerializeField] float _walkThreshhold; public float WalkThreshhold { get => _walkThreshhold; set => _walkThreshhold = value; }

    [SerializeField] float _runThreshold; public float RunThreshold { get => _runThreshold; set => _runThreshold = value; }

    [SerializeField] float _zeroThreshold; public float ZeroThreshold { get => _zeroThreshold; set => _zeroThreshold = value; }

    [SerializeField] float _defaultAnimationWeight = 0.6f;

    [SerializeField] int _animHandsLayer;


    #endregion

    #region Fight

    [Space(10)]
    [Header("Fight")]
    [Space(10)]

    [SerializeField] int _maxBlocks;
    int _blockCount = 0;
    [SerializeField] bool _isAttackPressed;
    [SerializeField] bool _isAttacking; public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    [SerializeField] bool _isBlockPressed;
    [SerializeField] bool _attackEnding = false;

    [SerializeField] bool _isBlockEnding = false;

    bool _hitTarget = false; public bool HitTarget { get => _hitTarget; set => _hitTarget = value; }


    [Space(10)]
    [Header("CameraShake")]
    [Space(10)]
    [SerializeField] float _shakeTime;
    [SerializeField] float _shakeAmount;
    [SerializeField] float _decreseFactor;
    bool _gotHit = false;
    Vector3 _originalRot;
    [SerializeField] float _shakeDuration;


    #endregion

    #region Sounds

    bool _heavyHeartBeatSound = false;
    float _audioHeartBeatPlayerVolume = 0;

    [SerializeField] float _mediumHeartBeatVolume;
    [SerializeField] float _heavyHeartBeatVolume;

    float _heavyBreathVolume = 0;

    String _groundMaterial;

    #endregion

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _rigidBody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _postProcess = GetComponentInChildren<Volume>();

        SetupAnimator();
        _animator.SetLayerWeight(_animHandsLayer, _defaultAnimationWeight);

        Cursor.lockState = CursorLockMode.Locked;
        _rigidBody.freezeRotation = true;
        _postProcess.weight = 0;
        _currentPostProcessWeight = _postProcess.weight;

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        _originalRot = transform.eulerAngles;


    }

    private void Start()
    {
        _event.EnableInput();
        SoundSetup();

        _playerStatsSO.PlayerHealth = _playerStatsSO.PlayerMaxHealth;
        _playerStatsSO.PlayerParagraphCount = 0;
    }

    private void Update()
    {
        GroundCheck();

        if (_isGrounded)
        {
            _rigidBody.drag = _groundDrag;
            _animator.SetBool(IsGroundedHash, true);
        }
        else
        {
            _rigidBody.drag = _airbornDrag;
            _animator.SetBool(IsGroundedHash, false);
        }



        _currentState.UpdateStates();

        if (!_attackEnding && !_isBlockEnding)
        {
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), _defaultAnimationWeight, Time.deltaTime * 3));
        }

        HandleLowLife();


    }

    private void FixedUpdate()
    {

        HandleCameraShake();
        HandleRotation();
        HandleMovement();
        _currentState.UpdateStatePhysics();
    }

    private void LateUpdate()
    {

    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;


        if (_currentMovementInput.x >= _zeroThreshold)
        {
            _moveLeft = true;
            _moveRight = false;
        }
        else if (_currentMovementInput.x <= -_zeroThreshold)
        {
            _moveLeft = false;
            _moveRight = true;
        }
        else
        {
            _moveLeft = false;
            _moveRight = false;
        }

        if (_currentMovementInput.y >= _zeroThreshold)
        {
            _moveForward = true;
            _moveBackward = false;
        }
        else if (_currentMovementInput.y <= -_zeroThreshold)
        {
            _moveForward = false;
            _moveBackward = true;
        }
        else
        {
            _moveBackward = false;
            _moveForward = false;
        }


    }

    void OnMouseRotation(InputAction.CallbackContext context)
    {
        _currentMouseInput = _playerInput.InGame.MouseLook.ReadValue<Vector2>();
        _mouseX = _currentMouseInput.x * _optionsSO.mouseSensetivity;
        _mouseY = _currentMouseInput.y * _optionsSO.mouseSensetivity;

    }

    void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _requireNewJumpPress = false;
        _isJumpPressed = context.ReadValueAsButton();
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
    }

    void OnBlock(InputAction.CallbackContext context)
    {
        _blockCount = 0;
        _isBlockPressed = context.ReadValueAsButton();
    }

    void HandleMovement()
    {
        _moveDirection = transform.forward * _currentMovement.z + transform.right * _currentMovement.x;
        _rigidBody.AddForce(_moveDirection.normalized * _walkForce, ForceMode.Acceleration);

    }
    void GroundCheck()
    {
        _isGrounded = Physics.SphereCast(_groundCheckTransform.position,
            _groundCheckRadius, Vector3.down * _groundCheckDistance,
            out RaycastHit info,
            _groundCheckDistance,
            _groundLayer);
    }
    void HandleRotation()
    {
        _xRotation -= _mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -_maxUpLookAngle, -_maxDownLookAngle);

        _camera.localRotation = Quaternion.Euler(_xRotation * 0.9f, 0, 0);
        transform.Rotate(Vector3.up * _mouseX);
    }
    void SetupAnimator()
    {
        _animator = GetComponent<Animator>();
        _isGroundedHash = Animator.StringToHash("isGrounded");
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isAttackHash = Animator.StringToHash("isAttack");
        _isBlockHash = Animator.StringToHash("isBlock");
        _isJumpHash = Animator.StringToHash("isJumping");
        _velocityXHash = Animator.StringToHash("velocityX");
        _velocityZHash = Animator.StringToHash("velocityZ");
    }
    void PlayerAttackEnding()
    {
        _attackEnding = true;
    }

    void PlayerAttackEnded()
    {
        _animator.SetBool(IsAttackHash, false);
        _attackEnding = false;
        _isAttacking = false;
        _hitTarget = false;
    }

    void PlayerBlockEnding()
    {
        if (_isAttacking)
            return;

        _isBlockEnding = true;
    }

    void PlayerBlockEnded()
    {
        _isBlockEnding = false;
    }

    public void HandleAttack()
    {

        if (_isAttackPressed && !_isAttacking)
        {
            _audioUtiletyPlayer.PlayOneShot(_soundEffectSO.AttackSwoosh(), _optionsSO.AttackSwooshVolume);

            _isAttacking = true;
            _isBlockPressed = false;
            _isBlockEnding = false;
            _animator.SetBool(_isBlockHash, false);
            _animator.SetBool(_isAttackHash, true);
        }

        if (_isAttacking && !_attackEnding)
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 1, Time.deltaTime * 30));
    }

    public void HandleBlock()
    {
        if (_isBlockPressed && _isGrounded)
        {
            _isAttacking = false;
            _isAttackPressed = false;
            _animator.SetBool(_isAttackHash, false);
            _animator.SetBool(_isBlockHash, true);
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 1, Time.deltaTime * 30));
        }

        if (!_isBlockPressed)
        {
            _animator.SetBool(_isBlockHash, false);
        }
    }


    void HandleLowLife()
    {
        if (_playerStatsSO.PlayerHealth > 2)
        {
            _postProcess.weight = Mathf.Lerp(_postProcess.weight, 0, Time.deltaTime * 5f);


            if (_audioHeartBeatPlayer.volume == 0f)
                return;

            _audioHeartBeatPlayer.volume = Mathf.Lerp(_audioHeartBeatPlayerVolume, _mediumHeartBeatVolume, Time.deltaTime * 0.2f);


            if (_audioHeartBeatPlayer.volume <= 0.1f && _heavyHeartBeatSound)
            {
                _audioHeartBeatPlayer.volume = 0;
                _heavyHeartBeatSound = false;
                _audioHeartBeatPlayer.Stop();
            }

            return;
        }


        if (_playerStatsSO.PlayerHealth == 2)
        {
            _postProcess.weight = Mathf.Lerp(_postProcess.weight, _2HPPostProcess, Time.deltaTime * 20f);
            _audioHeartBeatPlayer.volume = Mathf.Lerp(_audioHeartBeatPlayerVolume, 0.5f, Time.deltaTime * 1f);

            if (_heavyHeartBeatSound)
            {
                _heavyHeartBeatSound = true;
                _audioHeartBeatPlayer.Play();

            }
            return;
        }

        if (_playerStatsSO.PlayerHealth == 1)
        {
            _audioHeartBeatPlayer.volume = Mathf.Lerp(_audioHeartBeatPlayerVolume, _heavyHeartBeatVolume, Time.deltaTime * 1f);
            _postProcess.weight = Mathf.Lerp(_postProcess.weight, _1HPPostProcess, Time.deltaTime * 20f);

            return;
        }


    }

    void HandleCameraShake()
    {

        if (_shakeDuration > 0)
        {
            _gotHit = true;
            _camera.eulerAngles = new Vector3(Random.insideUnitSphere.x, _camera.eulerAngles.y, Random.insideUnitSphere.z) * _shakeAmount;
            _shakeDuration -= Time.deltaTime * _decreseFactor;
        }
        else if (_gotHit)
        {
            _gotHit = false;
            _shakeDuration = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        TeacherLogic _teacher;

        if (!other.CompareTag("EnemyWeapon"))
            return;

        _teacher = other.GetComponentInParent<TeacherLogic>();

        if (!_teacher.DamageActive)
            return;

        if (_teacher.AlreadyHit)
            return;


        if (_blockCount < _maxBlocks && _isBlockPressed)
        {
            _audioUtiletyPlayer.PlayOneShot(_soundEffectSO.AttorneyBlock(), _optionsSO.AttorneyBlockVolume);
            _shakeDuration = _shakeTime / 2;
            _blockCount++;
            _teacher.AlreadyHit = true;
            return;
        }
        else if (_blockCount >= _maxBlocks)
        {
            _audioUtiletyPlayer.PlayOneShot(_soundEffectSO.AttorneyBlockBreak(), _optionsSO.AttorneyBlockBreakVolume);
            _isBlockPressed = false;
        }


        _audioUtiletyPlayer.PlayOneShot(_soundEffectSO.AttorneyGotHit(), _optionsSO.AttorneyGotHitVolume);
        _blockCount = 0;
        _event.LoseLife();
        _shakeDuration = _shakeTime;
        _teacher.AlreadyHit = true;
    }

    public void IncreaseBreathSound()
    {
        AudioBreathPlayer.volume = Mathf.Lerp(_heavyBreathVolume, _optionsSO.AttorneyHeavyBreathVolume, Time.deltaTime * 0.1f);
    }

    public void DecreaseBreathSound()
    {
        AudioBreathPlayer.volume = Mathf.Lerp(_heavyBreathVolume, 0, Time.deltaTime * 0.2f);
    }

    void SoundSetup()
    {
        _audioUtiletyPlayer = gameObject.AddComponent<AudioSource>();
        _audioUtiletyPlayer.spatialBlend = 0;
        _audioBreathPlayer = gameObject.AddComponent<AudioSource>();
        _audioBreathPlayer.spatialBlend = 0;
        _audioBreathPlayer.clip = _soundEffectSO.AttorneyheavyBreathing();
        _audioHeartBeatPlayer = gameObject.AddComponent<AudioSource>();
        _audioHeartBeatPlayer.spatialBlend = 0;
        _audioHeartBeatPlayer.clip = _soundEffectSO.AttorneyHeartBeat();

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Debug.DrawLine(_groundCheckTransform.position, _groundCheckTransform.position + Vector3.down * _groundCheckDistance, Color.red);
        Gizmos.DrawWireSphere(_groundCheckTransform.position + Vector3.down * _groundCheckDistance, _groundCheckRadius);
    }

    private void OnEnable()
    {
        EventChannelSO.OnEnableInput += EnableInput;
        EventChannelSO.OnDisableInput += DisableInput;

    }


    private void OnDisable()
    {
        EventChannelSO.OnEnableInput -= EnableInput;
        EventChannelSO.OnDisableInput -= DisableInput;
        _event.DisableInput();
    }

    public void EnableInput()
    {
        _playerInput.Enable();
        _playerInput.InGame.Movement.started += OnMovementInput;
        _playerInput.InGame.Movement.canceled += OnMovementInput;
        _playerInput.InGame.Movement.performed += OnMovementInput;

        _playerInput.InGame.Run.started += OnRun;
        _playerInput.InGame.Run.canceled += OnRun;

        _playerInput.InGame.Jump.started += OnJump;
        _playerInput.InGame.Jump.canceled += OnJump;

        _playerInput.InGame.MouseLook.performed += OnMouseRotation;

        _playerInput.InGame.Melee.started += OnAttack;
        _playerInput.InGame.Melee.canceled += OnAttack;

        _playerInput.InGame.Block.started += OnBlock;
        _playerInput.InGame.Block.canceled += OnBlock;
    }
    public void DisableInput()
    {
        _playerInput.Disable();
        _playerInput.InGame.Movement.started -= OnMovementInput;
        _playerInput.InGame.Movement.canceled -= OnMovementInput;
        _playerInput.InGame.Movement.performed -= OnMovementInput;

        _playerInput.InGame.Run.started -= OnRun;
        _playerInput.InGame.Run.canceled -= OnRun;

        _playerInput.InGame.Jump.started -= OnJump;
        _playerInput.InGame.Jump.canceled -= OnJump;

        _playerInput.InGame.MouseLook.performed -= OnMouseRotation;

        _playerInput.InGame.Melee.started -= OnAttack;
        _playerInput.InGame.Melee.canceled -= OnAttack;

        _playerInput.InGame.Block.started -= OnBlock;
        _playerInput.InGame.Block.canceled -= OnBlock;
    }


}
