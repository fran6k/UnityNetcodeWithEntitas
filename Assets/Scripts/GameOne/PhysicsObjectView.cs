using UnityEngine;

public class PhysicsObjectView : UnityNetworkView, ILocalPosition, IMoveInput
{
    private Rigidbody _rigidbody;
    private PlayerInput _PlayerInput;

    //[SerializeField] public NetworkVariable<Vector3> _serverPosition = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if(!base.IsOwner) return;
        
        _rigidbody = GetComponent<Rigidbody>();
        _PlayerInput = GetComponent<PlayerInput>();
        
        Link(Contexts.sharedInstance);
    }

    Vector3 ILocalPosition.Get()
    {
        return _rigidbody.transform.position;
    }

    void ILocalPosition.Set(Vector3 position)
    {
        _rigidbody.transform.position = position;
    }

    Vector3 IMoveInput.Get()
    {
        return new Vector3(_PlayerInput.moveVector.x, 0, _PlayerInput.moveVector.y).normalized;
    }

    public void Link(Contexts contexts)
    {
        var e = contexts.game.CreateEntity();
        base.Link(contexts, e);
        
        e.AddView(this);
        e.AddLocalPosition(this);
        e.AddMoveInput(this);
        e.AddMove(3f,_rigidbody);
    }
}