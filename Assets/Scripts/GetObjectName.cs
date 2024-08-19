using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetObjectName : MonoBehaviour
{
    public TextMeshProUGUI tagDisplayText;
    private string hitTag;
    void Update()
    {
        GetName();
    }
    private void GetName()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitTag = hit.collider.tag;
            if (hitTag == "Untagged")
                tagDisplayText.text = "";
            else
            {
                tagDisplayText.text = hitTag;
            }
        }
        else
        {
            tagDisplayText.text = "";
        }
    }
}
