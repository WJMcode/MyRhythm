using UnityEngine;

public class TrackSelector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SelectorInput selectorInput;

    [Header("Track List Settings")]
    [SerializeField] private RectTransform[] trackItems;   // �� Ʈ�� �̹��� RectTransform
    [SerializeField] private RectTransform contentParent;  // ��� Ʈ���� �θ�
    [SerializeField] private float moveSpeed = 10f;        // �߾� �̵� �ӵ�
    [SerializeField] private float scaleSpeed = 8f;        // ������ ���� �ӵ�

    [Header("Scale Settings")]
    [SerializeField] private float selectedScale = 1.2f;   // �߾� Ʈ�� ũ��
    [SerializeField] private float sideScale = 0.9f;       // �� �� Ʈ�� ũ��

    [Header("Position Offset Settings")]
    [SerializeField] private float sideOffsetX = 250f;     // �� ������ �󸶳� ��������
    [SerializeField] private float depthZ = 0f;            // �ʿ� �� ���̰� ǥ����

    private int currentIndex = 0;
    private Vector2 targetPosition;

    void Start()
    {
        if (selectorInput != null)
            selectorInput.OnMove += MoveSelection;

        UpdateTargetPosition();
        UpdateTrackVisuals(true); // �ʱ� ���� ��� �ݿ�
    }

    void OnDestroy()
    {
        if (selectorInput != null)
            selectorInput.OnMove -= MoveSelection;
    }

    void Update()
    {
        // ���õ� Ʈ���� �߾ӿ� ������ �ε巴�� �̵�
        contentParent.anchoredPosition = Vector2.Lerp(
            contentParent.anchoredPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        // �� Ʈ���� �����ϰ� ��ġ ����
        UpdateTrackVisuals();
    }

    void MoveSelection(int direction)
    {
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, trackItems.Length - 1);
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        // ���õ� Ʈ���� ȭ�� �߾ӿ� ������
        RectTransform selected = trackItems[currentIndex];
        targetPosition = -selected.anchoredPosition;
    }

    void UpdateTrackVisuals(bool instant = false)
    {
        for (int i = 0; i < trackItems.Length; i++)
        {
            RectTransform rect = trackItems[i];

            // �߾� ���� �Ÿ� ���
            int distance = Mathf.Abs(i - currentIndex);

            // ũ�� ����
            float targetScale = (i == currentIndex)
                ? selectedScale
                : sideScale;

            // X ��ǥ ������ (�翷���� ��¦ ������ ��ü��)
            float targetX = (i - currentIndex) * sideOffsetX;
            Vector3 targetPosition = new Vector3(targetX, rect.anchoredPosition.y, depthZ);

            // ���� ����
            if (instant)
            {
                rect.localScale = Vector3.one * targetScale;
                rect.localPosition = targetPosition;
            }
            else
            {
                rect.localScale = Vector3.Lerp(
                    rect.localScale,
                    Vector3.one * targetScale,
                    Time.deltaTime * scaleSpeed
                );

                rect.localPosition = Vector3.Lerp(
                    rect.localPosition,
                    targetPosition,
                    Time.deltaTime * moveSpeed
                );
            }
        }
    }
}
