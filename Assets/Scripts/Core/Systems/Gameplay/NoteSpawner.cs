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
    [SerializeField] private Transform judgementLine;  // 판정선 Transform


    [Header("Timing")]
    [SerializeField] private float noteSpeed = 5f;

    [Header("Note Data")]
    [SerializeField] private List<NoteData> notes = new List<NoteData>();
    private int currentNoteIndex = 0;
    private float spawnDistance;

    void Start()
    {
        // 실제 Transform 위치로 거리 계산
        spawnDistance = spawnPoints[0].position.y - judgementLine.position.y;
        Debug.Log($"Spawn Distance: {spawnDistance}");
    }

    public void LoadNoteData(TextAsset noteDataFile)
    {
        if (noteDataFile != null)
        {
            try
            {
                NoteDataList noteList = JsonUtility.FromJson<NoteDataList>(noteDataFile.text);
                notes = new List<NoteData>(noteList.notes);
                Debug.Log($"노트 {notes.Count}개 로드 완료");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"노트 데이터 로드 실패: {e.Message}");
                CreateTestNotes();
            }
        }
        else
        {
            Debug.LogWarning("노트 데이터가 없습니다. 테스트 노트 생성");
            CreateTestNotes();
        }
    }

    void Update()
    {
        //if (!musicManager.IsPlaying) return;

        //float currentTime = musicManager.GetCurrentTime();

        //// 노트 스폰 체크
        //while (currentNoteIndex < notes.Count)
        //{
        //    NoteData noteData = notes[currentNoteIndex];

        //    // 노트가 떨어지는 시간 계산
        //    float travelTime = spawnDistance / noteSpeed;
        //    float spawnTime = noteData.time - travelTime;

        //    if (currentTime >= spawnTime)
        //    {
        //        SpawnNote(noteData);
        //        currentNoteIndex++;
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}

        if (!musicManager.IsPlaying) return;

        float currentTime = musicManager.GetCurrentTime();
        SpawnNotesAtTime(currentTime);
    }

    void SpawnNotesAtTime(float currentTime)
    {
        while (currentNoteIndex < notes.Count)
        {
            NoteData noteData = notes[currentNoteIndex];

            // 노트가 떨어지는 데 걸리는 시간
            float travelTime = spawnDistance / noteSpeed;

            // 스폰해야 하는 시간 = 판정 시간 - 이동 시간
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

        //Vector3 spawnPos = spawnPoints[noteData.lane].position;
        //GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, transform);

        //Note note = noteObj.GetComponent<Note>();
        //note.noteData = noteData;
        //note.fallSpeed = noteSpeed;

        //Debug.Log($"노트 생성: Lane {noteData.lane}, Time {noteData.time}");

        GameObject noteObj = Instantiate(notePrefab, spawnPoints[noteData.lane]);

        Note note = noteObj.GetComponent<Note>();
        note.noteData = noteData;
        note.fallSpeed = noteSpeed;
        note.targetTime = noteData.time;  // 판정 시간 저장
    }

    void CreateTestNotes()
    {
        // 테스트용: 2초부터 0.5초마다 각 레인에 노트
        for (int i = 0; i < 20; i++)
        {
            notes.Add(new NoteData
            {
                time = 2f + (i * 1f),
                lane = i % spawnPoints.Length,
                type = NoteType.Normal
            });
        }
        Debug.Log($"테스트 노트 {notes.Count}개 생성");
    }

    void LoadFromJSON(string jsonData)
    {
        // JSON 파싱 (나중에 구현)
        // NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonData);
        // notes = data.notes;
    }
}
