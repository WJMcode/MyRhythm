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
        // Input Actions���� Submit �׼� ��������
        if (InputActions != null)
        {
            SubmitAction = InputActions.FindAction("UI/Submit");

            if (SubmitAction == null)
            {
                Debug.LogWarning("Submit �׼��� ã�� �� �����ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset�� �Ҵ���� �ʾҽ��ϴ�. �⺻ Ű ���ε��� ����մϴ�.");
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
            Debug.LogError("SceneTransitionManager�� ã�� �� �����ϴ�!");
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
        Debug.Log("Enter Ű �Է� ����");

        if (SceneTransitionManager != null)
        {
            CanTransition = false;
            SceneTransitionManager.TransitionToScene(NextSceneName);
        }
    }
}
