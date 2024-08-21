using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("MOVING AND LOOKING")]
    [Space]
    [SerializeField] private float speed = 10f; // Movement speed
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public Transform playerBody; // Reference to the player's body (to rotate)
    public Transform playerCamera; // Reference to the camera (to rotate)

    private float xRotation = 0f; // Current x rotation for the camera

    private CharacterController characterController; // Reference to the characterController component
    private bool isGrounded = false;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpHeight;
    public float gravity = -9.81f;
    [SerializeField] private float sensitivityMultiplier;

    [Header("LEANING RIGHT AND LEFT")]
    [Space]
    [SerializeField] private KeyCode leanLeftKey;
    [SerializeField] private KeyCode leanRightKey;
    [SerializeField] Transform leanPivot;
    [SerializeField] float leanRotationDegree;

    [Header("LEAN CHECKER")]
    [Space]
    [SerializeField] private LayerMask wall;
    [SerializeField] private float leanSpaceDistance;

    [Header("Crouching")]
    [Space]
    [SerializeField] Transform playerToCrouch;
    [SerializeField] private KeyCode crouchKey;
    [SerializeField] private float playerHeight;
    [SerializeField] private float crouchedHeight;
    [SerializeField] private float crouchSpeed;
    private bool isCrouching = false;

    [Header("Interaction")]
    [Space]
    [SerializeField] private KeyCode InteractionKey;
    [SerializeField] private float playerInteractionDistance;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
        Cursor.visible = false; // Hide cursor
    }

    void Update()
    {
        Moving();
        Leaning();
        Crouching();
        Interaction();
    }

    //   __  __  ______      _______ _   _  _____ 
    //  |  \/  |/ __ \ \    / /_   _| \ | |/ ____|
    //  | \  / | |  | \ \  / /  | | |  \| | |  __ 
    //  | |\/| | |  | |\ \/ /   | | | . ` | | |_ |
    //  | |  | | |__| | \  /   _| |_| |\  | |__| |
    //  |_|  |_|\____/   \/   |_____|_| \_|\_____|


    private void Moving()
    {
        // Check if the player is grounded
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small downward force to keep grounded
        }

        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the player's orientation
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the player
        characterController.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player downward due to gravity
        characterController.Move(velocity * Time.deltaTime);
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * sensitivityMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * sensitivityMultiplier;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    //   _      ______          _   _ _____ _   _  _____ 
    //  | |    |  ____|   /\   | \ | |_   _| \ | |/ ____|
    //  | |    | |__     /  \  |  \| | | | |  \| | |  __ 
    //  | |    |  __|   / /\ \ | . ` | | | | . ` | | |_ |
    //  | |____| |____ / ____ \| |\  |_| |_| |\  | |__| |
    //  |______|______/_/    \_\_| \_|_____|_| \_|\_____|

    private void Leaning()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hit, leanSpaceDistance, wall))
        {
        }
        else
        {
            if (Input.GetKey(leanRightKey))
            {
                LeanRight();
            }
            else { LeanDefault(); }
        }
        if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hit, leanSpaceDistance, wall))
        {
        }
        else
        {
            if (Input.GetKey(leanLeftKey))
            {
                LeanLeft();
            }
            else { LeanDefault(); }
        }
    }
    private void LeanLeft()
    {
        Quaternion newRot = Quaternion.Euler(leanPivot.transform.eulerAngles.x, leanPivot.transform.eulerAngles.y, leanRotationDegree);
        leanPivot.rotation = Quaternion.Slerp(leanPivot.rotation, newRot, Time.deltaTime * 5f);

    }
    private void LeanRight()
    {
        Quaternion newRot = Quaternion.Euler(leanPivot.transform.eulerAngles.x, leanPivot.transform.eulerAngles.y, -leanRotationDegree);
        leanPivot.rotation = Quaternion.Slerp(leanPivot.rotation, newRot, Time.deltaTime * 5f);

    }
    private void LeanDefault()
    {
        Quaternion newRot = Quaternion.Euler(leanPivot.transform.eulerAngles.x, leanPivot.transform.eulerAngles.y, 0);
        leanPivot.rotation = Quaternion.Slerp(leanPivot.rotation, newRot, Time.deltaTime * 5f);
    }

    //    _____ _____   ____  _    _  _____ _    _ _____ _   _  _____ 
    //   / ____|  __ \ / __ \| |  | |/ ____| |  | |_   _| \ | |/ ____|
    //  | |    | |__) | |  | | |  | | |    | |__| | | | |  \| | |  __ 
    //  | |    |  _  /| |  | | |  | | |    |  __  | | | | . ` | | |_ |
    //  | |____| | \ \| |__| | |__| | |____| |  | |_| |_| |\  | |__| |
    //   \_____|_|  \_\\____/ \____/ \_____|_|  |_|_____|_| \_|\_____|


    private void Crouching()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
        }
        float targetHeight = isCrouching ? crouchedHeight : playerHeight;
        float currentHeight = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchSpeed);

        characterController.height = currentHeight;
        characterController.center = new Vector3(0, currentHeight / 2, 0);
    }

    //   _____ _   _ _______ ______ _____            _____ _______ _____ ____  _   _ 
    //  |_   _| \ | |__   __|  ____|  __ \     /\   / ____|__   __|_   _/ __ \| \ | |
    //    | | |  \| |  | |  | |__  | |__) |   /  \ | |       | |    | || |  | |  \| |
    //    | | | . ` |  | |  |  __| |  _  /   / /\ \| |       | |    | || |  | | . ` |
    //   _| |_| |\  |  | |  | |____| | \ \  / ____ \ |____   | |   _| || |__| | |\  |
    //  |_____|_| \_|  |_|  |______|_|  \_\/_/    \_\_____|  |_|  |_____\____/|_| \_|

    private void Interaction()
    {
        if (Input.GetKeyDown(InteractionKey))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactObject))
                {
                    interactObject.Interact();
                }
            }
        }
    }
}

