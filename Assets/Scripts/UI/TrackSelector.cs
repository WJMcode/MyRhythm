using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackSelector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SelectorInput selectorInput;
    public event Action OnTrackConfirmed; // 엔터 입력 알림

    [Header("Track List Settings")]
    [SerializeField] public TrackData[] tracks;   // 각 트랙의 정보
    [SerializeField] private RectTransform contentParent;  // 모든 트랙의 부모
    [SerializeField] private float moveSpeed = 10f;        // 중앙 이동 속도
    [SerializeField] private float scaleSpeed = 8f;        // 스케일 보간 속도

    [Header("Scale Settings")]
    [SerializeField] private float selectedScale = 1.2f;   // 중앙 트랙 크기
    [SerializeField] private float sideScale = 0.9f;       // 양 옆 트랙 크기

    [Header("Position Offset Settings")]
    [SerializeField] private float sideOffsetX = 250f;     // 양 옆으로 얼마나 벌어질지
    [SerializeField] private float depthZ = 0f;            // 필요 시 깊이감 표현용

    private int currentIndex = 0;
    private Vector2 targetPosition;

    // RectTransform 배열은 내부적으로 생성
    private RectTransform[] trackItems;

    // @WJM : 질문 (이 부분의 용도?)
    void Awake()
    {
        // TrackData에서 RectTransform 추출
        trackItems = new RectTransform[tracks.Length];
        for (int i = 0; i < tracks.Length; i++)
        {
            // 각 TrackData에 rectTransform 필드가 있다고 가정
            // 또는 자식 오브젝트를 사용
        }
    }

    void Start()
    {
        UpdateTargetPosition();
        UpdateTrackVisuals(true); // 초기 설정 즉시 반영
    }

    void OnDisable()
    {
        if (selectorInput != null)
        {
            selectorInput.OnMove += MoveSelection;
            selectorInput.OnConfirm += ConfirmSelection;
        }
    }

    void OnDestroy()
    {
        if (selectorInput != null)
        {
            selectorInput.OnMove -= MoveSelection;
            selectorInput.OnConfirm += ConfirmSelection;
        }

    }
    void ConfirmSelection()
    {
        // 선택한 트랙 정보 저장
        if (GameData.Instance != null)
        {
            GameData.Instance.selectedTrack = tracks[currentIndex];
        }

        // TrackSelectSceneManager에게 엔터 입력 알림
        OnTrackConfirmed?.Invoke();
    }

    void Update()
    {
        // 선택된 트랙이 중앙에 오도록 부드럽게 이동
        contentParent.anchoredPosition = Vector2.Lerp(
            contentParent.anchoredPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        // 각 트랙의 스케일과 위치 보간
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
        // 선택된 트랙이 화면 중앙에 오도록
        RectTransform selected = trackItems[currentIndex];
        targetPosition = -selected.anchoredPosition;
    }

    void UpdateTrackVisuals(bool instant = false)
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            RectTransform rect = trackItems[currentIndex];

            // 중앙 기준 거리 계산
            int distance = Mathf.Abs(i - currentIndex);

            // 크기 결정
            float targetScale = (i == currentIndex)
                ? selectedScale
                : sideScale;

            // X 좌표 오프셋 (양옆으로 살짝 벌려서 입체감)
            float targetX = (i - currentIndex) * sideOffsetX;
            Vector3 targetPosition = new Vector3(targetX, rect.anchoredPosition.y, depthZ);

            // 보간 적용
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
