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

    [Header("Timing")]
    [SerializeField] private float noteSpeed = 5f;
    [SerializeField] private float spawnDistance = 10f;  // ���������� ���� �������� �Ÿ�

    [Header("Note Data")]
    [SerializeField] private List<NoteData> notes = new List<NoteData>();

    private int currentNoteIndex = 0;
    private float judgementLineY = -4f;  // ������ Y ��ǥ

    void Start()
    {
        // �׽�Ʈ�� ��Ʈ ����
        CreateTestNotes();
    }

    void Update()
    {
        if (!musicManager.IsPlaying) return;

        float currentTime = musicManager.GetCurrentTime();

        // ��Ʈ ���� üũ
        while (currentNoteIndex < notes.Count)
        {
            NoteData noteData = notes[currentNoteIndex];

            // ��Ʈ�� �������� �ð� ���
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
            Debug.LogError($"�߸��� ����: {noteData.lane}");
            return;
        }

        GameObject note = Instantiate(notePrefab, spawnPoints[noteData.lane].position, Quaternion.identity);
        Note noteScript = note.GetComponent<Note>();
        noteScript.noteData = noteData;

        Debug.Log($"��Ʈ ����: Lane {noteData.lane}, Time {noteData.time}");
    }

    void CreateTestNotes()
    {
        // �׽�Ʈ��: 1�ʸ��� �� ���ο� ��Ʈ ����
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
            // JSON �Ǵ� CSV �Ľ�
            LoadFromJSON(noteDataFile.text);
        }
        else
        {
            Debug.LogWarning("��Ʈ �����Ͱ� �����ϴ�. �׽�Ʈ ��Ʈ ����.");
            CreateTestNotes();
        }
    }

    void LoadFromJSON(string jsonData)
    {
        // JSON �Ľ� (���߿� ����)
        // NoteDataList data = JsonUtility.FromJson<NoteDataList>(jsonData);
        // notes = data.notes;
    }
}
