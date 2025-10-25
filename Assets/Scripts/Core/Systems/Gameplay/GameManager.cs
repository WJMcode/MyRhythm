using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private NoteSpawner noteSpawner;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI trackNameText;

    void Start()
    {
        // ���޹��� Ʈ�� ������ �ε�
        if (GameData.Instance != null && GameData.Instance.selectedTrack != null)
        {
            TrackData track = GameData.Instance.selectedTrack;

            // UI ������Ʈ
            if (trackNameText != null)
                trackNameText.text = track.trackName;

            // ���� ����
            musicManager.Initialize(track.musicEvent, track.bpm);

            // ��Ʈ ������ �ε�
            noteSpawner.LoadNoteData(track.noteDataFile);

            // ���� ����
            musicManager.StartMusic();
        }
        else
        {
            Debug.LogError("Ʈ�� �����Ͱ� �����ϴ�! TrackSelect ������ �����ϼ���.");
        }
    }
}
