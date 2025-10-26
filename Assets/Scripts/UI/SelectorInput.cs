using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    private InputAction selectorMoveAction;
    private InputAction confirmAction;

    // ���� �Է� �̺�Ʈ (1: �Ʒ�, -1: ��)
    public event Action<int> OnMove;
    public event Action OnConfirm;

    void Awake()
    {
        if (inputActions != null)
        {
            selectorMoveAction = inputActions.FindAction("UI/Selector");
            if (selectorMoveAction == null)
                Debug.LogWarning("UI/Selector �׼��� ã�� �� �����ϴ�.");

            confirmAction = inputActions.FindAction("UI/Submit");
            if (confirmAction == null)
                Debug.LogWarning("UI/Submit �׼��� ã�� �� �����ϴ�.");
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
        else
        {
            Debug.LogWarning("selectorMoveAction�� null�Դϴ�.");
        }

        if (confirmAction != null)
        {
            confirmAction.Enable();
            confirmAction.performed += OnConfirmPerformed;
        }
        else
        {
            Debug.LogWarning("confirmAction�� null�Դϴ�.");
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
        else
        {
            Debug.LogWarning("OnDisable() : selectorMoveAction�� �̹� null�Դϴ�.");
        }

        if (confirmAction != null)
        {
            confirmAction.performed -= OnConfirmPerformed;
            confirmAction.Disable();
        }
        else
        {
            Debug.LogWarning("OnDisable() : confirmAction�� �̹� null�Դϴ�.");
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

    void OnConfirmPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnConfirm?.Invoke();
    }
}
