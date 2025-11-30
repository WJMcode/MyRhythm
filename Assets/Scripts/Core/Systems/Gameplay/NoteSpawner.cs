using UnityEngine;
using System.Collections.Generic;
using static TreeEditor.TreeEditorHelper;
using UnityEditor.Experimental.GraphView;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform judgementLine;

    [Header("Timing")]
    [SerializeField] private float noteSpeed = 5f;

    [Header("Note Data")]
    [SerializeField] private List<NoteData> notes = new List<NoteData>();
    private int currentNoteIndex = 0;
    private float spawnDistance;

    // 활성 노트 리스트
    [HideInInspector] public List<Note> activeNotes = new List<Note>();

    void Start()
    {
        if (judgementLine == null)
        {
            Debug.LogError("Judgement Line이 할당되지 않았습니다!");
            return;
        }

        spawnDistance = spawnPoints[0].position.y - judgementLine.position.y;
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
        if (!musicManager.IsPlaying) return;

        float currentTime = musicManager.GetCurrentTime();
        SpawnNotesAtTime(currentTime);
    }
    void SpawnNotesAtTime(float currentTime)
    {
        while (currentNoteIndex < notes.Count)
        {
            NoteData noteData = notes[currentNoteIndex];
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

        Transform spawnPoint = spawnPoints[noteData.lane];
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);

        Note note = noteObj.GetComponent<Note>();
        note.noteData = noteData;
        note.fallSpeed = noteSpeed;
        note.targetTime = noteData.time;

        activeNotes.Add(note);
    }
      

    void CreateTestNotes()
    { 
        // 테스트용: 2초부터 0.5초마다 각 레인에 노트
        for (int i = 0; i < 20; i++)
        {
            NoteData noteData = new NoteData
            {
                time = 2f + (i * 1f),
                //lane = i % spawnPoints.Length,
                lane = 0,
                type = NoteType.Normal

            };

            notes.Add(noteData
            );
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
