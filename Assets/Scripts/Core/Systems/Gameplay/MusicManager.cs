using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private EventReference musicEventRef;
    private FMOD.Studio.EventInstance musicEvent;

    [Header("Timing")]
    [SerializeField] private float bpm = 120f;

    public float BPM => bpm;
    public bool IsPlaying { get; private set; }

    public void Initialize(EventReference eventRef, float trackBpm)
    {
        musicEventRef = eventRef;
        bpm = trackBpm;
    }

    public void StartMusic()
    {
        if (!musicEventRef.IsNull)
        {
            musicEvent = RuntimeManager.CreateInstance(musicEventRef);
            musicEvent.start();
            IsPlaying = true;
            Debug.Log($"Music Started! BPM: {bpm}");
        }
    }

    public float GetCurrentTime()
    {
        if (musicEvent.isValid())
        {
            musicEvent.getTimelinePosition(out int position);
            return position / 1000f;
        }
        return 0f;
    }

    void OnDestroy()
    {
        if (musicEvent.isValid())
        {
            musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicEvent.release();
        }
    }
}