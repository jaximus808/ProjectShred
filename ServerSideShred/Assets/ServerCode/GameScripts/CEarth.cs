using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEarth : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public float baseMass;
    public GameObject headParent;

    public Player casterPlayer; 

    public int damage;
    public bool child = false; 

    private Player hitPlayer;
    private GameObject hitOb;

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
        if (!child)
        {
            rb.mass = transform.localScale.y + baseMass;
        } 
        
        if (prevPos == transform.position && preRot == transform.rotation) return;
        ServerSend.UpdateProjectile(2, id, transform.position, transform.rotation);
        prevPos = transform.position;
        preRot = transform.rotation;
    }
    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.gameObject.layer == 8 )
        {
            Debug.Log(_collision.transform.gameObject.name);
            if (_collision.transform.gameObject == hitOb)
            {
                if (rb.velocity.magnitude < 15f) return;
                hitPlayer.ApplyDamage(damage + (int)rb.velocity.magnitude, casterPlayer.name);
            }
            else
            {
                if (rb.velocity.magnitude < 15f) return;
                hitPlayer = _collision.transform.GetComponent<Player>();
                hitOb = _collision.transform.gameObject;
                hitPlayer.ApplyDamage(damage + (int)rb.velocity.magnitude, casterPlayer.name);

            }
        }

    }
}
