using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject Cam;
    public GameObject startMenu;
    public GameObject ClassSelection;
    public InputField usernameField;
    public InputField IPField;
    public InputField Port;
    public Text Status; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        IPField.text = "127.0.0.1";
        Port.text = "26950";
    }

    /// <summary>Attempts to connect to the server.</summary>
    /// //create a save of servers through initalization
    public void ConnectToServer()
    {
        string tryConnect = Client.instance.ConnectToServer(IPField.text, Port.text);

        if (tryConnect != "Good")
        {
            Status.text = $"Status: {tryConnect}";
            return;
        }
        startMenu.SetActive(false);
        usernameField.interactable = false;
        
        //Debug.Log("FUKC");
        
        //Debug.Log("FUKC2");

    }
    public void ClassSelectionShow()
    {
        ClassSelection.SetActive(true);
    }

    public void ChooseClass(int _id)
    {
        ClientSend.WelcomeReceived(_id);
        Cam.SetActive(false);
        ClassSelection.SetActive(false);
        
    }


}