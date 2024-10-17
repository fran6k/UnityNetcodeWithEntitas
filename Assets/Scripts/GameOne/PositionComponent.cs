﻿using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

public interface INetworkPosition
{
    Vector3 Get();
    void Set(Vector3 position);
}
public interface ILocalPosition
{
    Vector3 Get();
}

public class NetworkPositionComponent : IComponent
{
    public INetworkPosition Value;
}
public class LocalPositionComponent : IComponent
{
    public ILocalPosition Value;
}

public class ClientComponent : IComponent
{
    
}
