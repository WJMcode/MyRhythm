using UnityEngine;
using UnityEngine.InputSystem;

public class DJInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    // 다른 스크립트에서 구독할 이벤트
    public event System.Action<int> OnLaneInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InputSystem.actions.Disable();
        playerInput.currentActionMap?.Enable();
    }

    public void OnLane0(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Lane 0 입력!");
            OnLaneInput?.Invoke(0);
        }
    }
    public void OnLane1(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Lane 1 입력!");
            OnLaneInput?.Invoke(1);
        }
    }
    public void OnLane2(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Lane 2 입력!");
            OnLaneInput?.Invoke(2);
        }
    }
    public void OnLane3(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Lane 3 입력!");
            OnLaneInput?.Invoke(3);
        }
    }
    public void OnLane4(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Lane 4 입력!");
            OnLaneInput?.Invoke(4);
        }
    }
}
