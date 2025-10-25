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
        // 전달받은 트랙 데이터 로드
        if (GameData.Instance != null && GameData.Instance.selectedTrack != null)
        {
            TrackData track = GameData.Instance.selectedTrack;

            // UI 업데이트
            if (trackNameText != null)
                trackNameText.text = track.trackName;

            // 음악 설정
            musicManager.Initialize(track.musicEvent, track.bpm);

            // 노트 데이터 로드
            noteSpawner.LoadNoteData(track.noteDataFile);

            // 게임 시작
            musicManager.StartMusic();
        }
        else
        {
            Debug.LogError("트랙 데이터가 없습니다! TrackSelect 씬에서 선택하세요.");
        }
    }
}
