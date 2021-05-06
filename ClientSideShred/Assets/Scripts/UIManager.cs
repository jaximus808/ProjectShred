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
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        
        Client.instance.ConnectToServer();
        
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