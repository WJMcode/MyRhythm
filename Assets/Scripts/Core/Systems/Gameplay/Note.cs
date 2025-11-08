using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData noteData;
    public float fallSpeed = 5f;

    // 추가: 이 노트를 쳐야 하는 정확한 시간
    public float targetTime;  // noteData.time과 동일
    private bool judged = false;

    void Update()
    {
        // 이동
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Miss 체크 (판정 시간 지남)
        if (!judged)
        {
            float currentTime = Object.FindFirstObjectByType<MusicManager>().GetCurrentTime(); // Updated to use FindFirstObjectByType
            if (currentTime > targetTime + 0.15f)  // goodWindow 지남
            {
                Debug.Log("<color=red>MISS (시간 지남)</color>");
                judged = true;
                Destroy(gameObject, 1f);  // 1초 후 삭제
            }
        }

        // 화면 밖
        if (transform.position.y < -400f)
        {
            Destroy(gameObject);
        }
    }
}
