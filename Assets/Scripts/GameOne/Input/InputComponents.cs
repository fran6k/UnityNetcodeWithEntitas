using Entitas;
using UnityEngine;

public interface IMoveInput
{
    Vector3 Get();
}

public class MoveInputComponent : IComponent
{
    public IMoveInput value;
}
