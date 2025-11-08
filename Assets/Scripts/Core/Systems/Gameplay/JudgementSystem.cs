using UnityEngine;
using System.Collections.Generic;

public class JudgementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DJInputManager inputManager;
    //[SerializeField] private Transform[] judgementAreas;  // 각 레인의 판정 영역
    [SerializeField] private MusicManager musicManager;

    [Header("Judgement Settings")]
    [SerializeField] private float perfectWindow = 0.05f;  // ±50ms
    [SerializeField] private float greatWindow = 0.1f;     // ±100ms
    [SerializeField] private float goodWindow = 0.15f;     // ±150ms

    [Header("Judgement Area")]
    [SerializeField] private float judgementRadius = 0.5f;  // 판정 범위 반경

    void Start()
    {
        if (inputManager != null)
            inputManager.OnLaneInput += OnKeyPressed;
    }

    void OnDestroy()
    {
        if (inputManager != null)
            inputManager.OnLaneInput -= OnKeyPressed;
    }

    void OnKeyPressed(int lane)
    {
        Debug.Log($"판정 체크: Lane {lane}");

        // FMOD 기반: 현재 음악 시간
        float currentMusicTime = musicManager.GetCurrentTime();

        // 해당 레인의 노트 찾기
        Note targetNote = FindNoteByMusicTime(lane, currentMusicTime);

        if (targetNote != null)
        {
            // 음악 시간 기반 판정!
            float timingDiff = currentMusicTime - targetNote.targetTime;
            JudgeNote(targetNote, timingDiff);
        }
        else
        {
            Debug.Log($"Lane {lane}에 노트 없음 - Miss!");
        }
    }

    // 음악 시간 기반으로 노트 찾기
    Note FindNoteByMusicTime(int lane, float currentTime)
    {
        Note[] allNotes = FindObjectsByType<Note>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Note closestNote = null;
        float closestTimeDiff = float.MaxValue;

        foreach (Note note in allNotes)
        {
            // 같은 레인인지 확인
            if (note.noteData == null || note.noteData.lane != lane)
                continue;

            // 시간 차이 계산 (음악 시간 기준!)
            float timeDiff = Mathf.Abs(currentTime - note.targetTime);

            // 판정 범위 내인지 (goodWindow 이내)
            if (timeDiff > goodWindow)
                continue;

            // 가장 가까운 시간의 노트
            if (timeDiff < closestTimeDiff)
            {
                closestTimeDiff = timeDiff;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            Debug.Log($"노트 발견! 시간 차이: {closestTimeDiff:F3}초");
        }

        return closestNote;
    }

    void JudgeNote(Note note, float timingDiff)
    {
        float absTimingDiff = Mathf.Abs(timingDiff);
        string judgement;

        if (absTimingDiff <= perfectWindow)
        {
            judgement = "PERFECT!";
            Debug.Log($"<color=yellow>PERFECT!</color> (차이: {timingDiff:F3}초)");
        }
        else if (absTimingDiff <= greatWindow)
        {
            judgement = "GREAT!";
            Debug.Log($"<color=green>GREAT!</color> (차이: {timingDiff:F3}초)");
        }
        else if (absTimingDiff <= goodWindow)
        {
            judgement = "GOOD";
            Debug.Log($"<color=blue>GOOD</color> (차이: {timingDiff:F3}초)");
        }
        else
        {
            judgement = "MISS";
            Debug.Log($"<color=red>MISS</color> (차이: {timingDiff:F3}초)");
            return;  // Miss는 노트 제거 안 함 (통과하도록)
        }

        // 노트 제거
        Destroy(note.gameObject);
    }
}
