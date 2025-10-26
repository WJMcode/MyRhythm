using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;        // 노트가 쳐야 하는 시간 (초)
    public int lane;          // 어느 레인에 나올지 (0~3 등)
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