using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SceneTransitionManager : MonoBehaviour
{
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
        DontDestroyOnLoad(gameObject);

        if (FadeImage == null)
        {
            CreateFadeCanvas();
        }

        SetAlpha(0f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

