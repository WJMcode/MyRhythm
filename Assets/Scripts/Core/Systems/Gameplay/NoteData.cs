using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;        // 음악 재생 (time)초 시점에 판정선에서 쳐야 함
    public int lane;          // 어느 레인에 나올지 (0~4 등)
    public NoteType type;     // 일반, 롱노트 등
}

public enum NoteType
{
    Normal,
    Long
}

[System.Serializable]
public class NoteDataList
{
    public NoteData[] notes;
}