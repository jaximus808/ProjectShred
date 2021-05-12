using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public static Dictionary<int, NormalEarthAttack> NormalEarthAttacks = new Dictionary<int, NormalEarthAttack>();
    public static Dictionary<int, QEarth> InActionEarthQ = new Dictionary<int, QEarth>();
    public static Dictionary<int, CEarth> EarthCScale = new Dictionary<int, CEarth>();

    //public static Dictionary<int, PositionObject> PeningEathQ = new Dictionary<int, PositionObject>();

    public GameObject playerPrefab;
    public GameObject playerEarthPrefab;

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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    //public Player InstantiatePlayer()
    //{
    //    return Instantiate(playerPrefab, new Vector3(0f, 2f, 0f), Quaternion.identity).GetComponent<Player>();
    //}

    public Player InstantiateEarthPlayer()
    {
        return Instantiate(playerEarthPrefab, new Vector3(0f, 2f, 0f), Quaternion.identity).GetComponent<Player>();
    }
}