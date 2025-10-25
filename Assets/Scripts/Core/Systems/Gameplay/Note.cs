using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;

    public NoteData noteData;

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
