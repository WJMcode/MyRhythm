using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction selectorMoveAction;

    [Header("Button Settings")]
    [SerializeField] private Button[] buttons;

    [Header("Size Settings")]
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float scaleSpeed = 10f;

    private int currentIndex = 0;

    // �б� ���� ������Ƽ : �ٸ� Ŭ�������� ���� �ε����� ���� ���� ����(���� �Ұ���)
    public int CurrentIndex => currentIndex;

    void Awake()
    {
        // Input Actions���� Selector �׼� ��������
        if (InputActions != null)
        {
            selectorMoveAction = InputActions.FindAction("UI/Selector");

            if (selectorMoveAction == null)
            {
                Debug.LogWarning("SelectorMove �׼��� ã�� �� �����ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset�� �Ҵ���� �ʾҽ��ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
        }
    }

    // New Input System�� ���� �Լ���(OnEnable, OnDisable)
    // ��ũ��Ʈ Ȱ��ȭ ��, Input Action Ȱ��ȭ�ϰ� �̺�Ʈ�� ����
    void OnEnable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.Enable();
            selectorMoveAction.performed += OnSelectorMovePerformed;
        }
    }

    // ��ũ��Ʈ ��Ȱ��ȭ ��, �̺�Ʈ ������ �����ϰ� Input Action ��Ȱ��ȭ
    void OnDisable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.performed -= OnSelectorMovePerformed;
            selectorMoveAction.Disable();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ù ��° ��ư ����
        SelectButton(0);
    }

    // Update is called once per frame
    void Update()
    {
        // �ε巯�� ũ�� ����
        UpdateButtonScales();
    }
    private void OnSelectorMovePerformed(InputAction.CallbackContext context)
    {
        // float ������ �б� (1D Axis��)
        // Axis�� ��-�Ʒ�, ����-������ �� 1���� �������� ��Ÿ��
        // ���� Vector2�� �����ϸ� ��-�Ʒ�-��-�� �� 2���� �������� ��Ÿ�� (0, 1) (0, -1) (-1, 0) (1, 0) ��
        float input = context.ReadValue<float>();

        // input > 0: Positive (�Ʒ� ����)
        if (input > 0)
        {
            if (currentIndex < buttons.Length - 1)
            {
                SelectButton(currentIndex + 1);
            }
        }
        // input < 0: Negative (�� ����)
        else if (input < 0)
        {
            if (currentIndex > 0)
            {
                SelectButton(currentIndex - 1);
            }
        }
    }

    void SelectButton(int index)
    {
        currentIndex = index;
    }

    void UpdateButtonScales()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            float targetScale = (i == currentIndex) ? selectedScale : normalScale;
            RectTransform rect = buttons[i].GetComponent<RectTransform>();
            
            // Lerp�� ��ǥ�� ����������� ��ȭ���� �۾���
            // ��ǥ���� ���� �Ѿ�� ����
            // ��ǥ���� �����ϸ� ��ȭ�� ���� ������
            rect.localScale = Vector3.Lerp(
                rect.localScale,            // ����
                Vector3.one * targetScale,  // ��ǥ
                Time.deltaTime * scaleSpeed // ����
            );
        }
    }

    public Button GetSelectedButton()
    {
        return buttons[currentIndex];
    }
}
