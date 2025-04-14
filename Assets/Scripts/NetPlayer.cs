using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetPlayer : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    public PlayerInputActions playerInputActions;

    private NetworkVariable<Vector2> networkPlayerMove = new NetworkVariable<Vector2>(Vector2.zero);

    // 表达式用于初始化moveAxis获取rpgInputActions.PC.Move的返回值赋值给moveAxis
    private Vector2 moveAxis => playerInputActions.PC.moveControl.ReadValue<Vector2>();

    private void Awake() {
    	// 实例化脚本
        playerInputActions = new PlayerInputActions();
    }
    void OnEnable()
    {
    	// 使用前需要将该playerInputActions开启
        playerInputActions.PC.Enable();
    }
    void OnDisable()
    {
    	// 使用完需要将该rpgInputActions关闭
        playerInputActions.PC.Disable();
    }
    
    private void Update() {
        if (this.IsClient && this.IsOwner)
        {
            // 将读取到的Move返回值赋值给moveVector2 
            Vector2 moveVector2 = playerInputActions.PC.moveControl.ReadValue<Vector2>();
            
            // 因为我们的playerMove会在Update生命周期函数中逐帧执行，所以在执行前需要判断是否有按下对应的按键
            if (moveAxis!= Vector2.zero) {
                // 使用获取到的Vector2.x和Vector2.y返回值作为角色移动的参数
                PlayerMove(moveVector2.x,moveVector2.y);
            }

            UpdateMoveServerRpc(moveVector2);
        } else {
            PlayerMove(networkPlayerMove.Value.x, networkPlayerMove.Value.y);
        }
    }

    [ServerRpc]
    public void UpdateMoveServerRpc(Vector2 move)  // 函数名要以 ServerRpc 结尾
    {
        networkPlayerMove.Value = move;  // 传递位移信息
    }

    //使角色真正移动的方法
    private void PlayerMove(float horizontal,float vertical){
        transform.Translate(new Vector3(horizontal,0,vertical)*Time.deltaTime*moveSpeed, Space.World);
    }
}
