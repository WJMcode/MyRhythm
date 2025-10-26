using System;
using UnityEngine;

public class TrackSelector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SelectorInput selectorInput;
    public event Action OnTrackConfirmed; // ���� �Է� �˸�

    [Header("Track List Settings")]
    [SerializeField] public TrackData[] tracks;   // �� Ʈ���� ����
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

    // RectTransform �迭�� ���������� ����
    private RectTransform[] trackItems;

    // Ʈ�� UI�� RectTransform ������ �迭�� ����
    void Awake()
    {
        // TrackData���� RectTransform ����
        trackItems = new RectTransform[tracks.Length];
        for (int i = 0; i < tracks.Length; i++)
        {
            trackItems[i] = tracks[i].rectTransform;
        }
    }

    void Start()
    {
        UpdateTargetPosition();
        UpdateTrackVisuals(true); // �ʱ� ���� ��� �ݿ�
    }

    void OnEnable()
    {
        if (selectorInput != null)
        {
            selectorInput.OnMove += MoveSelection;
            selectorInput.OnConfirm += ConfirmSelection;
        }
    }

    void OnDisable()
    {
        if (selectorInput != null)
        {
            selectorInput.OnMove -= MoveSelection;
            selectorInput.OnConfirm -= ConfirmSelection;
        }

    }
    void ConfirmSelection()
    {
        // ������ Ʈ�� ���� ����
        if (GameData.Instance != null)
        {
            GameData.Instance.selectedTrack = tracks[currentIndex];
        }

        // TrackSelectSceneManager���� ���� �Է� �˸�
        OnTrackConfirmed?.Invoke();
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
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, tracks.Length - 1);
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
            float targetScale = (i == currentIndex) ? selectedScale : sideScale;
            float targetX = (i - currentIndex) * sideOffsetX;
            Vector3 targetPos = new Vector3(targetX, rect.anchoredPosition.y, depthZ);

            if (instant)
            {
                rect.localScale = Vector3.one * targetScale;
                rect.localPosition = targetPos;
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
                    targetPos,
                    Time.deltaTime * moveSpeed
                );
            }
        }
    }
}
