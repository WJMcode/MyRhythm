using UnityEngine;
using UnityEngine.UI;

public class MovingBackgroundImage : MonoBehaviour
{
    [Header("Movement Settings")]
    // 배경 이미지 움직임값
    public Vector2 ScrollSpeed = new Vector2(0.5f, 0.2f);
    // RawImage 사용?
    private bool UseRawImage = true;

    // RawImage 참조
    private RawImage BackgroundRawImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(UseRawImage)
        {
            // RawImage 참조하는 부분
            BackgroundRawImage = GetComponent<RawImage>();
            if(!BackgroundRawImage)
            {
                Debug.LogError("Raw Image Component가 필요합니다!");
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
