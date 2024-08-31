using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    private CharacterController characterController;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float sensitivityMultiplier;
    public Transform playerCamera;
    // Start is called before the first frame update
    private float xRotation = 0f;
    public bool cameraLockControl = false;
    public static CameraController isCameraActive;

    private void Awake() {
        if(isCameraActive == null){
            isCameraActive = this;
        }
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
        Cursor.visible = false; // Hide cursor
    }
    void Update()
    {
        if(cameraLockControl){
            return;
        }
        CameraLook();
    }
    public void CameraLook()
    {
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
}
