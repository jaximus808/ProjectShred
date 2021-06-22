using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QEarth : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public bool pending = false;
    public int damage;

    public Player casterPlayer; 

    public Transform spawnPoint;
    public Transform head;
    public Rigidbody rb;


    private Player hitPlayer;
    private GameObject hitOb;



    // Update is called once per frame
    private void FixedUpdate()
    {

        if (pending)
        {
            if (head == null || head == null) return;
            transform.position = spawnPoint.position;
            transform.rotation = head.rotation;
        }
        //make a check
        ServerSend.UpdateProjectile(1, id, transform.position, transform.rotation);

        //ServerSend.updateEarthNorm(id, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Debug.Log(_collision.transform.gameObject.layer);
        if (_collision.transform.gameObject.layer == 8 && _collision.transform.gameObject != casterPlayer.gameObject)
        {
            Debug.Log(_collision.transform.gameObject.name);
            if (_collision.transform.gameObject == hitOb)
            {
                if (rb.velocity.magnitude < 20f) return; 
                hitPlayer.ApplyDamage(damage, casterPlayer.username);
            }
            else
            {
                if (rb.velocity.magnitude < 20f) return;
                hitPlayer = _collision.transform.GetComponent<Player>();
                hitOb = _collision.transform.gameObject;
                hitPlayer.ApplyDamage(damage, casterPlayer.username);

            }
        }

    }
    private void OnTriggerEnter(Collider _collider)
    {
        //make a child of the earth C
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
