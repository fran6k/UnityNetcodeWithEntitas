using Entitas;
using Unity.Netcode;


public class NetworkGameController : NetworkBehaviour
{
    private Systems _systems;

    private bool _start;

    public void Start()
    {
        NetworkManager.Singleton.OnClientStarted += NetworkStart;
    }

    public void NetworkStart()
    {
        var contexts = Contexts.sharedInstance;
        
        _systems = new Feature("Move")
            .Add(new MoveSystem(contexts));
        
        _systems.Initialize();
        _start = true;
    }

    public void Update()
    {
        if (!_start) return;
        
        _systems.Execute();
        _systems.Cleanup();
    }

    public void OnDestroy()
    {
        if (!_start) return;
        
        _systems.TearDown();
    }
}