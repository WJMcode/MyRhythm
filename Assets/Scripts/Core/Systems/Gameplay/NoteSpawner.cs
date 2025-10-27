using UnityEngine;
using System.Collections.Generic;
using static TreeEditor.TreeEditorHelper;
using UnityEditor.Experimental.GraphView;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform[] spawnPoints;  // �� ������ ���� ��ġ
    [SerializeField] private Transform judgementLine;  // ������ Transform


    [Header("Timing")]
    [SerializeField] private float noteSpeed = 5f;

    [Header("Note Data")]
    [SerializeField] private List<NoteData> notes = new List<NoteData>();
    private int currentNoteIndex = 0;
    private float spawnDistance;

    void Start()
    {
        // ���� Transform ��ġ�� �Ÿ� ���
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
                Debug.Log($"��Ʈ {notes.Count}�� �ε� �Ϸ�");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"��Ʈ ������ �ε� ����: {e.Message}");
                CreateTestNotes();
            }
        }
        else
        {
            Debug.LogWarning("��Ʈ �����Ͱ� �����ϴ�. �׽�Ʈ ��Ʈ ����");
            CreateTestNotes();
        }
    }

    void Update()
    {
        //if (!musicManager.IsPlaying) return;

        //float currentTime = musicManager.GetCurrentTime();

        //// ��Ʈ ���� üũ
        //while (currentNoteIndex < notes.Count)
        //{
        //    NoteData noteData = notes[currentNoteIndex];

        //    // ��Ʈ�� �������� �ð� ���
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

            // ��Ʈ�� �������� �� �ɸ��� �ð�
            float travelTime = spawnDistance / noteSpeed;

            // �����ؾ� �ϴ� �ð� = ���� �ð� - �̵� �ð�
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
            Debug.LogError($"�߸��� ����: {noteData.lane}");
            return;
        }

        Vector3 spawnPos = spawnPoints[noteData.lane].position;
        GameObject noteObj = Instantiate(notePrefab, spawnPos, Quaternion.identity, transform);

        Note note = noteObj.GetComponent<Note>();
        note.noteData = noteData;
        note.fallSpeed = noteSpeed;

        Debug.Log($"��Ʈ ����: Lane {noteData.lane}, Time {noteData.time}");
    }

    void CreateTestNotes()
    {
        // �׽�Ʈ��: 2�ʺ��� 0.5�ʸ��� �� ���ο� ��Ʈ
        for (int i = 0; i < 20; i++)
        {
            notes.Add(new NoteData
            {
                time = 2f + (i * 0.5f),
                lane = i % spawnPoints.Length,
                type = NoteType.Normal
            });
        }
        Debug.Log($"�׽�Ʈ ��Ʈ {notes.Count}�� ����");
    }

    void LoadFromJSON(string jsonData)
    {
        // JSON �Ľ� (���߿� ����)
        // NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonData);
        // notes = data.notes;
    }
}
