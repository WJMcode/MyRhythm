using UnityEngine;

public class FirstSceneManager : BaseSceneManager
{
    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName;

    protected override void OnSubmit()
    {
        // 엔터 눌렀을 때 실행
        SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
