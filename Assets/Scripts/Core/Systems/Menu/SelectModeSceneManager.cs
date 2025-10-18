using UnityEngine;

public class SelectModeSceneManager : MonoBehaviour
{
    [Header("Reference")]
    private ButtonSelector buttonSelector;
    private SceneTransitionManager sceneTransitionManager;

    [Header("Scene Name")]
    [SerializeField] private string[] sceneNames;

    private bool isTransitioning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnterPressed()
    {
        isTransitioning = true;

        // 선택된 버튼에 따라 씬 결정
        int selectedIndex = buttonSelector.CurrentIndex;
        string targetScene = sceneNames[selectedIndex];

        // 씬 전환 (페이드아웃 포함)
        sceneTransitionManager.TransitionToScene(targetScene);
    }
}
