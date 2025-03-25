﻿using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DATN
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        [Tooltip("Acceleration and deceleration")]
        public float RotationSpeedY = 4f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        //Write to test
        // public CinemachineVirtualCamera aimCam;
        public GameObject aimPos;
        public GameObject targetAimPoint;
        public GameObject cameraAim;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        //my param
        // private float duationClick = 0 ; 

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private MyControllInputs _input;
        private GameObject _mainCamera;
        private PlayerController playerController;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private bool _isHoldToSpinAttack;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
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
            GameManager.Instance.SetPlayerMovementController(this);
            playerController = gameObject.GetComponent<PlayerController>();
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<MyControllInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            if(playerController.PlayerIsDie){
                return;
            }
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            // Attack();
            if(_isHoldToSpinAttack){
                return;
            }
            Move();
        }

        private void LateUpdate()
        {
// #if UNITY_EDITOR
//             CameraRotation();
// #else
// 			CameraRotationWithTouch();
// #endif
        CameraRotationWithTouch();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                // Tính toán góc quay camera
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // Clamp rotation để giới hạn góc quay của camera
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Xoay camera dựa trên góc tính toán
            // if(duationClick>=0.2){
            //     aimPos.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
            //     return;
            // }
            aimPos.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
            targetAimPoint.transform.position = GetSymmetricPoint(cameraAim.transform.position,aimPos.transform.position);
            targetAimPoint.transform.position-=new Vector3(0,0.5f,0);
        }
        Vector3 GetSymmetricPoint(Vector3 A, Vector3 O)
    {
        return new Vector3(
            2 * O.x - A.x,
            2 * O.y - A.y,
            2 * O.z - A.z
        );
    }
        #region RotateWithTouch

        TouchItem touchTarget;
        Touch touchTemp;
        Vector2 StartPosTouch;
        private bool _isForceRotate;

        private void CameraRotationWithTouch()
        {
            int i = 0;
            if (Input.touchCount > 0 && touchTarget == null)
            {
                while (i < Input.touchCount)
                {
                    touchTemp = Input.touches[i++];
                    //Debug.Log($"-------------TOUCH INFO---------------: {touchTemp.phase}");

                    if (touchTemp.phase == UnityEngine.TouchPhase.Began)
                    {
                        if (IsPointerOverUIObject(touchTemp.position))
                        {
                            continue;
                        }

                        touchTarget = new TouchItem(touchTemp.fingerId, touchTemp);
                        StartPosTouch = touchTemp.position;
                        //Debug.Log($"-------------TOUCH Began---------------: {StartPosTouch}");
                        break;
                    }
                }
            }

            if (touchTarget != null)
            {
                i = 0;

                while (i < Input.touchCount)
                {
                    Touch touch = Input.touches[i];
                    if (touch.fingerId == touchTarget.TouchId)
                    {
                        if (touch.phase == UnityEngine.TouchPhase.Ended)
                        {
                            touchTarget = null;
                            //Debug.Log($"-------------TOUCH Ended---------------: {StartPosTouch}");
                        }

                        if (touch.phase == UnityEngine.TouchPhase.Moved)
                        {
                            Vector2 defaultPos = (touch.position - StartPosTouch);
                            if (defaultPos.magnitude > 0.01f)
                            {
                                //Debug.Log($"-------------TOUCH Moved---------------: {defaultPos}");

                                // Debug.Log($"GameManager.Instance.ratioRotateSpeed {GameManager.Instance.RatioRotateSpeed}");

                                //Temp setup to test
                                // _cinemachineTargetYaw += defaultPos.x * RotationSpeed * Time.deltaTime * 2f;
                                // _cinemachineTargetPitch += -defaultPos.y * RotationSpeed * Time.deltaTime * 2f;
                                //Temp setup to test
                                _cinemachineTargetYaw += defaultPos.x * RotationSpeed * Time.deltaTime * GameManager.Instance.RatioRotateSpeed;
                                _cinemachineTargetPitch += -defaultPos.y * RotationSpeed * Time.deltaTime * GameManager.Instance.RatioRotateSpeed;
                                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
                                //CinemachineCameraTarget.transform.Rotate(Vector3.right * * Time.deltaTime);

                                // CinemachineCameraTarget.transform.localRotation =
                                //     Quaternion.Euler(_cinemachineTargetPitch, 0, 0);
                                aimPos.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                                CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                            }
                        }

                        StartPosTouch = touch.position;
                        return;
                    }

                    i++;
                }
            }
        }


        private static bool IsPointerOverUIObject(Vector2 PosTouch)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(PosTouch.x, PosTouch.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            //int index = 0;
            // foreach (var item in results)
            // {
            //     Debug.Log($"-------------TOUCH UI: index = {index++} => {item.gameObject.name}");
            // }
            int count = 0;
            for (int i = 0; i < results.Count; i++)
            {
                if(results[i].gameObject.tag == "ButtonAttack")
                {
                    return false;
                }
            }


            count = results.Count - count;

            return count > 0;
        }
        private static bool IsPointerOverUIObject(Vector2 PosTouch, int leyerUIImmersiveAds)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(PosTouch.x, PosTouch.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            //int index = 0;
            // foreach (var item in results)
            // {
            //     Debug.Log($"-------------TOUCH UI: index = {index++} => {item.gameObject.name}");
            // }
            int count = 0;
            for (int i = 0; i < results.Count; i++)
            {
                if(results[i].gameObject.layer == leyerUIImmersiveAds)
                {
                    count++;
                }
            }

            count = results.Count - count;

            return count > 0;
        }

        #endregion

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // Xoay nhân vật theo góc của camera
            transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetYaw, 0.0f);

            // Di chuyển nhân vật theo input
            Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

            // move the player
            _controller.Move(moveDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
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
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        // private void Attack(){
        //     if(_input.startAttack==true){
        //         if(_input.confirmAttack==false){
        //             if(duationClick<0.2&&duationClick+Time.deltaTime>=0.2){
        //                 aimCam.enabled = true;
        //             }
        //             duationClick += Time.deltaTime;
        //         }else{
        //             if(duationClick<=0.2){
        //                 Debug.Log("Short click attack");
        //                 _animator.SetTrigger("Attack");
        //                 _input.startAttack = false;
        //                 _input.confirmAttack = false;
        //                 duationClick = 0;
        //             }else{
        //                 Debug.Log("Long hold trigger attack: "+ duationClick);
        //                 _input.startAttack = false;
        //                 _input.confirmAttack = false;
        //                 duationClick = 0;
        //                 aimCam.enabled = false;
        //             }
        //         }
        //     }
        // }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
        public void SetIsHoldToSpinAttack(bool isHoldToSpinAttack){
            _isHoldToSpinAttack = isHoldToSpinAttack;
        }
    }
    [SerializeField]
    public class TouchItem
    {
        public int TouchId;
        public Touch Touch;

        public TouchItem()
        {
        }

        public TouchItem(int touchId, Touch touch)
        {
            TouchId = touchId;
            Touch = touch;
        }
    }
}