using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REarth : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public float baseMass;
    public float mass;

    public float initForce; 

    public int damage;

    public Player casterPlayer;
    
    private GameObject hitOb;
    private Player hitPlayer;
    private bool firstStart = true; 

    private Vector3 prevPos;
    private Quaternion preRot;

    public float initLife;

    private float curInitLife = 0f;

    public Collider initUltCollider;
    public Collider initUltCollider1;

    public float projectileLifeSpan;
    private float curLifeSpan;
    // Update is called once per frame
    private void Start()
    {
        prevPos = transform.position;
        preRot = transform.rotation;

    }
    private void FixedUpdate()
    {
        //if(firstStart&&rSpawnHit)
        //{
        //    hitPlayer.ApplyDamage(damage*2, casterPlayer.username);
        //}
        if(curInitLife>=initLife && firstStart)
        {
            initUltCollider.enabled = false;
            initUltCollider1.enabled = false; 
            firstStart = false;
        }
        else
        {
            curInitLife += Time.fixedDeltaTime; 
        }

        if (curLifeSpan >= projectileLifeSpan)
        {
            NetworkManager.UltEarth.Remove(id);
            ServerSend.DeleteObject(3, id);
            Destroy(gameObject);
            return;
        }
        curLifeSpan += Time.fixedDeltaTime;

        if (prevPos == transform.position && preRot == transform.rotation) return;
        
        ServerSend.UpdateProjectile(3, id, transform.position, transform.rotation);
        prevPos = transform.position;
        preRot = transform.rotation;
        
    }
    //get spawn hit detection working lol i forgot.
    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.transform.gameObject.layer == 8 && _collision.transform.gameObject != casterPlayer.gameObject)
        {
            Debug.Log(_collision.transform.gameObject.name);
            if (_collision.transform.gameObject == hitOb)
            {
                if (rb.velocity.magnitude < 15f) return;
                hitPlayer.ApplyDamage(damage, casterPlayer.username);
            }
            else
            {
                if (rb.velocity.magnitude < 15f) return;
                hitPlayer = _collision.transform.GetComponent<Player>();
                hitOb = _collision.transform.gameObject;
                
            }
        }

    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.transform.gameObject.layer == 8 && _collider.transform.gameObject != casterPlayer.gameObject)
        {
            if (_collider.transform.gameObject == hitOb)
            {
                bool _alive = hitPlayer.ApplyDamage(damage, casterPlayer.username);
                if (_alive)
                {
                    hitPlayer.ApplyPlayerForce(new Vector3(hitPlayer.transform.position.x - transform.position.x, hitPlayer.transform.position.y - transform.position.y, hitPlayer.transform.position.z - transform.position.z), initForce);
                }
            }
            else
            {
                hitPlayer = _collider.transform.GetComponent<Player>();
                hitOb = _collider.transform.gameObject;
                hitPlayer.ApplyDamage(damage*2, casterPlayer.username);
                bool _alive = hitPlayer.ApplyDamage(damage, casterPlayer.username);
                if (_alive)
                {
                    Debug.Log("1");
                    hitPlayer.ApplyPlayerForce(new Vector3(hitPlayer.transform.position.x - transform.position.x, hitPlayer.transform.position.y - transform.position.y, hitPlayer.transform.position.z - transform.position.z), initForce);
                }
                

            }
        }
    }
}

