using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Vehicle : MonoBehaviour
{
    
    public enum BlockType{ S1=0, S2=0, S3=0}
    private static readonly float[] BlockLengths = {1.2f, 1.2f, 0};
    private static readonly float[] BlockWidths = {.65f, .65f, 0};
    private static readonly float[] BlockHeights = {0.55f, .55f, 0};
    private static readonly float[] PortOffsetsX = {0.6f, 0.6f, 0};
    private const float PortOffsetY = 0.18f;

    public static PlayerAvatar Avatar;
    public static Road Road;
    public static float LengthStatic(int size=1, BlockType bt=BlockType.S1)
    {
        return BlockLengths[(int)bt] * size;
    }
    
    private float BlockLength()
    {
        return BlockLengths[(int)_blockType];
    }
    private float BlockWidth()
    {
        return BlockWidths[(int)_blockType];
    }
    private float BlockHeight()
    {
        return BlockHeights[(int)_blockType];
    }
    private float PortOffsetX()
    {
        return PortOffsetsX[(int)_blockType];
    }

    public float Length()
    {
        return BlockLength() * Size;
    }
    public float FrontPosZ()
    {
        return transform.position.z + Length();
    }
    public float BackPosZ()
    {
        return transform.position.z;
    }
    public float CenterZ()
    {
        return (FrontPosZ() + BackPosZ()) / 2;
    }

#if UNITY_EDITOR
    public string BehaviourTag;
#endif
    private BlockType _blockType;
    public int Size { get; private set; }
    private bool _isBusy;
    private bool _isEated;
    private Rigidbody _rbody;
    private VehicleBehaviour _behaviour;
    private Action _onAdopt;

    public Vector3 Velocity => _behaviour.GetVelocity();
    public Action OnAdopt
    {
        set { _onAdopt += value; }
    }
    public VehicleBehaviour Behaviour => _behaviour;

    private void Start()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    //todo
    public void InitSize(BlockType blockType=BlockType.S1, int size=1)
    {
        Size = size;
        _blockType = blockType;
    }

    public void InitBehaviour(VehicleBehaviour behaviour)
    {
        #if UNITY_EDITOR
        BehaviourTag = behaviour.Tag;
        #endif
        _behaviour?.Destruct();
        _behaviour = behaviour;
        _behaviour.SetVehicle(this);
        _behaviour.TurnOn();
    }

    public void Rezet()
    {
        _isBusy = false;
        _isEated = false;
        var rbody = GetComponent<Rigidbody>();
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;
    }
    
    public bool DoesComprisePoint()
    {
        bool result;
        Vector3 pos = transform.InverseTransformPoint(Avatar.transform.position);
        result = 0-.1f <= pos.z
                 && Length()+.1f >= pos.z
                 && BlockWidth()+.1f / 2 >= pos.x
                 && -BlockWidth()-.1f / 2 <= pos.x;
        return result;
    }

    public void Adopt()
    {
        Avatar.transform.parent = transform;
        Avatar.Vehicle = this;
        _isBusy = true;
        _onAdopt?.Invoke();
        _behaviour?.HandleAdopting();
    }

    public Vector3 GetLocalPort()
    {
        Vector3 port;
        port.x = 0;
        port.y = BlockHeight()+PortOffsetY;
        port.z = ComputeBlockNum(Avatar.transform.localPosition) * BlockLength() + PortOffsetX();
        return port;
    }

    private int ComputeBlockNum(Vector3 localPos)
    {
        return Mathf.FloorToInt( localPos.z/BlockLength() );
    }

    //redo todo
    public void BreakeDown()
    {
        _isBusy = false;
        _behaviour?.HandleBreakeDown();
    }

    public bool TryEat()
    {
        return _behaviour != null && _behaviour.HandleEating();
    }

    //todo
    private void Сrash()
    {
        
    }
    //todo
    private void DropAvatar(Vector3 velocity)
    {
        Avatar.Die();
        var rb = Avatar.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1;
        rb.velocity = velocity;
    }
  
    public void Destruct()
    {
        _behaviour?.Destruct();
    }

    private Vector3 _savedVel;
    private void FixedUpdate()
    {
        if (_avaColl)
        {
            _savedVel = _rbody.velocity;
            _rbody.constraints = RigidbodyConstraints.FreezeAll;
            _delayFlag = true;
            _avaColl = false;
            transform.position = transform.position + _savedVel * Time.fixedDeltaTime;
        } else if (_delayFlag)
        {
            _rbody.constraints = RigidbodyConstraints.None;
            _rbody.velocity = _savedVel;
            _delayFlag = false;
        }
        _behaviour?.Affect();
    }

    //todo
    private bool _avaColl;
    private bool _delayFlag;
    
    private void OnCollisionEnter(Collision other)
    {                
        switch (other.gameObject.tag)
        {
            case "Avatar":
                _avaColl = true;
                break;
            case "Shell":
                break;
            case "Meteor":
                _behaviour?.Stop();
                break;
            case "Car":
                break;
            case "Road":
                break;
            default:
                throw new ArgumentException("Wrong tag: "+other.gameObject.tag);
        }
        _behaviour?.HandleCollisionEnter(other);
    }

    private void OnCollisionExit(Collision other)
    {
        var rbody = GetComponent<Rigidbody>();
        switch (other.gameObject.tag)
        {
            case "Avatar":
            case "Car":
                //todo undo
                //rbody.angularVelocity = Vector3.zero;
                break;
            case "Shell":
                break;
            case "Meteor":
                break;
            case "Tank":
            case "Pointer":
            case "Oil":
                break;
            case "Road":
                break;
            default:
                throw new ArgumentException("Wrong tag: "+other.gameObject.tag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Oil":
                break;
            case "OilEnd":
                break;
            case "Pointer":
                break;
        }
        _behaviour?.HandleTriggerEnter(other);
    }

    private void OnDisable()
    {
    }

    private void OnDestroy()
    {
        _behaviour?.Destruct();
    }
}

