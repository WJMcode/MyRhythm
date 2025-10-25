using UnityEngine;

[System.Serializable]
public class TrackData
{
    public string trackName;
    public FMODUnity.EventReference musicEvent;  // FMOD �̺�Ʈ
    public TextAsset noteDataFile;  // ��Ʈ ������ (JSON �Ǵ� CSV)
    public Sprite thumbnail;
    public float bpm;
}
