using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseSceneManager : MonoBehaviour
{
    [Header("Input Settings - Submit")]
    [SerializeField] private InputActionAsset InputActions;
    protected InputAction submitAction;

    void Awake()
    {
        // Input Actions에서 Submit 액션 가져오기
        if (InputActions != null)
        {
            submitAction = InputActions.FindAction("UI/Submit");

            if (submitAction == null)
            {
                Debug.LogWarning("Submit 액션을 찾을 수 없습니다. 기본 키 바인딩을 사용합니다.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset이 할당되지 않았습니다. 기본 키 바인딩을 사용합니다.");
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

    // 자식 클래스에서 구현해야 하는 추상 메서드
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
