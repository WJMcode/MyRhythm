using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectModeSceneManager : BaseSceneManager
{
    [Header("Reference")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("Scene Settings")]
    [SerializeField] private string[] sceneNames;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSubmit()
    {
        int selectedIndex = buttonSelector.CurrentIndex;

        if (selectedIndex >= 0 && selectedIndex < sceneNames.Length)
        {
            // 선택된 버튼에 따라 씬 결정
            string targetScene = sceneNames[selectedIndex];
            // 씬 전환 (페이드아웃 포함)
            SceneTransitionManager.Instance.TransitionToScene(targetScene);
        }
    }
}
