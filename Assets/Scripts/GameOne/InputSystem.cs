using Entitas;
using Unity.Netcode;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class InputSystem : IExecuteSystem
{
    private Contexts _contexts;
    private PhysicsObjectView _physicsObjectView;
    private Vector2 _value;

    public InputSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var group = _contexts.game.GetGroup(GameMatcher.WASDInput);
        _value = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            _value.x = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _value.x = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _value.y = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _value.y = -1;
        }
        foreach (var e in group)
        {
            e.ReplaceWASDInput(_value);
        }
    }
}