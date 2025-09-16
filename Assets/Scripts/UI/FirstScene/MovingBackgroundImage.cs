using UnityEngine;
using UnityEngine.UI;

public class MovingBackgroundImage : MonoBehaviour
{
    [Header("Movement Settings")]
    // ��� �̹��� �����Ӱ�
    public Vector2 ScrollSpeed = new Vector2(0.5f, 0.2f);
    // RawImage ���?
    private bool UseRawImage = true;

    // RawImage ����
    private RawImage BackgroundRawImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(UseRawImage)
        {
            // RawImage �����ϴ� �κ�
            BackgroundRawImage = GetComponent<RawImage>();
            if(!BackgroundRawImage)
            {
                Debug.LogError("Raw Image Component�� �ʿ��մϴ�!");
            }
        }
        else
        {
            Debug.LogError("MovingBackgroundImage::UseRawImage == false.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BackgroundRawImage)
        {
            Rect UVRect = BackgroundRawImage.uvRect;
            UVRect.x = ScrollSpeed.x * Time.deltaTime;
            UVRect.y = ScrollSpeed.y * Time.deltaTime;
            BackgroundRawImage.uvRect = UVRect;
        }
    }
}
