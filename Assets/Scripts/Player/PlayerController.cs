using Cinemachine;
using InputSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")] 
        [SerializeField]
        private float _moveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")] 
        [SerializeField]
        private float _sprintSpeed = 6.0f;
        [Tooltip("Rotation speed of the character")] 
        [SerializeField]
        private float _rotationSpeed = 1.0f;
        [Tooltip("Acceleration and deceleration")] 
        [SerializeField]
        private float _speedChangeRate = 10.0f;
        [Tooltip("The height the player can jump")] 
        [SerializeField]
        private float _jumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")] 
        [SerializeField]
        private float _gravity = -15.0f;
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField]
        private float _jumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField]
        private float _fallTimeout = 0.15f;
        
        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField]
        private bool _grounded = true;
        [Tooltip("Useful for rough ground")] 
        [SerializeField]
        private float _groundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField]
        private float _groundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")] 
        [SerializeField]
        private LayerMask _groundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField]
        private GameObject _cinemachineCameraTarget;
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField]
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [Tooltip("How far in degrees can you move the camera up")] 
        [SerializeField]
        private Vector2 _yClamp = new Vector2(-60, 60);

        // cinemachine
        private float _cinemachineTargetPitch;
        private Quaternion rotationCharacter;
        private Quaternion rotationCamera;

        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // Animator
        private int _xVelHash;
        private int _yVelHash;

        [SerializeField] public GameObject CharacterRig;
        private Animator _animator;
        private CharacterController _controller;
        private PlayerInputController _input;
        private GameObject _mainCamera;

        [SerializeField] private Weapon _weapon;
        private Coroutine fireCoroutine;

        public override void OnNetworkSpawn()
        {
            //If this is not the owner, turn of player inputs
            if (!IsOwner) gameObject.GetComponent<PlayerInput>().enabled = false;
            _cinemachineVirtualCamera.Priority = IsOwner ? 10 : 0;
            GameManager.Instance.SetSessionInfo();
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _animator = CharacterRig.GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputController>();

            if (IsClient && IsOwner)
            {
                Transform newtransform = LevelManager.Instance.GetRandomSpawn();
                transform.position = newtransform.position;
                transform.rotation = newtransform.rotation;
                _mainCamera.transform.position = newtransform.position;
                _mainCamera.transform.rotation = newtransform.rotation;
            }

            // reset our timeouts on start
            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;

            // Animator Hashes
            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
        }

        private void Update()
        {
            if (!IsOwner) return; //If this is not the owner, skip Update()
            if (GameManager.Instance.IsPlaying)
            {
                JumpAndGravity();
                GroundedCheck();
                Move();
                if (_input.shoot)
                {
                    StartFiring();
                    _input.shoot = false;
                }
            }
        }

        private void LateUpdate()
        {
            if (!IsOwner) return; //If this is not the owner, skip LateUpdate()
            if (GameManager.Instance.IsPlaying) CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
                transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= 0.01f) // if there is an input
            {
                _cinemachineTargetPitch += _input.look.y * _rotationSpeed;
                _rotationVelocity = _input.look.x * _rotationSpeed;
                
                // Update Cinemachine camera target pitch
                _cinemachineTargetPitch =
                    Mathf.Clamp(_cinemachineTargetPitch, _yClamp.x, _yClamp.y); // clamp our pitch rotation
                _cinemachineCameraTarget.transform.localRotation =
                    Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
                
                // Rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? _sprintSpeed : _moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * _speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            // move the player
            Vector3 motion = inputDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
            _controller.Move(motion);

            _animator.SetFloat(_xVelHash, _input.move.x * _speed);
            _animator.SetFloat(_yVelHash, _input.move.y * _speed);
        }

        private void JumpAndGravity()
        {
            if (_grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = _fallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = _jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z),
                _groundedRadius);
        }

        private void StartFiring()
        {
            fireCoroutine = StartCoroutine(_weapon.RapidFire());
        }

        private void StopFiring()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }
        }
    }
}