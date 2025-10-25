using UnityEngine;

public class TrackSelectSceneManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TrackSelector trackSelector;

    void OnDisable()
    {
        trackSelector.OnTrackConfirmed += HandleTrackConfirmed;
    }

    void OnDestroy()
    {
        trackSelector.OnTrackConfirmed -= HandleTrackConfirmed;
    }
    private void HandleTrackConfirmed()
    {
        // �ΰ��� ������ ��ȯ
        SceneTransitionManager.Instance.TransitionToScene("DJingScene");
    }
}
