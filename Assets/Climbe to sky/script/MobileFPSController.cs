using UnityEngine;
using UnityEngine.UI;

public class MobileFPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;

    [Header("Look Settings")]
    public float lookSensitivity = 2f;
    private float yRotation;

    [Header("UI Controls")]
    public FixedJoystick movementJoystick;
    public FixedJoystick lookJoystick;
    public Button jumpButton;

    [Header("Camera & Ground Check")]
    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.3f;

    private bool isGrounded;
    private bool canDoubleJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock X and Z rotations in Rigidbody to prevent tipping
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // Hook up jump button
        jumpButton.onClick.AddListener(Jump);
    }

    void Update()
    {
        HandleLook();
        GroundCheck();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Get joystick input for movement
        float horizontal = movementJoystick.Horizontal;
        float vertical = movementJoystick.Vertical;

        // Calculate move direction relative to player’s facing direction
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection *= moveSpeed;

        // Preserve existing Y velocity (gravity)
        moveDirection.y = rb.linearVelocity.y;

        // Apply velocity to Rigidbody
        rb.linearVelocity = moveDirection;
    }

    void HandleLook()
    {
        // Get joystick input for look direction
        float lookX = lookJoystick.Horizontal * lookSensitivity;
        float lookY = lookJoystick.Vertical * lookSensitivity;

        // Rotate player left/right (Y axis)
        transform.Rotate(0f, lookX, 0f);

        // Rotate camera up/down (X axis)
        yRotation -= lookY;
        yRotation = Mathf.Clamp(yRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    void Jump()
    {
        if (isGrounded)
        {
            // First jump
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            canDoubleJump = true;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayJumpSound();
        }
        else if (canDoubleJump)
        {
            // Double jump
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            canDoubleJump = false;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayJumpSound();
        }
    }

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.CheckCollision(other.gameObject);
        }
    }
}
