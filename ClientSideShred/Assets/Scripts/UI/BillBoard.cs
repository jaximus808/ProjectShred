using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cam;
    private bool aimTowards = false;

    public void SetCam ()
    {
        cam = GameManager.instance.localPlayer.cam;
        aimTowards = true; 
    }

    void LateUpdate()
    {
        if (!aimTowards) return;
        if (cam == null) return;
        transform.LookAt(cam.position);
    }
}
