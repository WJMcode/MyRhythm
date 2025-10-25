using UnityEngine;
using System.Collections.Generic;
using static TreeEditor.TreeEditorHelper;
using UnityEditor.Experimental.GraphView;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform[] spawnPoints;  // 각 레인의 스폰 위치

    [Header("Timing")]
    [SerializeField] private float noteSpeed = 5f;
    [SerializeField] private float spawnDistance = 10f;  // 판정선에서 스폰 지점까지 거리

    [Header("Note Data")]
    [SerializeField] private List<NoteData> notes = new List<NoteData>();

    private int currentNoteIndex = 0;
    private float judgementLineY = -4f;  // 판정선 Y 좌표

    void Start()
    {
        // 테스트용 노트 생성
        CreateTestNotes();
    }

    void Update()
    {
        if (!musicManager.IsPlaying) return;

        float currentTime = musicManager.GetCurrentTime();

        // 노트 스폰 체크
        while (currentNoteIndex < notes.Count)
        {
            NoteData noteData = notes[currentNoteIndex];

            // 노트가 떨어지는 시간 계산
            float travelTime = spawnDistance / noteSpeed;
            float spawnTime = noteData.time - travelTime;

            if (currentTime >= spawnTime)
            {
                SpawnNote(noteData);
                currentNoteIndex++;
            }
            else
            {
                break;
            }
        }
    }

    void SpawnNote(NoteData noteData)
    {
        if (noteData.lane < 0 || noteData.lane >= spawnPoints.Length)
        {
            Debug.LogError($"잘못된 레인: {noteData.lane}");
            return;
        }

        GameObject note = Instantiate(notePrefab, spawnPoints[noteData.lane].position, Quaternion.identity);
        Note noteScript = note.GetComponent<Note>();
        noteScript.noteData = noteData;

        Debug.Log($"노트 생성: Lane {noteData.lane}, Time {noteData.time}");
    }

    void CreateTestNotes()
    {
        // 테스트용: 1초마다 각 레인에 노트 생성
        for (int i = 0; i < 16; i++)
        {
            notes.Add(new NoteData
            {
                time = i * 0.5f,
                lane = i % spawnPoints.Length,
                type = NoteType.Normal
            });
        }
    }

    public void LoadNoteData(TextAsset noteDataFile)
    {
        if (noteDataFile != null)
        {
            // JSON 또는 CSV 파싱
            LoadFromJSON(noteDataFile.text);
        }
        else
        {
            Debug.LogWarning("노트 데이터가 없습니다. 테스트 노트 생성.");
            CreateTestNotes();
        }
    }

    void LoadFromJSON(string jsonData)
    {
        // JSON 파싱 (나중에 구현)
        // NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonData);
        // notes = data.notes;
    }
}
