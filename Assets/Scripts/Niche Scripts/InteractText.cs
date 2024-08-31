using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    public void Interact(){
        Text();
    }
    void Text(){
        
    }
}
