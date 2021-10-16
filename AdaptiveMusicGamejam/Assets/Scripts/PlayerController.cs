using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform head;
    [SerializeField] Transform body;
    [SerializeField] CharacterController cc;
    Camera cam;

    [Header("Features")]
    [SerializeField] bool willSlideOnSlopes = true;
    [SerializeField] bool rotateBodyHorizontally = true;

    [Header("Movement Settings")]
    [SerializeField] float gravity = -15;
    [SerializeField] float walkSpeed = 6;
    [SerializeField] float runSpeed = 10;
    [SerializeField] float slopeSlideSpeed = 15f;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 3;

    [Header("Look Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxLookUpAngle = 90f;
    [SerializeField] float maxLookDownAngle = 90f;


    float verticalLookAngle = 0;
    float horizontalLookAngle = 0;

    Vector3 velocity;
    bool isSliding = false;

    public float CharacterHeight
    {
        get => _currentCharacterHeight;
        set
        {
            cc.height = value;
            _currentCharacterHeight = value;
        }
    }

    public float CharacterWidth
    {
        get => _currentCharacterWidth;
        set
        {
            cc.radius = value;
            _currentCharacterWidth = value;
        }
    }

    [SerializeField] float _currentCharacterHeight;
    [SerializeField] float _currentCharacterWidth;
    [SerializeField] LayerMask groundMask;

    private void Awake()
    {
        //if (con == null) con = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();

        if (head == null) head = transform;
        if (body == null) body = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    void Update()
    {
        //Input
        Vector2 inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 inputDirections = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool inputRun = Input.GetButton("Run");
        bool inputJump = Input.GetButtonDown("Jump");

        //Rotation
        Vector2 angleDelta = inputMouse * mouseSensitivity * Time.deltaTime;

        horizontalLookAngle += angleDelta.x;
        verticalLookAngle = Mathf.Clamp(verticalLookAngle - angleDelta.y, -maxLookUpAngle, maxLookDownAngle);

        ApplyRotation(horizontalLookAngle, verticalLookAngle);

        //Sliding
        HandleSliding();

        //Calculate movement
        Vector3 walkDirection = (body.right * inputDirections.x + body.forward * inputDirections.y).normalized;
        Vector3 horizontalMovement = walkDirection * (inputRun ? runSpeed : walkSpeed);


        //Apply gravity
        velocity.y += gravity * Time.deltaTime;

        if (!isSliding)
        {
            //Apply movement
            velocity.x = horizontalMovement.x;
            velocity.z = horizontalMovement.z;



            //Reset Y velocity
            if (cc.isGrounded && velocity.y < -1) velocity.y = 0;

            //Jump
            if (cc.isGrounded && inputJump && !isSliding) velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Apply movement
        cc.Move(velocity * Time.deltaTime);
    }

    private void OnValidate()
    {
        CharacterHeight = _currentCharacterHeight;
        CharacterWidth = _currentCharacterWidth;
    }

    private void HandleSliding()
    {
        isSliding = false;
        if (willSlideOnSlopes)
        {
            if (!cc.isGrounded) return;
            if (cc.velocity.y > 0) return;
            if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, CharacterHeight * 2)) return;
            if (Vector3.Angle(slopeHit.normal, Vector3.up) <= cc.slopeLimit) return;


            isSliding = true;
            velocity += Quaternion.AngleAxis(90, Vector3.Cross(Vector3.up, slopeHit.normal)) * (slopeHit.normal * slopeSlideSpeed) * Time.deltaTime;
        }
    }

    private void ApplyRotation(float horizontalAngle, float verticalAngle)
    {
        if (rotateBodyHorizontally)
        {
            head.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
            body.rotation = Quaternion.Euler(0, horizontalAngle, 0);
        }
        else
        {
            head.localRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        }
    }

}
