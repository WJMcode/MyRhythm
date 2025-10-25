using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;        // ��Ʈ�� �ľ� �ϴ� �ð� (��)
    public int lane;          // ��� ���ο� ������ (0~3 ��)
    public NoteType type;     // �Ϲ�, �ճ�Ʈ ��
}

public enum NoteType
{
    Normal,
    Long
}