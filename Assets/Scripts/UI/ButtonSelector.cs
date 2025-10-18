using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionAsset InputActions;
    private InputAction selectorMoveAction;

    [Header("Button Settings")]
    [SerializeField] private Button[] buttons;

    [Header("Size Settings")]
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float scaleSpeed = 10f;

    private int currentIndex = 0;

    // 읽기 전용 프로퍼티 : 다른 클래스에서 현재 인덱스를 읽을 수만 있음(쓰기 불가능)
    public int CurrentIndex => currentIndex;

    void Awake()
    {
        // Input Actions에서 Selector 액션 가져오기
        if (InputActions != null)
        {
            selectorMoveAction = InputActions.FindAction("UI/Selector");

            if (selectorMoveAction == null)
            {
                Debug.LogWarning("SelectorMove 액션을 찾을 수 없습니다. 기본 키 바인딩을 사용합니다.");
            }
        }
        else
        {
            Debug.LogWarning("InputActionAsset이 할당되지 않았습니다. 기본 키 바인딩을 사용합니다.");
        }
    }

    // New Input System을 위한 함수들(OnEnable, OnDisable)
    // 스크립트 활성화 시, Input Action 활성화하고 이벤트를 구독
    void OnEnable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.Enable();
            selectorMoveAction.performed += OnSelectorMovePerformed;
        }
    }

    // 스크립트 비활성화 시, 이벤트 구독을 해제하고 Input Action 비활성화
    void OnDisable()
    {
        if (selectorMoveAction != null)
        {
            selectorMoveAction.performed -= OnSelectorMovePerformed;
            selectorMoveAction.Disable();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 첫 번째 버튼 선택
        SelectButton(0);
    }

    // Update is called once per frame
    void Update()
    {
        // 부드러운 크기 변경
        UpdateButtonScales();
    }
    private void OnSelectorMovePerformed(InputAction.CallbackContext context)
    {
        // float 값으로 읽기 (1D Axis용)
        // Axis는 위-아래, 왼쪽-오른쪽 등 1차원 움직임을 나타냄
        // 만약 Vector2로 설정하면 위-아래-좌-우 등 2차원 움직임을 나타냄 (0, 1) (0, -1) (-1, 0) (1, 0) 등
        float input = context.ReadValue<float>();

        // input > 0: Positive (아래 방향)
        if (input > 0)
        {
            if (currentIndex < buttons.Length - 1)
            {
                SelectButton(currentIndex + 1);
            }
        }
        // input < 0: Negative (위 방향)
        else if (input < 0)
        {
            if (currentIndex > 0)
            {
                SelectButton(currentIndex - 1);
            }
        }
    }

    void SelectButton(int index)
    {
        currentIndex = index;
    }

    void UpdateButtonScales()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            float targetScale = (i == currentIndex) ? selectedScale : normalScale;
            RectTransform rect = buttons[i].GetComponent<RectTransform>();
            
            // Lerp는 목표에 가까워질수록 변화량이 작아짐
            // 목표값을 절대 넘어가지 않음
            // 목표값에 도달하면 변화가 거의 없어짐
            rect.localScale = Vector3.Lerp(
                rect.localScale,            // 현재
                Vector3.one * targetScale,  // 목표
                Time.deltaTime * scaleSpeed // 비율
            );
        }
    }

    public Button GetSelectedButton()
    {
        return buttons[currentIndex];
    }
}
