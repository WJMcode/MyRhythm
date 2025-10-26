using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData noteData;
    public float fallSpeed = 5f;

    void Update()
    {
        // �Ʒ��� ��������
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // ȭ�� �Ʒ��� ����� ����
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
