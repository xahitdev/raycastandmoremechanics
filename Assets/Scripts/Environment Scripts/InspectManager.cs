using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class InspectManager : MonoBehaviour, IInteractable
{
    public CameraController cameraScript;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float zoomedFOV;
    private bool isInspecting;
    public void Interact()
    {
        ItemInspect();
    }

    private void ItemInspect()
    {
        CameraController.isCameraActive.cameraLockControl = true;
        // Camera.main.transform.LookAt(this.gameObject.transform.position);
        if (!isInspecting)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Camera.main.fieldOfView = zoomedFOV;
            isInspecting = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Camera.main.fieldOfView = defaultFOV;
            isInspecting = false;
            CameraController.isCameraActive.cameraLockControl = false;
        }
    }
    private void Spawn()
    {

    }
}
