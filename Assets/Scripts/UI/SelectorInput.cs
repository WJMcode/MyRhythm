using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction selectorMoveAction;

    // ���� �Է� �̺�Ʈ (1: �Ʒ�, -1: ��)
    public event Action<int> OnMove;

    void Awake()
    {
        if (inputActions != null)
        {
            selectorMoveAction = inputActions.FindAction("UI/Selector");
            if (selectorMoveAction == null)
                Debug.LogWarning("UI/Selector �׼��� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.LogWarning("InputActionAsset�� �������� �ʾҽ��ϴ�.");
        }
    }

    // New Input System�� ���� �Լ���(OnEnable, OnDisable)
    // ��ũ��Ʈ Ȱ��ȭ ��, Input Action Ȱ��ȭ�ϰ� �̺�Ʈ�� ����
    void OnEnable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.Enable();
            selectorMoveAction.performed += OnMovePerformed;
        }
    }

    // ��ũ��Ʈ ��Ȱ��ȭ ��, �̺�Ʈ ������ �����ϰ� Input Action ��Ȱ��ȭ
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
        // float ������ �б� (1D Axis��)
        // Axis�� ��-�Ʒ�, ����-������ �� 1���� �������� ��Ÿ��
        // ���� Vector2�� �����ϸ� ��-�Ʒ�-��-�� �� 2���� �������� ��Ÿ�� (0, 1) (0, -1) (-1, 0) (1, 0) ��
        float input = context.ReadValue<float>();

        if (input > 0)
            OnMove?.Invoke(+1);   // �Ʒ���
        else if (input < 0)
            OnMove?.Invoke(-1);   // ����
    }
}
