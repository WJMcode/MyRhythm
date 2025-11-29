using UnityEngine;
using System.Collections.Generic;

public class JudgementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DJInputManager inputManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private NoteSpawner noteSpawner;
    [SerializeField] private Transform judgementLine;

    [Header("Judgement Settings")]
    [SerializeField] private float perfectWindow = 0.05f;
    [SerializeField] private float coolWindow = 0.1f;
    [SerializeField] private float goodWindow = 0.15f;
    [SerializeField] private float missWindow = 0.5f;

    [Header("Judgement Area")]
    [SerializeField] private float judgementAreaHeight = 1.5f; // 위치 기준 판정 범위

    private float judgementLineY;

    void Start()
    {
        if (inputManager != null)
            inputManager.OnLaneInput += OnKeyPressed;

        if (judgementLine != null)
            judgementLineY = judgementLine.position.y;
    }

    void OnDestroy()
    {
        if (inputManager != null)
            inputManager.OnLaneInput -= OnKeyPressed;
    }

    void Update()
    {
        float currentTime = musicManager.GetCurrentTime();

        // 자동 FAIL 처리: 시간 초과된 노트만
        for (int i = noteSpawner.activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = noteSpawner.activeNotes[i];
            if (note.judged) continue;

            float timeDiff = currentTime - note.targetTime;
            if (timeDiff > missWindow)
            {
                JudgeNote(note, timeDiff, isAutoFail: true);
            }
        }
    }

    void OnKeyPressed(int lane)
    {
        float currentTime = musicManager.GetCurrentTime();
        Note closestNote = null;
        float closestTimeDiff = float.MaxValue;

        foreach (var note in noteSpawner.activeNotes)
        {
            if (note.judged || note.noteData.lane != lane)
                continue;

            float distance = Mathf.Abs(note.transform.position.y - judgementLineY);
            float timeDiff = Mathf.Abs(currentTime - note.targetTime);

            // 입력 가능한 범위: 판정선 근처 + missWindow 안
            if (distance <= judgementAreaHeight && timeDiff <= missWindow)
            {
                if (timeDiff < closestTimeDiff)
                {
                    closestTimeDiff = timeDiff;
                    closestNote = note;
                }
            }
        }

        if (closestNote != null)
        {
            float timingDiff = currentTime - closestNote.targetTime;
            JudgeNote(closestNote, timingDiff, isAutoFail: false);
        }
        else
        {
            Debug.Log($"Lane {lane}에 판정 가능한 노트 없음!");
        }
    }
       

    void JudgeNote(Note note, float timingDiff, bool isAutoFail)
    {
        if (note.judged) return;

        note.judged = true;
        string judgement;
        float absDiff = Mathf.Abs(timingDiff);

        if (isAutoFail)
        {
            judgement = "FAIL";
            Debug.Log($"<color=red>FAIL</color> (차이: {timingDiff:F3}s)");
        }
        else if (absDiff <= perfectWindow)
        {
            judgement = "PERFECT";
            Debug.Log($"<color=yellow>PERFECT!</color> (차이: {timingDiff:F3}s)");
        }
        else if (absDiff <= coolWindow)
        {
            judgement = "COOL";
            Debug.Log($"<color=green>COOL!</color> (차이: {timingDiff:F3}s)");
        }
        else if (absDiff <= goodWindow)
        {
            judgement = "GOOD";
            Debug.Log($"<color=blue>GOOD</color> (차이: {timingDiff:F3}s)");
        }
        else
        {
            judgement = "MISS";
            Debug.Log($"<color=purple>MISS</color> (차이: {timingDiff:F3}s)");
        }

        // 노트 제거 및 리스트에서 제거
        noteSpawner.activeNotes.Remove(note);
        Destroy(note.gameObject);
    }
}
