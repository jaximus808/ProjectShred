using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REarth : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public float baseMass;
    public float mass;

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

        
        if (prevPos == transform.position && preRot == transform.rotation) return;
        
        ServerSend.UpdateProjectile(3, id, transform.position, transform.rotation);
        prevPos = transform.position;
        preRot = transform.rotation;
    }
}

