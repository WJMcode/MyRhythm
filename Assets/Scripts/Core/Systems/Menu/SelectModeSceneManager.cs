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
            // ���õ� ��ư�� ���� �� ����
            string targetScene = sceneNames[selectedIndex];
            // �� ��ȯ (���̵�ƿ� ����)
            SceneTransitionManager.Instance.TransitionToScene(targetScene);
        }
    }
}
