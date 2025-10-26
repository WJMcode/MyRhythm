using TMPro;
using UnityEngine;

public class DJingSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private NoteSpawner noteSpawner;

    // ȭ�鿡 ǥ�õ� Ʈ�� �̸� (���� ������� ����)
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI trackNameText;

    void Start()
    {
        LoadTrackData();
    }

    void LoadTrackData()
    {
        if (GameData.Instance != null && GameData.Instance.selectedTrack != null)
        {
            TrackData track = GameData.Instance.selectedTrack;

            Debug.Log($"Ʈ�� �ε�: {track.trackName}");

            // UI ������Ʈ
            if (trackNameText != null)
                trackNameText.text = track.trackName;

            // ���� �ʱ�ȭ
            musicManager.Initialize(track.musicEvent, track.bpm);

            // ��Ʈ ������ �ε�
            noteSpawner.LoadNoteData(track.noteDataFile);

            // ���� ���� (1�� ��)
            Invoke(nameof(StartGame), 1f);
        }
        else
        {
            Debug.LogError("Ʈ�� �����Ͱ� �����ϴ�!");
        }
    }

    void StartGame()
    {
        musicManager.StartMusic();
        Debug.Log("���� ����!");
    }
}
