using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

// �� ��ȯ �� ���̵� ȿ�� ���� �Ŵ���
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // scene ��ȯ �ÿ��� �ı����� �ʵ��� ����, �̱��� ���� ����
        DontDestroyOnLoad(gameObject);  

        if (fadeImage == null)
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

        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = fadeColor;

        // ��ü ȭ�� ä���
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // DontDestroyOnLoad ����
        DontDestroyOnLoad(canvasObj);
    }

    public void TransitionToScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionSequence(sceneName));
        }
    }

    IEnumerator TransitionSequence(string sceneName)
    {
        isTransitioning = true;

        // 1. ���̵� �ƿ�    // yield return���� ���� ù ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(FadeOut());

        // 3. �� �ε�  // yield return���� ���� �� ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(LoadSceneAsync(sceneName));

        // 4. ���̵� �� (�� ������) // yield return���� ���� �� ��° �������� ��ȯ�Ǵ� ��
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    // ���̵� �ƿ�: ���� -> ������

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f); // ������ ������
    }

    // ���̵� ��: ������ -> ����
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f); // ������ ����
    }

    // ���İ� ���� ���� �޼���
    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
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

