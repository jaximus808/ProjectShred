using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEarth : MonoBehaviour
{
    public int id;
    public Rigidbody rb;

    private Vector3 prevPos; 
    private Quaternion preRot; 

    // Update is called once per frame
    private void Start()
    {
        prevPos = transform.position;
        preRot = transform.rotation; 
    }
    private void FixedUpdate()
    {
        rb.mass = transform.localScale.y;
        if (prevPos == transform.position && preRot == transform.rotation) return;
        ServerSend.UpdateProjectile(2, id, transform.position, transform.rotation);
        prevPos = transform.position;
        preRot = transform.rotation; 
    }
}
