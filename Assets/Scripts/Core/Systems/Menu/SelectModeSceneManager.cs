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

        // ���õ� ��ư�� ���� �� ����
        int selectedIndex = buttonSelector.CurrentIndex;
        string targetScene = sceneNames[selectedIndex];

        // �� ��ȯ (���̵�ƿ� ����)
        sceneTransitionManager.TransitionToScene(targetScene);
    }
}
