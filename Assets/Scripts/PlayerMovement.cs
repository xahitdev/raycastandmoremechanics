using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("MOVING AND LOOKING")]
    [Space]
    [SerializeField] private float speed = 10f; // Movement speed
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public Transform playerBody; // Reference to the player's body (to rotate)
    public Transform playerCamera; // Reference to the camera (to rotate)

    private float xRotation = 0f; // Current x rotation for the camera

    private Rigidbody rb; // Reference to the Rigidbody component

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
    private bool isCrouching = false;

    [Header("Interaction")]
    [Space]
    [SerializeField] private KeyCode InteractionKey;
    [SerializeField] private float playerInteractionDistance;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 moveDirection = playerBody.transform.TransformDirection(movement); // Transform direction relative to the player's rotation
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

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
        if (Input.GetKeyDown(crouchKey) && !isCrouching)
        {
            playerToCrouch.GetComponent<CapsuleCollider>().height = crouchedHeight;
            isCrouching = true;
        }
        else if (Input.GetKeyDown(crouchKey) && isCrouching)
        {
            playerToCrouch.GetComponent<CapsuleCollider>().height = playerHeight;
            isCrouching = false;
        }

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

