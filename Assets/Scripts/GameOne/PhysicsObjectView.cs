using System;
using Entitas;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsObjectView : UnityNetworkView, INetworkPosition, ILocalPosition
{
    private Rigidbody _rigidbody;

    [SerializeField] public NetworkVariable<Vector3> _serverPosition = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        Link(Contexts.sharedInstance);
        // _rigidbody.AddForce(new Vector3(5,0,0),ForceMode.Impulse);
    }

    Vector3 INetworkPosition.Get()
    {
        // 如果是服务器环境下，就返回本地的数据信息
        // if the code is running in the server, then return the server's rigidbody position.
        if (IsServer)
        {
            return _rigidbody.transform.position;
        }

        // 否则就返回服务器传来的数据
        // else return the networkVariable's value. let client get the remote positon
        if (IsClient)
        {
            return _serverPosition.Value;
        }

        else return Vector3.zero;
    }
    // ? 用FixedUpdate来同步刚体的位置。如果用独立的物理模块而不是RigidBody，就不需要这样做了。
    // TODO: idk if i should do this. to sync the rigidbody's position.


    void INetworkPosition.Set(Vector3 pos)
    {
        // 直接设置坐标
        // we set the position directly.
        _rigidbody.position = pos;

        // 如果是服务器，则设置NetworkVariable来同步信息
        // if running in the server, we need to sync data to client.
        if (IsServer)
        {
            _serverPosition.Value = pos;
        }
    }

    Vector3 ILocalPosition.Get()
    {
        return _rigidbody.transform.position;
    }

    public void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > 0.1f && IsServer)
        {
            _serverPosition.Value = _rigidbody.position;
        }
    }

    public void Link(Contexts contexts)
    {
        var e = contexts.game.CreateEntity();

        e.AddView(this);

        base.Link(contexts, e);

        e.AddNetworkPosition(this);
        e.AddWASDInput(Vector2.zero);
        e.AddSpeed(10.0f);

        if (IsClient)
        {
            e.isClient = true;
            e.AddLocalPosition(this);
        }

        // e.AddPositionListener(this);
        _rigidbody = GetComponent<Rigidbody>();
    }
}