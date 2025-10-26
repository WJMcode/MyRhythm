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
        // �ΰ��� ������ ��ȯ
        SceneTransitionManager.Instance.TransitionToScene("DJingScene");
    }
}
