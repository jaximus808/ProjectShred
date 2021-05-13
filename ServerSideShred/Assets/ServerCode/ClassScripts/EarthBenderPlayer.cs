using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarthBenderPlayer : MonoBehaviour
{
    public int id;
    public string username;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float setTimerNorm = 0f;
    public float setTimerQ;
    public float setTimerQAdd;
    public int qAddCap;
    public float setTimerC;
    public float raiseRate;
    public Transform groundCheck;
    [SerializeField] private CharacterController controller;

    [SerializeField] private GameObject NormalAttackGameOb;
    [SerializeField] private Transform NormalAttackSpawn;


    [SerializeField] private GameObject QAttackGameOb;
    [SerializeField] private Transform[] QAttackSpawns;

    [SerializeField] private GameObject WallObject;

    [SerializeField] private GameObject headOb;
    [SerializeField] private Transform qAim;

    [SerializeField] private LayerMask PlayerLayer;

    private float yVelocity = 0;
    private bool[] inputs;
    private float velocity;
    private Player player;
    private bool canNormalAtk;
    private bool canQ;
    private bool inQProc = false;
    private bool canC = true;
    private bool raising = false;
    private int[] idQs = { 0, 0, 0, 0, 0 };

    private float timerNorm = 0f;
    private float timerQ = 0f;
    private float timerQAdd = 0f;
    private float timerC = 0f;

    private int currCWall = 0;
    private int QTotalAdd = 0;

    private void Start()
    {
        //idQs = new int[] { 0,0,0,0,0};
        timerC = setTimerC;
        timerNorm = setTimerNorm;
        timerQ = setTimerQ;
        canNormalAtk = true;
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(Player _player)
    {
        //id = _id;
        //username = _username;
        player = _player;
        inputs = new bool[8];
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        Vector3 _inputDirection = Vector3.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            _inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            _inputDirection.x += 1;
        }
        if (inputs[4])
        {
            _inputDirection.z = 1;
        }
        Move(_inputDirection);
        
        if (canQ && inputs[6])
        {
            //canQ = false;

            //ShootNormalAttack();
            //shoot soemthing
            inQProc = true;
            if (timerQAdd <= 0)
            {
                if (QTotalAdd + 1 <= qAddCap)
                {
                    QTotalAdd++;
                    //send to client of it adding or smth idk
                    AddSpikesPend();
                    Debug.Log("QADDED");
                    timerQAdd = setTimerQAdd;
                }

            }
            else
            {
                timerQAdd -= Time.fixedDeltaTime;
            }

        }
        else if (canQ && inQProc)
        {
            canQ = false;
            inQProc = false;
            ShootQSpikes();
            Debug.Log($"Shot {QTotalAdd} spikes");
            QTotalAdd = 0;
        }
        else if (!canQ)
        {
            timerQ -= Time.fixedDeltaTime;
            if (timerQ <= 0)
            {
                timerQ = setTimerQ;
                canQ = true;
            }
        }
        if (canNormalAtk && inputs[5])
        {
            canNormalAtk = false;
            Debug.Log("Fire!");
            ShootNormalAttack();
            //shoot soemthing
        }
        else if (!canNormalAtk)
        {
            timerNorm -= Time.fixedDeltaTime;
            if (timerNorm <= 0)
            {
                timerNorm = setTimerNorm;
                canNormalAtk = true;
            }
        }
        //prob make this logic nicer but should work
        if (inputs[7] && canC && !raising)
        {
            RaycastHit hit;
            //maybe make it where u can hold, or make it like sage wall, i like first idea better
            if(!Physics.Raycast(headOb.transform.position, headOb.transform.forward, out hit, 300, PlayerLayer))
            {
                return;
            }
            
            raising = true;
            Quaternion direction = Quaternion.FromToRotation(hit.transform.up, hit.normal);
            CreateNewWall(hit.point, direction);
            //raising
        }
        else if(inputs[7] && raising)
        {
            //Raise Object 
            NetworkManager.EarthCScale[currCWall].transform.localScale += new Vector3(0f, raiseRate, 0);
            ServerSend.RaiseEarthWall(currCWall, NetworkManager.EarthCScale[currCWall].transform.localScale);
        }
        else if(raising)
        {
            raising = false;
            canC = false;
            currCWall = 0; 
        }
        if(!canC)
        {
            timerC -= Time.fixedDeltaTime; 
            if (timerC <= 0)
            {
                timerC = setTimerC;
                canC = true;
            }
        }
    }

    private void CreateNewWall(Vector3 _position, Quaternion _direction)
    {
        //amke wall object vertices change later some how 
        CEarth curScalingWall = Instantiate(WallObject, _position, _direction).GetComponent<CEarth>();
        int[] _keyArrayEarthC = NetworkManager.EarthCScale.Keys.ToArray();
        if (_keyArrayEarthC.Length == 0)
        {
            curScalingWall.id = 0;
            //create a limit maybe?
            NetworkManager.EarthCScale.Add(0, curScalingWall);
            currCWall = 0;
            ServerSend.CreateProjectile(2, 0, _position, _direction, false, 0);
            return;
        }
        int _atkId = _keyArrayEarthC[_keyArrayEarthC.Length - 1] + 1;
        //idQs[QTotalAdd - 1] = _atkId;
        curScalingWall.id = _atkId;
        NetworkManager.EarthCScale.Add(_atkId, curScalingWall);
        currCWall = _atkId;
        Debug.Log($"NewC: {NetworkManager.EarthCScale[_atkId].id}");
        ServerSend.CreateProjectile(2, _atkId, _position, _direction, false, 0);


    }

    private void ShootQSpikes()
    {
        Vector3 hitPoint = new Vector3(0,0,0); 
        RaycastHit hit;
        bool hitTrue = false;
        if(Physics.Raycast(headOb.transform.position, headOb.transform.forward, out hit, 1000))
        {
            hitTrue = true;
            hitPoint = hit.point; 
        }
        
        for(int i = 0; i < QTotalAdd; i++)
        {
            if(hitTrue)
            {
                NetworkManager.InActionEarthQ[idQs[i]].transform.LookAt(hitPoint);
                Debug.Log(":)");
            }
            
            NetworkManager.InActionEarthQ[idQs[i]].pending = false;
            NetworkManager.InActionEarthQ[idQs[i]].rb.isKinematic = false;
            NetworkManager.InActionEarthQ[idQs[i]].rb.AddForce(QAttackSpawns[i].forward * 1000, ForceMode.Impulse);
        }
        idQs = new int[]{ 0, 0, 0, 0, 0 };
    }

    private void AddSpikesPend()
    {
        Transform spawnPos = QAttackSpawns[QTotalAdd - 1];
        QEarth currQEarthAtk = Instantiate(QAttackGameOb, spawnPos.transform.position, headOb.transform.rotation).GetComponent<QEarth>();
        currQEarthAtk.head = headOb.transform;
        currQEarthAtk.spawnPoint = spawnPos;
        currQEarthAtk.pending = true;
        int[] _keyArrayEarthQ = NetworkManager.InActionEarthQ.Keys.ToArray();
        if (_keyArrayEarthQ.Length == 0)
        {
            currQEarthAtk.id = 0;
            idQs[QTotalAdd - 1] = 0;
            NetworkManager.InActionEarthQ.Add(0, currQEarthAtk);
            ServerSend.CreateProjectile(1,0, spawnPos.position, currQEarthAtk.transform.rotation, false, 0);
            return;
        }
        int _atkId = _keyArrayEarthQ[_keyArrayEarthQ.Length - 1] + 1;
        idQs[QTotalAdd - 1] = _atkId;
        currQEarthAtk.id = _atkId;
        NetworkManager.InActionEarthQ.Add(_atkId, currQEarthAtk);
        Debug.Log($"NewQ: {NetworkManager.InActionEarthQ[_atkId].id}");
        ServerSend.CreateProjectile(1,_atkId, spawnPos.position, currQEarthAtk.transform.rotation, false, 0);
    }

    private void ShootNormalAttack()
    {
        NormalEarthAttack currentEarthAttack= Instantiate(NormalAttackGameOb, NormalAttackSpawn.position, headOb.transform.rotation).GetComponent<NormalEarthAttack>();
        currentEarthAttack.GetComponent<Rigidbody>().AddForce(currentEarthAttack.transform.forward * 1000,ForceMode.Impulse);
        int[] _keyArrayEarth = NetworkManager.NormalEarthAttacks.Keys.ToArray();
        if (_keyArrayEarth.Length == 0)
        {
            currentEarthAttack.id = 0;
            NetworkManager.NormalEarthAttacks.Add(0, currentEarthAttack);
            ServerSend.CreateProjectile(0,0, NormalAttackSpawn.position, currentEarthAttack.transform.rotation, false, 0);
            return;
        }
        int _atkId = _keyArrayEarth[_keyArrayEarth.Length - 1] + 1;
        currentEarthAttack.id = _atkId;
        NetworkManager.NormalEarthAttacks.Add(_atkId, currentEarthAttack);
        Debug.Log($"Bruh: {NetworkManager.NormalEarthAttacks[_atkId].id}");
        ServerSend.CreateProjectile(0,_atkId, NormalAttackSpawn.position, currentEarthAttack.transform.rotation, false, 0);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector3 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        //transform.position += _moveDirection * moveSpeed;
        //if()
        _moveDirection *= moveSpeed;
        
        if (Physics.CheckSphere(groundCheck.position, 1f, PlayerLayer))
        {
            yVelocity = 0f;
            if (_inputDirection.z == 1)
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;
        _moveDirection.y = yVelocity;

        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(player.id, transform.position);
        ServerSend.PlayerRotation(player.id, transform.rotation);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation, Quaternion _headRotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
        headOb.transform.rotation = _headRotation;
    }

    public void Disconnect()
    {
        for(int i = 0; i < QTotalAdd; i++)
        {

            NetworkManager.InActionEarthQ[idQs[i]].pending = false;
            NetworkManager.InActionEarthQ[idQs[i]].rb.isKinematic = false;
        }
    }
}