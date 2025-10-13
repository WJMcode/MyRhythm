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

    // Fade Canvas �ڵ� ����
    private void CreateFadeCanvas()
    {
        // Canvas ����
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // �ֻ����� ǥ��

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        // Image ����
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);

        FadeImage = imageObj.AddComponent<Image>();
        FadeImage.color = FadeColor;

        // ��ü ȭ�� ä���
        RectTransform rect = FadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // DontDestroyOnLoad ����
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

        // 1. ���̵� �ƿ�    // yield return���� ���� ù ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(FadeOut());

        // 3. �� �ε�  // yield return���� ���� �� ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(LoadSceneAsync(sceneName));

        // 4. ���̵� �� (�� ������) // yield return���� ���� �� ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(FadeIn());

        IsTransitioning = false;
    }

    // ���̵� �ƿ�: ���� -> ������

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

        SetAlpha(1f); // ������ ������
    }

    // ���̵� ��: ������ -> ����
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

        SetAlpha(0f); // ������ ����
    }

    // ���İ� ���� ���� �޼���
    private void SetAlpha(float alpha)
    {
        if (FadeImage != null)
        {
            Color color = FadeImage.color;
            color.a = alpha;
            FadeImage.color = color;
        }
    }

    // �񵿱� �ε� ó��
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

