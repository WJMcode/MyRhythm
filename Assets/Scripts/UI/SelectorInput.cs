using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction selectorMoveAction;

    // 방향 입력 이벤트 (1: 아래, -1: 위)
    public event Action<int> OnMove;

    void Awake()
    {
        if (inputActions != null)
        {
            selectorMoveAction = inputActions.FindAction("UI/Selector");
            if (selectorMoveAction == null)
                Debug.LogWarning("UI/Selector 액션을 찾을 수 없습니다.");
        }
        else
        {
            Debug.LogWarning("InputActionAsset이 설정되지 않았습니다.");
        }
    }

    // New Input System을 위한 함수들(OnEnable, OnDisable)
    // 스크립트 활성화 시, Input Action 활성화하고 이벤트를 구독
    void OnEnable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.Enable();
            selectorMoveAction.performed += OnMovePerformed;
        }
    }

    // 스크립트 비활성화 시, 이벤트 구독을 해제하고 Input Action 비활성화
    void OnDisable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.performed -= OnMovePerformed;
            selectorMoveAction.Disable();
        }
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        // float 값으로 읽기 (1D Axis용)
        // Axis는 위-아래, 왼쪽-오른쪽 등 1차원 움직임을 나타냄
        // 만약 Vector2로 설정하면 위-아래-좌-우 등 2차원 움직임을 나타냄 (0, 1) (0, -1) (-1, 0) (1, 0) 등
        float input = context.ReadValue<float>();

        if (input > 0)
            OnMove?.Invoke(+1);   // 아래쪽
        else if (input < 0)
            OnMove?.Invoke(-1);   // 위쪽
    }
}
