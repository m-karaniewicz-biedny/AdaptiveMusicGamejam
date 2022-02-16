using System;
using UnityEngine;

/* !NOTES!
 * !!!Work in progress!!!
 * Author: Silvarc
 */


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Features")]
    [SerializeField] private bool rotateHeadOnly = false;

    [Header("Movement Settings")]
    [SerializeField] private float gravity = 30;

    [SerializeField] private float maxFallSpeed = 25f;
    [SerializeField] private float baseAccelerationTimeGround = 0.075f;
    [SerializeField] private float baseAccelerationTimeAir = 0.3f;
    //[SerializeField] private float baseAccelerationTimeSlide = 1f;
    [SerializeField] private float frictionTime = 0.075f;
    [SerializeField] private float maxWalkSpeed = 6;
    [SerializeField] private float maxRunSpeed = 10;
    //[SerializeField] private float slopeSlideAccelerationTime = 1f;
    //[SerializeField] private float maxSlopeSlideSpeed = 15f;
    //[SerializeField] private float slopeSlideLimit = 45;
    [SerializeField] private float _slopeHardLimit = 75;
    [SerializeField] private float stickToGroundForce = 10;
    [SerializeField] private float minVelocity = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 3;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private float maxLookUpAngle = 75f;
    [SerializeField] private float maxLookDownAngle = 45f;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float _characterHeight = 2;
    [SerializeField] private float _characterWidth = 1;

    [Header("References")]
    [SerializeField] private Transform head;

    [SerializeField] private Transform body;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera cam;

    public float CharacterHeight
    {
        get => _characterHeight;
        set
        {
            characterController.height = value;
            _characterHeight = value;
        }
    }

    public float CharacterWidth
    {
        get => _characterWidth;
        set
        {
            characterController.radius = value;
            _characterWidth = value;
        }
    }

    public float SlopeHardLimit
    {
        get => _slopeHardLimit;
        set
        {
            characterController.slopeLimit = value;
            _slopeHardLimit = value;
        }
    }

    public bool IsGrounded
    {
        get
        {
            if (!characterController.isGrounded) return false;

            if (Vector3.Angle(slopeNormal, Vector3.up) <= SlopeHardLimit )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private Vector3 velocity;

    //Look
    private float verticalLookAngle = 0;
    private float horizontalLookAngle = 0;
    
    //Collision
    private Vector3 slopeNormal;
    private bool groundDetected = false;
    private bool IsGroundedLastFrame = false;

    //Input
    private Vector2 inputMouse;
    private Vector2 inputDirections;
    private bool inputRun;
    private bool inputJump;

    private void Awake()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();

        if (cam == null) cam = GetComponentInChildren<Camera>();

        if (head == null) head = transform;
        if (body == null) body = transform;

        characterController.slopeLimit = 180;
        characterController.stepOffset = 0.3f;
        //characterController.skinWidth = 0.08f;
        characterController.minMoveDistance = 0.001f;
        characterController.center = Vector3.zero;

        CharacterHeight = _characterHeight;
        CharacterWidth = _characterWidth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateInput();

        HandleRotation();

        RaycastGround();

        //if (IsGroundedLastFrame == true && IsGrounded == false) OnWalkOffAnEdge();

        ApplyBaseMovement();

        HandleSlopes();

        ApplyFriction();

        //Apply gravity
        if (velocity.y < maxFallSpeed) velocity.y += -gravity * Time.deltaTime;

        //Force that makes moving down slope more fluid
        if (IsGrounded) velocity.y = -stickToGroundForce;

        //Debug.Log($"grounded: {IsCollidingWithGround} sliding: {isSliding}");
        //Debug.Log($"g: {IsGrounded} glf: {IsGroundedLastFrame}");

        //Land
        if (IsGroundedLastFrame == false && IsGrounded == true) OnLandOnGround();

        //Jump
        if (IsGrounded && inputJump) OnJump(); 

        //Zero out velocity when too small to reduce jitter
        if (velocity.magnitude < minVelocity) velocity = Vector3.zero;

        //Last frame values
        IsGroundedLastFrame = IsGrounded;

        //Debug.Log($"Magnitude: {velocity.magnitude}\nDirection: {velocity.normalized}");

        //Apply velocity
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnValidate()
    {
        CharacterHeight = _characterHeight;
        CharacterWidth = _characterWidth;
        SlopeHardLimit = _slopeHardLimit;
    }

    private void HandleSlopes()
    {
        if (!IsGrounded && groundDetected)// && characterController.velocity.y <= 0)
        { 
            if (Vector3.Angle(slopeNormal, Vector3.up) > SlopeHardLimit)
            {
                velocity = Vector3.ProjectOnPlane(velocity, slopeNormal);
            }
        }

        //private void SlideOnSlope()
        //{
        //    float slideAccel = 1 / slopeSlideAccelerationTime * Time.deltaTime * maxSlopeSlideSpeed;
        //    Vector3 slopeDir = Quaternion.AngleAxis(90, Vector3.Cross(Vector3.up, slopeNormal)) * slopeNormal;
        //    Vector3 velDelta = GetLimitedVelocityDelta(velocity, slopeDir * slideAccel, maxSlopeSlideSpeed);
        //    velocity += velDelta;
        //}
    }

    private void UpdateInput()
    {
        inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        inputDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputRun = Input.GetButton("Run");
        inputJump = Input.GetButtonDown("Jump");
    }

    private void ApplyFriction()
    { 
        if (!IsGrounded || inputDirections != Vector2.zero) return;

        float friction = 1 / frictionTime * Time.deltaTime * maxWalkSpeed;
        velocity -= velocity.normalized * friction;
    }

    private void HandleRotation()
    {
        Vector2 angleDelta = mouseSensitivity * Time.deltaTime * inputMouse;

        horizontalLookAngle += angleDelta.x;
        verticalLookAngle = Mathf.Clamp(verticalLookAngle - angleDelta.y, -maxLookUpAngle, maxLookDownAngle);

        ApplyRotation(horizontalLookAngle, verticalLookAngle);
    }

    private void ApplyBaseMovement()
    {
        //Calculate movement
        Vector3 targetDirection = (body.right * inputDirections.x + body.forward * inputDirections.y).normalized;

        Vector3 hVelocity = new Vector3(velocity.x, 0, velocity.z);

        float maxSpeed = (inputRun ? maxRunSpeed : maxWalkSpeed);

        float acceleration = 1 / GetAccelerationTime() * Time.deltaTime * maxSpeed;

        Vector3 force = targetDirection * acceleration;

        if (IsGrounded) force = Vector3.ProjectOnPlane(force, slopeNormal);

        //Limit velocity
        Vector3 limitedVelocityDelta = GetLimitedVelocityDelta(hVelocity, force, maxSpeed);

        //Apply movement
        velocity.x += limitedVelocityDelta.x;
        velocity.z += limitedVelocityDelta.z;

        //velocity = Vector3.ProjectOnPlane(velocity, slopeNormal);

        #region Limit only velocity projection

        //Limit velocity projection

        //float dotProjection = Vector3.Dot(hVelocity, targetDirection);

        //if (dotProjection + acceleration > maxSpeed)
        //{
        //    acceleration = maxSpeed - dotProjection;
        //}

        //finalMovementVelocityDelta = targetDirection * acceleration;

        #endregion Limit only velocity projection
    }

    private void RaycastGround()
    {
        //Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, CharacterHeight * 2);
        bool hit = Physics.SphereCast(transform.position, CharacterWidth, Vector3.down,
            out RaycastHit slopeHit, characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        groundDetected = hit;
        if (!hit) return;

        slopeNormal = slopeHit.normal;
    }

    private void ApplyRotation(float horizontalAngle, float verticalAngle)
    {
        if (rotateHeadOnly)
        {
            head.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        }
        else
        {
            head.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
            body.rotation = Quaternion.Euler(0, horizontalAngle, 0);
        }
    }

    private float GetAccelerationTime()
    {
        if (IsGrounded) return baseAccelerationTimeGround;
        else return baseAccelerationTimeAir;
    }

    Vector3 GetLimitedVelocity(Vector3 _velocity, float _maxSpeed)
    {
        if (_velocity.magnitude > _maxSpeed) return _velocity - _velocity.normalized * (_velocity.magnitude - _maxSpeed);
        else return _velocity;
    }

    Vector3 GetLimitedVelocityDelta(Vector3 _originalVelocity, Vector3 _addedForce, float _maxSpeed)
    {
        return GetLimitedVelocity(_originalVelocity + _addedForce, _maxSpeed) - _originalVelocity;
    }

    void OnJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
    }

    void OnLandOnGround()
    {

    }

    //void OnWalkOffAnEdge()
    //{
    //    //velocity.y = 0;
    //}
}