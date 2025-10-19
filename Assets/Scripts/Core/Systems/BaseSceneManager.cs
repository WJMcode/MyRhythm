using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseSceneManager : MonoBehaviour
{
    [Header("Input Settings - Submit")]
    [SerializeField] private InputActionAsset InputActions;
    protected InputAction submitAction;

    void Awake()
    {
        // Input Actions���� Submit �׼� ��������
        if (InputActions != null)
        {
            submitAction = InputActions.FindAction("UI/Submit");

            if (submitAction == null)
            {
                Debug.LogWarning("Submit �׼��� ã�� �� �����ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset�� �Ҵ���� �ʾҽ��ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
        }
    }

    protected virtual void OnEnable()
    {
        if (submitAction != null)
        {
            submitAction.Enable();
            submitAction.performed += OnSubmitPerformed;
        }
    }

    protected virtual void OnDisable()
    {
        if (submitAction != null)
        {
            submitAction.performed -= OnSubmitPerformed;
            submitAction.Disable();
        }
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        OnSubmit();
    }

    // �ڽ� Ŭ�������� �����ؾ� �ϴ� �߻� �޼���
    protected abstract void OnSubmit();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
