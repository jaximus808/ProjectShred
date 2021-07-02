using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REarth : MonoBehaviour
{
    public int id;
    public Rigidbody rb;
    public float baseMass;
    public float mass;

    public int damage;

    public Player casterPlayer;
    
    private GameObject hitOb;
    private Player hitPlayer;
    private bool rSpawnHit = false;
    private bool firstStart = true; 

    private Vector3 prevPos;
    private Quaternion preRot;

    public float initLife;

    private float curInitLife = 0f;

    public Collider initUltCollider;

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
            rSpawnHit = true;
            Debug.Log("L3");
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
                hitPlayer.ApplyDamage(damage, casterPlayer.username);

            }
        }

    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.transform.gameObject.layer == 8 && _collider.transform.gameObject != casterPlayer.gameObject)
        {
            if (_collider.transform.gameObject == hitOb)
            {
                hitPlayer.ApplyDamage(damage*2, casterPlayer.username);
            }
            else
            {
                hitPlayer = _collider.transform.GetComponent<Player>();
                hitOb = _collider.transform.gameObject;
                hitPlayer.ApplyDamage(damage*2, casterPlayer.username);

            }
        }
    }
}

