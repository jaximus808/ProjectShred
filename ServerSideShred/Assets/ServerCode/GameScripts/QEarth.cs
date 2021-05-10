using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QEarth : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public bool pending = false;

    public Transform spawnPoint;
    public Transform head;
    public Rigidbody rb;
    // Update is called once per frame
    private void FixedUpdate()
    {
        ServerSend.updateEarthQ(id, transform.position, transform.rotation);
        if (pending)
        {
            if (head == null || head == null) return;
            transform.position = spawnPoint.position;
            transform.rotation = head.rotation;
        }
        //ServerSend.updateEarthNorm(id, transform.position, transform.rotation);
    }
}
