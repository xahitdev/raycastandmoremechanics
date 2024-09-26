using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float doorRotation; //usually being 90 degree 
    bool doorState = false;
    bool isTriggered = false;
    private float doorSpeed = 2;
    public void Interact()
    {
        DoorMech();
    }
    private void DoorMech()
    {
        if (isTriggered) return;

        isTriggered = true;

        if (!doorState)
        {
            float targetRotY = doorPivot.transform.eulerAngles.y + 90;
            float rotX = doorPivot.transform.eulerAngles.x;
            float rotY = doorPivot.transform.eulerAngles.y;
            float rotZ = doorPivot.transform.eulerAngles.z;
            doorPivot.transform.DORotate(new Vector3(rotX, targetRotY, rotZ), doorSpeed, RotateMode.FastBeyond360).OnComplete(() => { doorState = true; isTriggered = false; });
        }
        else
        {
            float targetRotY = doorPivot.transform.eulerAngles.y - 90;
            float rotX = doorPivot.transform.eulerAngles.x;
            float rotY = doorPivot.transform.eulerAngles.y;
            float rotZ = doorPivot.transform.eulerAngles.z;
            doorPivot.transform.DORotate(new Vector3(rotX, targetRotY, rotZ), doorSpeed, RotateMode.FastBeyond360).OnComplete(() => { doorState = false; isTriggered = false; });
        }

    }
}
