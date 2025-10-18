using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

// 씬 전환 및 페이드 효과 관리 매니저
public class SceneTransitionManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction SubmitAction;

    [Header("Scene Settings")]
    [SerializeField] private string NextSceneName;

    [Header("Fade Settings")]
    [SerializeField] private Image FadeImage;
    [SerializeField] private float FadeDuration = 1f;
    [SerializeField] private Color FadeColor = Color.black;

    public static SceneTransitionManager Instance { get; private set; }

    private bool IsTransitioning = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // scene 전환 시에도 파괴되지 않도록 설정, 싱글톤 패턴 유지
        DontDestroyOnLoad(gameObject);  

        // Input Actions에서 Submit 액션 가져오기
        if (InputActions != null)
        {
            SubmitAction = InputActions.FindAction("UI/Submit");

            if (SubmitAction == null)
            {
                Debug.LogWarning("Submit 액션을 찾을 수 없습니다. 기본 키 바인딩을 사용합니다.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset이 할당되지 않았습니다. 기본 키 바인딩을 사용합니다.");
        }

        if (FadeImage == null)
        {
            CreateFadeCanvas();
        }

        SetAlpha(0f);
    }

    // New Input System을 위한 함수들(OnEnable, OnDisable)
    // 스크립트 활성화 시, Input Action 활성화하고 이벤트를 구독
    void OnEnable()
    {
        if (SubmitAction != null)
        {
            SubmitAction.Enable();
            SubmitAction.performed += OnSubmitPerformed;
        }
    }

    // 스크립트 비활성화 시, 이벤트 구독을 해제하고 Input Action 비활성화
    void OnDisable()
    {
        if (SubmitAction != null)
        {
            SubmitAction.performed -= OnSubmitPerformed;
            SubmitAction.Disable();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        if (!IsTransitioning)
        {
            OnEnterPressed();
        }
    }
    private void OnEnterPressed()
    {
        if(NextSceneName == " ")
        {
            Debug.LogError("다음 씬 이름이 설정되지 않았습니다!");
            return;
        }
        TransitionToScene(NextSceneName);
    }

    // Fade Canvas 자동 생성
    private void CreateFadeCanvas()
    {
        // Canvas 생성
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // 최상위에 표시

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Image 생성
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        FadeImage = imageObj.AddComponent<Image>();
        FadeImage.color = FadeColor;

        // 전체 화면 채우기
        RectTransform rect = FadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // DontDestroyOnLoad 설정
        DontDestroyOnLoad(canvasObj);
    }

    public void TransitionToScene(string sceneName)
    {
        if (!IsTransitioning)
        {
            StartCoroutine(TransitionSequence(sceneName));
        }
    }

    IEnumerator TransitionSequence(string sceneName)
    {
        IsTransitioning = true;

        // 1. 페이드 아웃    // yield return으로 인해 첫 번째 루프에서 반환되는 값
        yield return StartCoroutine(FadeOut());

        // 3. 씬 로드  // yield return으로 인해 두 번째 루프에서 반환되는 값
        yield return StartCoroutine(LoadSceneAsync(sceneName));

        // 4. 페이드 인 (새 씬에서) // yield return으로 인해 세 번째 루프에서 반환되는 값
        yield return StartCoroutine(FadeIn());

        IsTransitioning = false;
    }

    // 페이드 아웃: 투명 -> 불투명

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / FadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f); // 완전히 불투명
    }

    // 페이드 인: 불투명 -> 투명
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / FadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f); // 완전히 투명
    }

    // 알파값 설정 헬퍼 메서드
    private void SetAlpha(float alpha)
    {
        if (FadeImage != null)
        {
            Color color = FadeImage.color;
            color.a = alpha;
            FadeImage.color = color;
        }
    }

    // 비동기 로딩 처리
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

