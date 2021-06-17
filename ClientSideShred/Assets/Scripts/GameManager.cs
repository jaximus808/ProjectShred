using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ClientChatManager ChatMang;

    public int projectiles;
    
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public static Dictionary<int, Dictionary<int, GameObject>> Projectiles = new Dictionary<int, Dictionary<int, GameObject>>();

    public PlayerManager localPlayer; 

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public GameObject localEarthPlayerPrefab;
    public GameObject earthPlayerPrefab;

    //0 = EarthnormalAttack; 1 = EarthQPrefab
    public GameObject[] ProjectilesPrefab; 
    //public GameObject earthNormalAttackPrefab;
    //public GameObject earthQAttackPrefab;

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
        for(int i = 0; i < projectiles; i++)
        {
            Projectiles.Add(i, new Dictionary<int, GameObject>());
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation, int _classId, int _hp)
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
            localPlayer = _player;
            foreach(KeyValuePair<int, PlayerManager> player in players)
            {
                if(player.Value.id != _id)
                {
                    player.Value.bill.SetCam();
                }
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
        _player.SetMaxHeath(_hp);
        players.Add(_id, _player);
    }
    

    public void CreateProjectile(int _projectileId, int _id, Vector3 _position, Quaternion _rotation)
    {
        GameObject newProjectile = Instantiate(ProjectilesPrefab[_projectileId], _position, _rotation);
        Projectiles[_projectileId].Add(_id, newProjectile);
    }
}