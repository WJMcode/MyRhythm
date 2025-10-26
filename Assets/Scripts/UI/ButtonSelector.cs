using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ȭ�鿡 ǥ�õǴ� ��ư�� ��, ����Ű�� ���� ��ư�� ������ �� ����.
// �׸��� ���õ� ��ư�� ũ�⸦ �����ϴ� ��ũ��Ʈ
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

    // �б� ���� ������Ƽ : �ٸ� Ŭ�������� ���� �ε����� ���� ���� ����(���� �Ұ���)
    public int CurrentIndex => currentIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (selectorInput != null)
            selectorInput.OnMove += MoveSelection;

        // ù ��° ��ư ����
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
        // �ε巯�� ũ�� ����
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
            
            // Lerp�� ��ǥ�� ����������� ��ȭ���� �۾���
            // ��ǥ���� ���� �Ѿ�� ����
            // ��ǥ���� �����ϸ� ��ȭ�� ���� ������
            rect.localScale = Vector3.Lerp(
                rect.localScale,            // ����
                Vector3.one * targetScale,  // ��ǥ
                Time.deltaTime * scaleSpeed // ����
            );
        }
    }

    public Button GetSelectedButton()
    {
        return buttons[currentIndex];
    }
}
