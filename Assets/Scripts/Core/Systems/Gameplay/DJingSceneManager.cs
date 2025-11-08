//using TMPro;
using UnityEngine;

public class DJingSceneManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private NoteSpawner noteSpawner;

    // 화면에 표시될 트랙 이름 (현재 사용하지 않음)
    //[Header("UI")]
    //[SerializeField] private TextMeshProUGUI trackNameText;

    void Start()
    {
        LoadTrackData();
    }

    void LoadTrackData()
    {
        if (GameData.Instance != null && GameData.Instance.selectedTrack != null)
        {
            TrackData track = GameData.Instance.selectedTrack;

            Debug.Log($"트랙 로드: {track.trackName}");

            // UI 업데이트
            //if (trackNameText != null)
            //    trackNameText.text = track.trackName;

            // 음악 초기화
            musicManager.Initialize(track.musicEvent, track.bpm);

            // 노트 데이터 로드
            noteSpawner.LoadNoteData(track.noteDataFile);

            // 게임 시작 (1초 후)
            Invoke(nameof(StartGame), 1f);
        }
        else
        {
            Debug.LogError("트랙 데이터가 없습니다!");
        }
    }

    void StartGame()
    {
        musicManager.StartMusic();
        Debug.Log("게임 시작!");
    }
}
