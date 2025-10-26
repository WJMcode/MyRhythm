using UnityEngine;

public class TrackSelectSceneManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TrackSelector trackSelector;

    void OnEnable()
    {
        trackSelector.OnTrackConfirmed += HandleTrackConfirmed;
    }

    void OnDisable()
    {
        trackSelector.OnTrackConfirmed -= HandleTrackConfirmed;
    }

    private void HandleTrackConfirmed()
    {
        // 인게임 씬으로 전환
        SceneTransitionManager.Instance.TransitionToScene("DJingScene");
    }
}
