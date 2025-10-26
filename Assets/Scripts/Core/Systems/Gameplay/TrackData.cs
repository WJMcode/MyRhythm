using UnityEngine;
using FMODUnity;

[System.Serializable]
public class TrackData
{
    public string trackName;
    public RectTransform rectTransform;  // UI용
    public EventReference musicEvent;     // FMOD 이벤트
    public TextAsset noteDataFile;        // 노트 데이터 (JSON)
    public Sprite thumbnail;
    public float bpm = 120f;
}
