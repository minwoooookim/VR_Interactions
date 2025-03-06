using UnityEngine;
using Oculus.Interaction; // Meta(�� Oculus) Interaction SDK ���ӽ����̽�

public class KeyboardToggleButton : MonoBehaviour
{
    [Header("Keyboard Objects")]
    [SerializeField] private GameObject QWERTYKeyboard;
    [SerializeField] private GameObject WristKeyboard;

    [Header("Raycast Objects")]
    [SerializeField] private GameObject leftRayCaster;
    [SerializeField] private GameObject rightRayCaster;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    // WristKeyboard ���� ǥ�� ����
    private bool isWristKeyboardActive = false;

    // ================================
    //  ��ư ���� ��ȯ�� ���� �ʵ� �߰�
    // ================================
    [Header("Button Color Settings")]
    [Tooltip("��ư�� MeshRenderer �Ǵ� SpriteRenderer(2D UI) ���� �Ҵ�")]
    [SerializeField] private Renderer buttonRenderer;

    [Tooltip("WristKeyboard�� �������� ��(�⺻) ��ư ����")]
    [SerializeField] private Color colorKeyboardOff = Color.gray;

    [Tooltip("WristKeyboard�� �������� �� ��ư ����")]
    [SerializeField] private Color colorKeyboardOn = Color.green;

    private void Awake()
    {
        // PokeInteractable�� �Ҵ�Ǿ����� Ȯ�� �� �̺�Ʈ ���
        if (pokeInteractable != null)
        {
            // ��ư(��ŷ) ���°� �ٲ� ������ �ݹ� ����
            pokeInteractable.WhenStateChanged += HandleStateChanged;
        }
        else
        {
            Debug.LogWarning("PokeInteractable�� �Ҵ���� �ʾҽ��ϴ�.", this);
        }

        // �� ���� �� �ʱ� ���� ����
        UpdateButtonColor();
    }

    private void OnDestroy()
    {
        // ���� ��ε�ǰų� ������Ʈ �ı� �� �̺�Ʈ ����
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    /// <summary>
    /// PokeInteractable ���°� ���� �� ȣ��Ǵ� �ݹ�.
    /// InteractableState.Select�� �ٲ�� '��ư�� ���ȴ�'�� �����Ѵ�.
    /// </summary>
    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // �� ���°� Select(= ����)�� ���� ��� ���� ����
        if (args.NewState == InteractableState.Select)
        {
            ToggleKeyboards();
        }
    }

    /// <summary>
    /// Ű���� ��ȯ �� RayCaster Ȱ��/��Ȱ�� ����
    /// </summary>
    private void ToggleKeyboards()
    {
        // WristKeyboard Ȱ�� ���¸� ���
        isWristKeyboardActive = !isWristKeyboardActive;
        WristKeyboard.SetActive(isWristKeyboardActive);

        // QWERTYKeyboard�� �ݴ� ���·�
        QWERTYKeyboard.SetActive(!isWristKeyboardActive);

        // WristKeyboard�� ���� ������ ��� RayCaster ��Ȱ��ȭ
        bool shouldEnableRays = !isWristKeyboardActive;
        leftRayCaster.SetActive(shouldEnableRays);
        rightRayCaster.SetActive(shouldEnableRays);

        // ��ư ���� ������Ʈ
        UpdateButtonColor();
    }

    /// <summary>
    /// ��ư ������ ���� Ű���� ���¿� ���� ����
    /// </summary>
    private void UpdateButtonColor()
    {
        if (buttonRenderer == null) return;

        // WristKeyboard�� Ȱ�� ������ ���� colorKeyboardOn, �ƴ� ���� colorKeyboardOff
        buttonRenderer.material.color = isWristKeyboardActive ? colorKeyboardOn : colorKeyboardOff;
    }
}
