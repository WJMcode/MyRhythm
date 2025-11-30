using UnityEngine;
using System.Collections.Generic;

public class JudgementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DJInputManager inputManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private NoteSpawner noteSpawner;
    [SerializeField] private Transform judgementLine;

    [Header("Judgement Settings (Early Timing)")]
    [SerializeField] private float perfectWindow = 0.05f;   // 50ms 빠르게
    [SerializeField] private float coolWindow = 0.1f;       // 100ms 빠르게
    [SerializeField] private float goodWindow = 0.15f;      // 150ms 빠르게
    [SerializeField] private float missWindow = 0.3f;       // 300ms 빠르게
    [SerializeField] private float failThreshold = 0.1f;    // 100ms 늦으면 자동 FAIL

    [Header("Judgement Area")]
    [SerializeField] private float judgementAreaHeight = 1.5f;

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
        if (!musicManager.IsPlaying) return;

        float currentTime = musicManager.GetCurrentTime();

        // 자동 FAIL 처리: targetTime을 지나쳐서 너무 늦은 노트
        for (int i = noteSpawner.activeNotes.Count - 1; i >= 0; i--)
        {
            Note note = noteSpawner.activeNotes[i];

            if (note == null)
            {
                noteSpawner.activeNotes.RemoveAt(i);
                continue;
            }

            if (note.judged) continue;

            float timeDiff = currentTime - note.targetTime;

            // targetTime 지나서 failThreshold 초과하면 FAIL
            if (timeDiff > failThreshold)
            {
                Debug.Log($"자동 FAIL: targetTime={note.targetTime:F2}, 현재={currentTime:F2}, 늦음={timeDiff:F3}");
                JudgeNote(note, timeDiff, "FAIL");
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
            float timeDiff = currentTime - note.targetTime;

            // 입력 가능한 범위: 빠르게만 (-missWindow ~ +failThreshold)
            // 너무 빠르거나 너무 늦으면 무시
            if (timeDiff < -missWindow || timeDiff > failThreshold)
                continue;

            // 위치도 체크
            if (distance > judgementAreaHeight)
                continue;

            // 가장 시간 차이가 작은 노트 (절댓값)
            float absTimeDiff = Mathf.Abs(timeDiff);
            if (absTimeDiff < closestTimeDiff)
            {
                closestTimeDiff = absTimeDiff;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            float timingDiff = currentTime - closestNote.targetTime;

            // 단방향 판정 (빠르게만)
            string judgement = GetJudgement(timingDiff);
            JudgeNote(closestNote, timingDiff, judgement);
        }
        else
        {
            Debug.Log($"Lane {lane}에 판정 가능한 노트 없음!");
        }
    }

    // 단방향 판정 로직
    string GetJudgement(float timingDiff)
    {
        // timingDiff가 음수 = 빠르게 입력 (targetTime 전)
        // timingDiff가 양수 = 늦게 입력 (targetTime 후)

        if (timingDiff > failThreshold)
        {
            return "FAIL";  // 너무 늦음
        }
        else if (timingDiff >= -perfectWindow)
        {
            // -0.05 ~ 0.1 범위
            return "PERFECT";
        }
        else if (timingDiff >= -coolWindow)
        {
            // -0.1 ~ -0.05 범위
            return "COOL";
        }
        else if (timingDiff >= -goodWindow)
        {
            // -0.15 ~ -0.1 범위
            return "GOOD";
        }
        else if (timingDiff >= -missWindow)
        {
            // -0.3 ~ -0.15 범위
            return "MISS";
        }
        else
        {
            // -0.3 이전 = 너무 빠름
            return "MISS";
        }
    }

    void JudgeNote(Note note, float timingDiff, string judgement)
    {
        if (note.judged) return;
        note.judged = true;

        switch (judgement)
        {
            case "PERFECT":
                Debug.Log($"<color=yellow>PERFECT!</color> (차이: {timingDiff:F3}s)");
                break;
            case "COOL":
                Debug.Log($"<color=green>COOL!</color> (차이: {timingDiff:F3}s)");
                break;
            case "GOOD":
                Debug.Log($"<color=blue>GOOD</color> (차이: {timingDiff:F3}s)");
                break;
            case "MISS":
                Debug.Log($"<color=purple>MISS</color> (차이: {timingDiff:F3}s)");
                break;
            case "FAIL":
                Debug.Log($"<color=red>FAIL</color> (차이: {timingDiff:F3}s)");
                break;
        }

        noteSpawner.activeNotes.Remove(note);
        Destroy(note.gameObject);
    }
}
