using UnityEngine;

public class TrackSelectSceneManager : BaseSceneManager
{
    [Header("Reference")]
    [SerializeField] private TrackSelector trackSelector;

    protected override void OnSubmit()
    {
        // ���� ������ �� ����
        SceneTransitionManager.Instance.TransitionToScene("nextSceneName");
    }

    
}
