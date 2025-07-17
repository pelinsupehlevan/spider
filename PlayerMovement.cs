using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Transform cameraHolder;

    [Header("Jump and Gravity Settings")]
    public float gravity = -50f;                 // Stronger gravity for fast fall
    public float jumpHeight = 6f;                // Jump height
    public float fallMultiplier = 2.5f;          // Faster falling
    public float lowJumpMultiplier = 5f;         // Shorter jump if releasing jump early

    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundChecker;
    public float groundCheckerRadius = 0.3f;
    public LayerMask groundLayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = 0f;

        if (cameraHolder != null)
        {
            cameraHolder.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        HandleCameraLook();
        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleCameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        Vector3 moveInputs = Input.GetAxis("Horizontal") * transform.right +
                             Input.GetAxis("Vertical") * transform.forward;

        Vector3 moveVelocity = moveInputs * speed;

        if (!isGrounded)
            moveVelocity *= 0.8f;

        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, groundLayer);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
