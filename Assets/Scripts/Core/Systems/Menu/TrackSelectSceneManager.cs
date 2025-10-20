using UnityEngine;

public class TrackSelectSceneManager : BaseSceneManager
{
    [Header("Reference")]
    [SerializeField] private TrackSelector trackSelector;

    protected override void OnSubmit()
    {
        // 엔터 눌렀을 때 실행
        SceneTransitionManager.Instance.TransitionToScene("nextSceneName");
    }

    
}
