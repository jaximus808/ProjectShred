using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cam;
    private bool aimTowards = false;

    public void SetCam ()
    {
        Debug.Log("uh");
        cam = GameManager.instance.localPlayer.cam;
        aimTowards = true; 
    }

    void LateUpdate()
    {
        if (!aimTowards) return; 
        transform.LookAt(cam.position);
    }
}
