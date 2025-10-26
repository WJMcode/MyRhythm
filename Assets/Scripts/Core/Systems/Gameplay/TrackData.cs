using UnityEngine;
using FMODUnity;

[System.Serializable]
public class TrackData
{
    public string trackName;
    public RectTransform rectTransform;  // UI��
    public EventReference musicEvent;     // FMOD �̺�Ʈ
    public TextAsset noteDataFile;        // ��Ʈ ������ (JSON)
    public Sprite thumbnail;
    public float bpm = 120f;
}
