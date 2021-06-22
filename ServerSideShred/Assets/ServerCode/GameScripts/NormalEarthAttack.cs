using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEarthAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public int damage;

    public Rigidbody rb;

    public Player casterPlayer; 
    public GameObject parentPlayer;

    private Player hitPlayer;
    private GameObject hitOb;

    // Update is called once per frame
    private void FixedUpdate()
    {
        ServerSend.UpdateProjectile(0,id, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if(_collision.transform.gameObject.layer == 8 && _collision.transform.gameObject != casterPlayer.gameObject)
        {
            Debug.Log(_collision.transform.gameObject.name);
            if(_collision.transform.gameObject == hitOb)
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
}
