using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SceneTransitionManager SceneTransitionManager;

    [Header("Scene Settings")]
    [SerializeField] private bool CanTransition = true;
    [SerializeField] private string NextSceneName = "UI_SelectModeScene";

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction SubmitAction;

    void Awake()
    {
        // Input Actions에서 Submit 액션 가져오기
        if (InputActions != null)
        {
            SubmitAction = InputActions.FindAction("UI/Submit");

            if (SubmitAction == null)
            {
                Debug.LogWarning("Submit 액션을 찾을 수 없습니다. 기본 키 바인딩을 사용합니다.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset이 할당되지 않았습니다. 기본 키 바인딩을 사용합니다.");
        }
    }

    void OnEnable()
    {
        if (SubmitAction != null)
        {
            SubmitAction.Enable();
            SubmitAction.performed += OnSubmitPerformed;
        }
    }

    void OnDisable()
    {
        if (SubmitAction != null)
        {
            SubmitAction.performed -= OnSubmitPerformed;
            SubmitAction.Disable();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializedFirstScene();
    }
    
    // Update is called once per frame
    void Update()
    {      
    }

    private void InitializedFirstScene()
    {
        if(SceneTransitionManager == null)
        {
            SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        }

        if (SceneTransitionManager == null)
        {
            Debug.LogError("SceneTransitionManager을 찾을 수 없습니다!");
            return;
        }

        CanTransition = true;
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        if (CanTransition)
        {
            OnEnterPressed();
        }
    }
    private void OnEnterPressed()
    {
        Debug.Log("Enter 키 입력 감지");

        if (SceneTransitionManager != null)
        {
            CanTransition = false;
            SceneTransitionManager.TransitionToScene(NextSceneName);
        }
    }
}
