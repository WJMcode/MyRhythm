using UnityEngine;

public class FirstSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SceneTransitionManager SceneTransitionManager;

    [Header("Scene Settings")]
    [SerializeField] private string NextSceneName = "SelectModeScene";

    private bool CanTransition = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializedFirstScene();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();        
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
    private void HandleInput()
    {
        if(!CanTransition) { return; }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnterPressed();
        }
    }

    private void OnEnterPressed()
    {
        if (!CanTransition) { return; }

        CanTransition = false;

        if(SceneTransitionManager == null)
        {
            SceneTransitionManager.
        }
        else
        {
            Debug.LogError("SceneTransitionManager가 nullptr입니다!");
            CanTransition = true;
        }
    }
}
