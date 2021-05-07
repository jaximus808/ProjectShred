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

    

    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject NormalAttackGameOb;
    [SerializeField] private Transform NormalAttackSpawn;

    private float yVelocity = 0;
    private bool[] inputs;
    private float velocity;
    private Player player;
    private bool canNormalAtk;
    private bool canQ;
    private bool inQProc = false;
    
    private float timerNorm = 0f;
    private float timerQ = 0f;
    private float timerQAdd = 0f;

    private int QTotalAdd = 0;

    private void Start()
    {
        timerNorm = setTimerNorm;
        timerQ = setTimerQ;
        canNormalAtk = true;
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize( Player _player)
    {
        //id = _id;
        //username = _username;
        player = _player;
        inputs = new bool[7];
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
        if(canNormalAtk && inputs[5])
        {
            canNormalAtk = false;
            Debug.Log("Fire!");
            ShootNormalAttack();
            //shoot soemthing
        }
        else if(!canNormalAtk)
        {
            timerNorm -= Time.fixedDeltaTime;
            if(timerNorm <= 0)
            {
                timerNorm = setTimerNorm;
                canNormalAtk = true;
            }
        }
        if (canQ && inputs[6])
        {
            //canQ = false;

            //ShootNormalAttack();
            //shoot soemthing
            inQProc = true;
            if (timerQAdd <= 0)
            {
                QTotalAdd++;
                //send to client of it adding or smth idk
                Debug.Log("QADDED");
                timerQAdd = setTimerQAdd;
            }
            else
            {
                timerQAdd--;
            }
            
        }
        else if(canQ && inQProc)
        {
            canQ = false;
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
    }

    private void ShootNormalAttack()
    {
        NormalEarthAttack currentEarthAttack= Instantiate(NormalAttackGameOb, NormalAttackSpawn.position, transform.rotation).GetComponent<NormalEarthAttack>();
        currentEarthAttack.GetComponent<Rigidbody>().AddForce(currentEarthAttack.transform.forward * 50,ForceMode.Impulse);
        int[] _keyArrayEarth = NetworkManager.NormalEarthAttacks.Keys.ToArray();
        if (_keyArrayEarth.Length == 0)
        {
            currentEarthAttack.id = 0;
            NetworkManager.NormalEarthAttacks.Add(0, currentEarthAttack);
            ServerSend.ShootEarthNorm(0, NormalAttackSpawn.position, transform.rotation, false, 0);
            return;
        }
        int _atkId = _keyArrayEarth[_keyArrayEarth.Length - 1] + 1;
        currentEarthAttack.id = _atkId;
        NetworkManager.NormalEarthAttacks.Add(_atkId, currentEarthAttack);
        Debug.Log($"Bruh: {NetworkManager.NormalEarthAttacks[_atkId].id}");
        ServerSend.ShootEarthNorm(_atkId, NormalAttackSpawn.position, transform.rotation, false, 0);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector3 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        //transform.position += _moveDirection * moveSpeed;
        //if()
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
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
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }
}
