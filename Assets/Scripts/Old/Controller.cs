using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [Space(10)]
    [Header("References")]
    [Space(10)]
    [SerializeField] OptionsSO _optionsSO;
    [SerializeField] PlayerStatsSO _playerStatsSO;
    [SerializeField] Camera _mainCamera;

    Rigidbody _rigidBody;
    PlayerInput _playerInput;
    Transform _camera;
    CapsuleCollider _playerCollider;


    [Space(10)]
    [Header("Walking")]
    [Space(10)]
    [SerializeField] float _maxWalkSpeed;
    [SerializeField] float _maxRunSpeed;
    float _walkForce = 100;
    [SerializeField] float _runMultiplier;
    Vector3 _moveDirection;

    [Space(10)]
    [Header("Drag")]
    [Space(10)]
    [SerializeField] float _groundDrag;
    [SerializeField] float _airbornDrag;

    [Space(10)]
    [Header("GroundCheck")]
    [Space(10)]
    [SerializeField] bool _isGrounded;
    [SerializeField] Transform _groundCheckTransform;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckDistance;
    [SerializeField] float _groundCheckRadius;

    bool _isMovementPressed;
    bool _isRunPressed;

    Vector2 _currentMovementInput; //current input directly from the Inputcontroller
    Vector3 _currentMovement; //Remapped Input from Vector2 into Vector 3

    Vector2 _currentMouseInput; // Vector2 Position of mouse on Screen
    float _mouseX;
    float _mouseY;
    float _xRotation = 0;


    [Space(10)]
    [Header("Jumping")]
    [Space(10)]
    [SerializeField] float _initialJumpVelocity;
    bool _requireNewJumpPress;
    bool _isJumpPressed = false;

    // Start is called before the first frame update
    void Awake()
    {
        _playerInput = new PlayerInput();

        _rigidBody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<CapsuleCollider>();
        _camera = _mainCamera.GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        _rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        if (_isGrounded)
        {
            _rigidBody.drag = _groundDrag;
        }
        else
            _rigidBody.drag = _airbornDrag;

        SpeedControl();
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleJump();
        HandleMovement();
    }

    void HandleMovement()
    {
        _moveDirection = transform.forward * _currentMovement.z + transform.right * _currentMovement.x;
        _rigidBody.AddForce(_moveDirection.normalized * _walkForce, ForceMode.Acceleration);

    }

    void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);


        if (!_isRunPressed && flatVelocity.magnitude > _maxWalkSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxWalkSpeed;
            _rigidBody.velocity = new Vector3(limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
            Debug.Log("Walk");
            return;
        }

        if (_isRunPressed && flatVelocity.magnitude > _maxRunSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxRunSpeed;
            _rigidBody.velocity = new Vector3(limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
            Debug.Log("Run");
            return;
        }
    }

    void HandleJump()
    {
        if (_isJumpPressed && !_requireNewJumpPress)
        {
            _rigidBody.AddForce(transform.up * _initialJumpVelocity, ForceMode.Impulse);
            _requireNewJumpPress = true;
        }
    }

    void GroundCheck()
    {
        _isGrounded = Physics.SphereCast(_groundCheckTransform.position, _groundCheckRadius, Vector3.down * _groundCheckDistance, out RaycastHit info, _groundCheckDistance, _groundLayer);
    }

    void HandleRotation()
    {
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 60f);

        _camera.localRotation = Quaternion.Euler(_xRotation * 0.9f, 0, 0);
        transform.Rotate(Vector3.up * _mouseX);
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;


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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Debug.DrawLine(_groundCheckTransform.position, _groundCheckTransform.position + Vector3.down * _groundCheckDistance, Color.red);
        Gizmos.DrawWireSphere(_groundCheckTransform.position + Vector3.down * _groundCheckDistance, _groundCheckRadius);
    }


    private void OnEnable()
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
    }
    private void OnDisable()
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

    }




}
