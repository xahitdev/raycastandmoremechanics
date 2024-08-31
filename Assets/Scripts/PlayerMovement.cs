using System;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("MOVING AND LOOKING")]
    [Space]
    [SerializeField] private float speed = 10f; // Movement speed
    public Transform playerBody; // Reference to the player's body (to rotate)

    private CharacterController characterController; // Reference to the characterController component
    private bool isGrounded = false;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpHeight;
    public float gravity = -9.81f;

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
    [SerializeField] private float crouchedPlayerSpeed;
    [SerializeField] private float standPlayerSpeed;
    private bool isCrouching = false;

    [Header("Interaction")]
    [Space]
    [SerializeField] private KeyCode InteractionKey;
    [SerializeField] private float playerInteractionDistance;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        //CROUCHED SPEED MODIFIER

        speed = isCrouching ? crouchedPlayerSpeed : standPlayerSpeed;

        //MAKE IS SMOOTHER 
        characterController.center = isCrouching ? new Vector3(0,0,0) : new Vector3(0,0,0);
        //MAKE IS SMOOTHER 

        //CROUCING MANAGER

        float targetHeight = isCrouching ? crouchedHeight : playerHeight;
        float currentHeight = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchSpeed);
        characterController.height = currentHeight;
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

