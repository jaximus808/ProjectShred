using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public ClientChatManager clientChatMan;

    public GameObject Cam;
    public GameObject startMenu;
    public GameObject ClassSelection;
    public InputField usernameField;
    public InputField IPField;
    public InputField Port;
    public Text Status;
    public Text ConnectingStatus;
    public GameObject CrossHair; // make this customizable later


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
        ConnectingStatus.gameObject.SetActive(true);
        //Debug.Log("FUKC");

        //Debug.Log("FUKC2");

    }
    public void ClassSelectionShow()
    {
        ClassSelection.gameObject.SetActive(true);
        ConnectingStatus.gameObject.SetActive(false);
    }

    public void ChooseClass(int _id)
    {
        ClientSend.WelcomeReceived(_id);
        Cam.SetActive(false);
        ClassSelection.SetActive(false);
        CrossHair.SetActive(true);
        clientChatMan.ChatArea.SetActive(true);

        clientChatMan.inIngameSettings = false;
    }

    public void Disconnect()
    {
        Debug.Log("Failed Connecting");
        Status.text = $"Status: Server is not responding or doesn't exist";
        startMenu.SetActive(true);
        ConnectingStatus.gameObject.SetActive(false);
        usernameField.interactable = true;
    }

    public void InDisconnect()
    {
        Status.text = $"Status: You Left The Game";
        startMenu.SetActive(true);
        ConnectingStatus.gameObject.SetActive(false);
        usernameField.interactable = true;
        clientChatMan.InGameSettings.SetActive(false);
        Cam.SetActive(true);
        CrossHair.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}