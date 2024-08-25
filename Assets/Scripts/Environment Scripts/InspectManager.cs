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
        if (!isInspecting)
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Camera.main.fieldOfView = zoomedFOV;
            cameraScript.enabled = false;
            isInspecting = true;
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Camera.main.fieldOfView = defaultFOV;
            cameraScript.enabled = true;
            isInspecting = false;
        }
    }
}
