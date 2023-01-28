using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class TeacherLogic : MonoBehaviour
{
    #region References
    Rigidbody _rigidBody;
    Animator _animator;
    CapsuleCollider _teacherCollider;

    [SerializeField] PlayerStatsSO _playerStatsSO;
    [SerializeField] SoundEffectSO _soundEffectSO;
    [SerializeField] OptionsSO _optionsSO;


    [SerializeField] Transform _headTarget;
    Vector3 _headTargetDefault;

    MultiAimConstraint _aimConstraing;
    NavMeshAgent _agent;
    Transform _player;
    AudioSource _audioPlayer;
    AudioSource _audioStreamPlayer;
    #endregion

    #region Move

    [Space(10)]
    [Header("Walking")]
    [Space(10)]
    [SerializeField] float _maxWalkSpeed;

    [SerializeField] float _maxRunSpeed;

    Vector3 _moveDirection;

    private bool _moveForward;


    private bool _moveBackward;


    private bool _moveLeft;


    private bool _moveRight;





    #endregion

    #region Animation

    [Space(10)]
    [Header("Animation")]
    [Space(10)]

    [SerializeField] float _walkAnimMultiplicator;

    [SerializeField] float _acceleration; public float Acceleration { get => _acceleration; }

    [SerializeField] float _deceleration; public float Deceleration { get => _deceleration; }

    [SerializeField] float _walkThreshhold; public float WalkThreshhold { get => _walkThreshhold; }

    [SerializeField] float _runThreshold; public float RunThreshold { get => _runThreshold; }

    [SerializeField] float _zeroThreshold; public float ZeroThreshold { get => _zeroThreshold; }

    [SerializeField] float _defaultAnimationWeight = 0.6f; public float DefaultAnimationWeight { get => _defaultAnimationWeight; }

    [SerializeField] int _animHandsLayer; public int AnimHandsLayer { get => _animHandsLayer; }

    int _isWalkingHash;

    int _isRunningHash;

    int _isAttackHash;
    int _inAttackHash; public int InAttackHash { get { return _inAttackHash; } set { _inAttackHash = value; } }

    int _attackModeHash;

    int _gotHitHash; public int GotHitHash { get { return _gotHitHash; } }
    int _velocityXHash;

    int _velocityZHash;

    int _isDyingHash;

    int _deathNumberHash; public int DeathNumberHash { get { return _deathNumberHash; } set { _deathNumberHash = value; } }

    bool _dying;
    float _deathNumber;

    [SerializeField] int _availableDeaths;



    #endregion

    #region Fight




    [Space(10)]
    [Header("Fight")]
    [Space(10)]

    [SerializeField] float _health; public float Health { get => _health; set => _health = value; }
    [SerializeField] float _timeBetweenAttacks;

    [Space(10)]
    bool _attackTimeout;

    bool _damageActive = false; public bool DamageActive { get => _damageActive; }
    bool _alreadyHit = false; public bool AlreadyHit { get => _alreadyHit; set => _alreadyHit = value; }
    bool _attackEnding = false;

    bool _attackStarting = false;

    bool _isAttacking; public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    bool _isBlockPressed;



    bool _gotHit = false;
    #endregion

    #region AILogic

    [Space(10)]
    [Header("AI Logic")]
    [Space(10)]

    AISensor _sensor;

    [SerializeField] Vector3 _walkPoint;
    bool _walkPointSet;
    [SerializeField] float _walkPointRange;
    [SerializeField] float _attackRange;
    [SerializeField] bool _playerInSightRange;
    [SerializeField] float _chaseDistance;
    [SerializeField] bool _chasing; public bool Chasing { get => _chasing; }
    [SerializeField] bool _playerInAttackRange;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] Vector3 _startPosition;

    [SerializeField] Color _patrolColor = Color.green;
    [SerializeField] Color _attackColor = Color.red;
    #endregion

    #region Paragraph

    [Space(10)]
    [Header("Paragraphs")]
    [Space(10)]
    [SerializeField] GameObject _paragraphs;
    [SerializeField] int _paragraphAmount;
    [SerializeField] float _explosionForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionUpward;


    #endregion

    #region Sound
    [Space(10)]
    [Header("Sound")]
    [Space(10)]
    [SerializeField] bool _maleTeacher;
    #endregion

    void Start()
    {
        _player = GameObject.Find("HerrAnwalt").transform;
        _rigidBody = GetComponent<Rigidbody>();
        _teacherCollider = GetComponent<CapsuleCollider>();
        _agent = GetComponent<NavMeshAgent>();
        _sensor = GetComponent<AISensor>();
        SetupAnimator();
        _animator.SetLayerWeight(_animHandsLayer, _defaultAnimationWeight);
        _aimConstraing = GetComponentInChildren<MultiAimConstraint>();

        SoundSetup();

        _headTargetDefault = _headTarget.position;
        _rigidBody.freezeRotation = true;
        _startPosition = transform.position;

        StartCoroutine(TauntRoutine());

        EventChannelSO.OnGameEnded += GameChecker;
        EventChannelSO.OnGameOver += GameChecker;
    }

    void Update()
    {
        _headTarget.position = _player.transform.position + Vector3.up * 1.578f;

        if (_dying)
        {
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 0, 0.5f));
            return;
        }

        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);


        if (_agent.velocity.x != 0 || _agent.velocity.z != 0)
            _animator.SetBool(_isWalkingHash, true);
        else
            _animator.SetBool(_isWalkingHash, false);


        Vector3 localSpeed = transform.InverseTransformDirection(_agent.velocity);
        _animator.SetFloat(_velocityXHash, localSpeed.x * _walkAnimMultiplicator);
        _animator.SetFloat(_velocityZHash, localSpeed.z * _walkAnimMultiplicator);

        if (!_sensor.InSight)
        {
            _animator.SetBool(InAttackHash, false);
            Patroling();
            _chasing = false;
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), _defaultAnimationWeight, 0.1f));
            return;
        }

        if (_sensor.InSight && !_playerInAttackRange && transform.position.magnitude - _startPosition.magnitude <= _walkPointRange)
        {
            _animator.SetBool(InAttackHash, false);
            ChasePlayer();
            _chasing = true;
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), _defaultAnimationWeight, 0.1f));
            return;
        }
        else if (!_playerInAttackRange && Chasing && transform.position.magnitude - _player.transform.position.magnitude <= _chaseDistance)
        {
            _animator.SetBool(InAttackHash, false);
            ChasePlayer();
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), _defaultAnimationWeight, 0.1f));
            return;
        }

        if (_sensor.InSight && _playerInAttackRange)
        {

            _animator.SetBool(InAttackHash, true);

            if (_gotHit)
                return;
            AttackPlayer();

            return;
        }
    }

    private void LateUpdate()
    {
        _animator.SetBool(GotHitHash, false);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Patroling()
    {
        _aimConstraing.weight = Mathf.Lerp(_aimConstraing.weight, 0, 10f * Time.deltaTime);
        if (!_walkPointSet)
        {
            StartCoroutine(SearchWalkPoint());
        }

        Debug.DrawLine(transform.position, _walkPoint, Color.red);
        _agent.speed = _maxWalkSpeed;

        if (_walkPointSet)
            _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude <= 0.5f)
            _walkPointSet = false;
    }

    IEnumerator SearchWalkPoint()
    {
        yield return new WaitForSeconds(Random.Range(0, 7));

        float randomeX = Random.Range(-_walkPointRange + 1, _walkPointRange - 1);
        float randomeZ = Random.Range(-_walkPointRange + 1, _walkPointRange - 1);

        _walkPoint = new Vector3(_startPosition.x + randomeX, 0, _startPosition.z + randomeZ);
        NavMeshHit hit;

        NavMesh.SamplePosition(_walkPoint, out hit, _walkPointRange * 2f, NavMesh.AllAreas);
        _walkPoint = hit.position;

        _walkPointSet = true;
    }



    void ChasePlayer()
    {
        _aimConstraing.weight = Mathf.Lerp(_aimConstraing.weight, 1, 30f * Time.deltaTime);

        _agent.SetDestination(_player.position);
        _agent.speed = _maxRunSpeed;


    }

    void AttackPlayer()
    {
        _aimConstraing.weight = Mathf.Lerp(_aimConstraing.weight, 1, 30f * Time.deltaTime);
        transform.LookAt(_player);

        if (_attackEnding)
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), _defaultAnimationWeight, 40f * Time.deltaTime));
        else if (_attackStarting)
            _animator.SetLayerWeight(_animHandsLayer, Mathf.Lerp(_animator.GetLayerWeight(_animHandsLayer), 1, 40f * Time.deltaTime));



        if (_isAttacking || _attackTimeout)
            return;


        _attackStarting = true;
        _isAttacking = true;
        _attackTimeout = true;

        Invoke(nameof(ResetAttackTime), Random.Range(1, _timeBetweenAttacks));

        _animator.SetBool(_isAttackHash, true);

        _audioPlayer.PlayOneShot(_soundEffectSO.TeacherAttack(_maleTeacher), _optionsSO.TeacherAttackVolume);

    }

    void TeacherAttackEnding()
    {
        _attackEnding = true;
        _attackStarting = false;

    }

    void TeacherAttackEnded()
    {
        _animator.SetBool(_isAttackHash, false);
        _attackEnding = false;
        _isAttacking = false;
        _alreadyHit = false;
    }

    void HitEnded()
    {
        _gotHit = false;
    }

    void ResetAttackTime()
    {
        _attackTimeout = false;
    }

    void Death()
    {
        if (_dying)
            return;



        _damageActive = false;

        StopCoroutine(TauntRoutine());
        _audioStreamPlayer.Stop();
        StreamAudio(_soundEffectSO.TeacherDeath(_maleTeacher), _optionsSO.TeacherDeathVolume);


        List<Rigidbody> paragraphs = new List<Rigidbody>();

        for (int i = 0; i < _paragraphAmount; i++)
        {
            float randome = Random.Range(-0.6f, 0.6f);

            GameObject instance = Instantiate(_paragraphs, transform.position + new Vector3(randome, randome, randome) + Vector3.up * 2f, Quaternion.identity);
            paragraphs.Add(instance.GetComponent<Rigidbody>());
        }

        foreach (Rigidbody rb in paragraphs)
        {
            rb.AddExplosionForce(_explosionForce, transform.position + Vector3.down * 0.4f, _explosionRadius, _explosionUpward);
        }

        _playerStatsSO.TeacherAlive--;
        if (_playerStatsSO.TeacherAlive <= 0)
            _playerStatsSO.PlayerHealth = 4;


        _chasing = false;
        _rigidBody.isKinematic = true;
        _teacherCollider.enabled = false;
        _dying = true;
        _deathNumber = Random.Range(0, _availableDeaths);
        _agent.enabled = false;
        _animator.SetFloat(DeathNumberHash, _deathNumber);
        _animator.SetBool(_isDyingHash, true);
        _sensor.enabled = false;
    }


    IEnumerator TauntRoutine()
    {
        if (Chasing && !_audioStreamPlayer.isPlaying)
        {
            StreamAudio(_soundEffectSO.TeacherTaunt(_maleTeacher), _optionsSO.TeacherTauntingVolume);
        }



        yield return new WaitForSeconds(Random.Range(10f, 15f));

        StartCoroutine(TauntRoutine());
    }




    void Deactivate()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;
    }

    void SetupAnimator()
    {
        _animator = GetComponent<Animator>();
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isAttackHash = Animator.StringToHash("isAttack");
        _inAttackHash = Animator.StringToHash("inAttack");
        _velocityXHash = Animator.StringToHash("velocityX");
        _velocityZHash = Animator.StringToHash("velocityZ");
        _isDyingHash = Animator.StringToHash("isDying");
        _deathNumberHash = Animator.StringToHash("deathNumber");
        _gotHitHash = Animator.StringToHash("gotHit");


    }

    private void OnTriggerEnter(Collider other)
    {

        PlayerStateMachine _playerStateMachine = _player.GetComponent<PlayerStateMachine>(); // other.GetComponentInParent<PlayerStateMachine>();



        if (other.CompareTag("BookWeapon") && _playerStateMachine.IsAttacking && !_playerStateMachine.HitTarget)
        {
            Health--;
            _gotHit = true;
            _animator.SetBool(GotHitHash, true);
            _playerStateMachine.HitTarget = true;
            _audioStreamPlayer.Stop();
            _audioPlayer.PlayOneShot(_soundEffectSO.TeacherGotHit(_maleTeacher), _optionsSO.TeacherGotHitVolume);
        }

        if (other.CompareTag("Fence"))
        {
            Health--;
            _audioStreamPlayer.Stop();
            _audioPlayer.PlayOneShot(_soundEffectSO.TeacherGotHit(_maleTeacher), _optionsSO.TeacherGotHitVolume);
            _audioPlayer.PlayOneShot(_soundEffectSO.Fence(), _optionsSO.FenceHitVolume);
        }

        if (Health <= 0)
            Death();
    }


    void SoundSetup()
    {
        _audioPlayer = gameObject.AddComponent<AudioSource>();
        _audioPlayer.spatialBlend = 1;
        _audioPlayer.maxDistance = 20;
        _audioStreamPlayer = gameObject.AddComponent<AudioSource>();
        _audioStreamPlayer.spatialBlend = 0.5f;
    }

    void StreamAudio(AudioClip clip, float volume)
    {
        _audioStreamPlayer.clip = clip;
        _audioStreamPlayer.volume = volume;
        _audioStreamPlayer.Play();
    }

    void ActivateDamage()
    {
        _damageActive = true;
    }

    void DeactivateDamage()
    {
        _damageActive = false;
    }
#if (UNITY_EDITOR)
    private void OnDrawGizmosSelected()
    {
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = _patrolColor;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), _walkPointRange);
        }
        else if (EditorApplication.isPlaying)
        {
            Gizmos.color = _patrolColor;
            Gizmos.DrawWireSphere(_startPosition + new Vector3(0, 1, 0), _walkPointRange);
        }

        Gizmos.color = _attackColor;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), _attackRange);
    }

#endif
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




