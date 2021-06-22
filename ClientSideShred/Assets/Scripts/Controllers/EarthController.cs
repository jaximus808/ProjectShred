using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{
    private ClientChatManager  ChatMag = GameManager.instance.ChatMang;

    public Transform cameraRot; 
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        if (ChatMag.focused) return; 
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            Input.GetKey(KeyCode.Mouse0),
            Input.GetKey(KeyCode.Q),
            Input.GetKey(KeyCode.C),
            Input.GetKey(KeyCode.E),
            Input.GetKey(KeyCode.Z),
            Input.GetKey(KeyCode.X),
            Input.GetKey(KeyCode.R),
            Input.GetKey(KeyCode.M),
            Input.GetKey(KeyCode.LeftShift),
            Input.GetKey(KeyCode.LeftControl)
        };

        ClientSend.PlayerMovement(_inputs, cameraRot.rotation);
    }
}
