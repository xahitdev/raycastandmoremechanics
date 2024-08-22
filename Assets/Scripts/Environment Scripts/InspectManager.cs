using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class InspectManager : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        ItemInspect();
    }

    private void ItemInspect()
    {
        Camera.main.fieldOfView = 20f;
    }
}
