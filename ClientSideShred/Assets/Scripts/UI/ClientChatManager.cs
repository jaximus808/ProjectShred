using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ClientChatManager : MonoBehaviour
{
    public InputField chatInput;

    public GameObject ChatArea;
    public GameObject InGameSettings;

    public Button DisconnectButton;

    private int state = 1;
    public bool focused = false;

    private bool canT = true;
    private float setChatTimer = 0.5f;
    private float curChatTimer = 0;

    private bool canRet = true;
    private float setRetTimer = 0.5f;
    private float curRetTimer = 0;

    private bool canEsc = true;
    private float setEscTimer = 0.5f;
    private float curEscTimer = 0f;

    public bool inIngameSettings = false; 
    

    private void Update()
    {
        Timers();
        if (Client.instance.isConnected && state == 0)
        {
            state = 1;

        }
        if (!Client.instance.isConnected && state == 1)
        {
            chatInput.interactable = false;
            state = 0;
        }
        if (Input.GetKey(KeyCode.T) && Client.instance.isConnected && canT && !focused)
        {            
            chatInput.interactable = true;
            chatInput.ActivateInputField();
            focused = true;   
            canT = false;
            return; 
        }
        else if (Input.GetKey(KeyCode.Escape) && Client.instance.isConnected && focused)
        {
            chatInput.DeactivateInputField();
        }
        if (focused)
        {
            chatInput.interactable = true;
        }
        else
        {
            chatInput.interactable = false ;
            focused = false;
        }
        //if (!chatInput.isFocused || state == 0)
        //{
        //    Debug.Log("??");
           
        //    return;
        //}
        if(Input.GetKey(KeyCode.Return)&&canRet)
        {
            if(chatInput.text != "") ClientSend.SendChatMessage(chatInput.text);
            canRet = false;
            chatInput.text = "";
            focused = false;
            chatInput.interactable = false;
        }
        if(Input.GetKey(KeyCode.Escape) && canEsc && focused)
        {
            canEsc = false;
            chatInput.text = "";
            focused = false;
            chatInput.interactable = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKey(KeyCode.Escape) && canEsc && !inIngameSettings && !focused && Client.instance.isConnected)
        {
            //ingame menu; 
            canEsc = false; 
            ChatArea.SetActive(false);
            InGameSettings.SetActive(true);
            inIngameSettings = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (Input.GetKey(KeyCode.Escape) && canEsc && !focused)
        {
            //ingame menu; 
            canEsc = false;
            ChatArea.SetActive(true);
            InGameSettings.SetActive(false);
            inIngameSettings = false;
        }
    }
    private void Timers()
    {
        if(!canT)
        {
            curChatTimer += Time.deltaTime;
            if (curChatTimer > setChatTimer)
            {
                canT = true;
                curChatTimer = 0;
            }
        }
        if (!canRet)
        {
            curRetTimer += Time.deltaTime;
            if (curRetTimer > setRetTimer)
            {
                canRet = true;
                curRetTimer = 0;
            }
        }
        if (!canEsc)
        {
            
            curEscTimer += Time.deltaTime;
            if (curEscTimer > setEscTimer)
            {
                canEsc = true;
               
                curEscTimer = 0;
            }
        }
    }
}
