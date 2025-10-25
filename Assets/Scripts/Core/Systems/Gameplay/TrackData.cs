using UnityEngine;

[System.Serializable]
public class TrackData
{
    public string trackName;
    public FMODUnity.EventReference musicEvent;  // FMOD 이벤트
    public TextAsset noteDataFile;  // 노트 데이터 (JSON 또는 CSV)
    public Sprite thumbnail;
    public float bpm;
}
