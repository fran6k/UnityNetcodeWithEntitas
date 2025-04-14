using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 moveVector;

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }
}