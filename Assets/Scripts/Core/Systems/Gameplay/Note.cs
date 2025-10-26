using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData noteData;
    public float fallSpeed = 5f;

    void Update()
    {
        // 아래로 떨어지기
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 화면 아래로 벗어나면 삭제
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
