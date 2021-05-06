using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEarthAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public int id; 

    // Update is called once per frame
    private void FixedUpdate()
    {
        ServerSend.updateEarthNorm(id, transform.position, transform.rotation);
    }
}
