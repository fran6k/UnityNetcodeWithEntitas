using Entitas;
using UnityEngine;

public interface ILocalPosition
{
    Vector3 Get();
    void Set(Vector3 position);
}

public class LocalPositionComponent : IComponent
{
    public ILocalPosition Value;
}


