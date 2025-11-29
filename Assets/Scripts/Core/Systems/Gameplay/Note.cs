using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData noteData;
    public float fallSpeed;
    public float targetTime;
    public bool judged = false;

    void Update()
    {
        // ¿Ãµø
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
}
