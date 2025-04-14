using Entitas;
using UnityEngine;
using Unity.Netcode;

public class MoveSystem : IExecuteSystem
{
    private Contexts _contexts;
    
    public MoveSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var group = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Move,GameMatcher.LocalPosition));
        foreach (var e in group)
        {
            //way 1
            //e.localPosition.Value.Set(e.localPosition.Value.Get() + e.moveInput.value.Get()*0.1f);
            
            //way 2
            if(e.move.rb == null) return;
            e.move.rb.AddForce(e.moveInput.value.Get() * e.move.speed,ForceMode.Force);
        }
    }
}
