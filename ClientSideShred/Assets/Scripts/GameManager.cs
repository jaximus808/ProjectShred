using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, GameObject> earthNormalAttacks = new Dictionary<int, GameObject>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public GameObject localEarthPlayerPrefab;
    public GameObject earthPlayerPrefab;

    public GameObject earthNormalAttackPrefab;

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

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation, int _classId)
    {
        PlayerManager _player;
        if (_id == Client.instance.myId)
        {
            switch(_classId)
            {
                case 0:
                    _player = Instantiate(localEarthPlayerPrefab, _position, _rotation).GetComponent<PlayerManager>();
                    break;
                default:
                    _player = Instantiate(localPlayerPrefab, _position, _rotation).GetComponent<PlayerManager>();
                    break;
            }
            //_player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            switch(_classId)
            {
                case 0:
                    _player = Instantiate(earthPlayerPrefab, _position, _rotation).GetComponent<PlayerManager>();
                    break;
                default:
                    _player = Instantiate(playerPrefab, _position, _rotation).GetComponent<PlayerManager>();
                    break;
            }
            //_player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.id = _id;
        _player.username = _username;
        players.Add(_id, _player);
    }
    public void createEarthNormAttack(int _id, Vector3 _position, Quaternion _rotation)
    {
        GameObject newEarthAttack = Instantiate(earthNormalAttackPrefab, _position, _rotation);
        earthNormalAttacks.Add(_id, newEarthAttack);
    }
}