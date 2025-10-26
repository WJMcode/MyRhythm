using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 화면에 표시되는 버튼들 중, 방향키를 통해 버튼을 선택할 수 있음.
// 그리고 선택된 버튼의 크기를 조절하는 스크립트
public class ButtonSelector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SelectorInput selectorInput;

    [Header("Button Settings")]
    [SerializeField] private Button[] buttons;

    [Header("Size Settings")]
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float normalScale = 1.0f;
    [SerializeField] private float scaleSpeed = 10f;

    private int currentIndex = 0;

    // 읽기 전용 프로퍼티 : 다른 클래스에서 현재 인덱스를 읽을 수만 있음(쓰기 불가능)
    public int CurrentIndex => currentIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (selectorInput != null)
            selectorInput.OnMove += MoveSelection;

        // 첫 번째 버튼 선택
        SelectButton(0);
    }
    void OnDisable()
    {
        if (selectorInput != null)
            selectorInput.OnMove -= MoveSelection;
    }

    // Update is called once per frame
    void Update()
    {
        // 부드러운 크기 변경
        UpdateButtonScales();
    }
    void MoveSelection(int direction)
    {
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, buttons.Length - 1);
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
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
