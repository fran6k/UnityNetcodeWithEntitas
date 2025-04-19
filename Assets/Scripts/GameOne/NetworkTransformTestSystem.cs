using Entitas;
using Unity.Netcode;
using UnityEngine;

public class NetworkTransformTestSystem : IExecuteSystem
{
    private Contexts _contexts;
    private PhysicsObjectView _physicsObjectView;
    private int pretick = 0;

    public NetworkTransformTestSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        var nowtick = NetworkManager.Singleton.NetworkTickSystem.ServerTime.Tick;
        var group = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.NetworkPosition, GameMatcher.WASDInput));
        foreach (var e in group)
        {
            // 如果是客户端的实体，则进行插值
            // if is a client entity, we interpolate the position, because the remote data is dispersed
            if (e.isClient)
            {
                var remotePosition = e.networkPosition.Value.Get();
                var localPosition = e.localPosition.Value.Get();
                var lerpPosition = Vector3.Lerp(localPosition, remotePosition, 0.2f);
                e.networkPosition.Value.Set(lerpPosition);
            }
            else
            {

                var originPosition = e.networkPosition.Value.Get();
                Vector3 weiyi = new Vector3(e.wASDInput.Value.x, 0f, e.wASDInput.Value.y).normalized * e.speed.Value * Time.deltaTime;
                originPosition += weiyi;
                e.networkPosition.Value.Set(originPosition);
            }
        }
    }
}