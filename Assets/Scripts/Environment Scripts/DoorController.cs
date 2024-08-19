using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using DG.Tweening;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private float doorRotation; //usually being 90 degree 
    bool doorState = false;
    private float doorSpeed = 2;
    public void Interact()
    {
        Door();
    }
    private void Door()
    {
        if (!doorState)
        {
            DoorOpen();
        }
        else if (doorState)
        {
            DoorDefault();
        }
    }
    private void DoorOpen()
    {
        float rotX = doorPivot.transform.eulerAngles.x;
        float rotY = doorPivot.transform.eulerAngles.y;
        float rotZ = doorPivot.transform.eulerAngles.z;
        doorPivot.transform.DORotate(new Vector3(rotX, rotY + 90, rotZ), doorSpeed, RotateMode.FastBeyond360);
        doorState = true;
    }
    private void DoorDefault()
    {
        float rotX = doorPivot.transform.eulerAngles.x;
        float rotY = doorPivot.transform.eulerAngles.y;
        float rotZ = doorPivot.transform.eulerAngles.z;
        doorPivot.transform.DORotate(new Vector3(rotX, rotY - 90, rotZ), doorSpeed, RotateMode.FastBeyond360);
        doorState = false;
    }
}
