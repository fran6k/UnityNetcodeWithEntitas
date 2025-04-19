using System;
using Entitas;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkGameController : NetworkBehaviour
{
    private Systems _systems;

    private bool _start;

    public void Start()
    {
        NetworkManager.Singleton.OnServerStarted += NetworkStart;
        NetworkManager.Singleton.OnClientStarted += NetworkStart;
    }

    public void NetworkStart()
    {
        if (IsServer)
        {
            var prefebs = Resources.Load<GameObject>("NetPlayer");
            var gameObject = GameObject.Instantiate(prefebs);
            gameObject.GetComponent<NetworkObject>().Spawn();
        }

        var contexts = Contexts.sharedInstance;
        _systems = new Feature("test")
            .Add(new NetworkTransformTestSystem(contexts))
            .Add(new InputSystem(contexts));

        _systems.Initialize();
        _start = true;
    }

    public void Update()
    {
        if (!_start) return;
        _systems.Execute();
        _systems.Cleanup();
    }

    public override void OnDestroy()
    {
        if (!_start) return;
        _systems.TearDown();
    }
}