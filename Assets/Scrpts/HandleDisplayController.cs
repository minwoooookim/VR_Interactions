using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Video;
using Oculus.Interaction;

public enum HandleMode
{
    Brightness,
    Audio
}

public class HandleDisplayController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("�����̴� ����")]
    [SerializeField] private Slider targetSlider;           // ����� �����̴�
    [SerializeField] private HandleMode handleMode = HandleMode.Brightness; // ��� ����
   

    [Header("UI ǥ�� ����")]
    [SerializeField] private GameObject handleDisplay;        // Ŭ��/�巡�� �� ǥ���� ������Ʈ
    [SerializeField] private TextMeshProUGUI displayText;       // ���� ���� ǥ���� �ؽ�Ʈ

    private void Start()
    {
        if (handleDisplay != null)
            handleDisplay.SetActive(false);

        // �ʱ� �ؽ�Ʈ ������Ʈ
        UpdateDisplayText(targetSlider.value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (handleDisplay != null)
            handleDisplay.SetActive(true);

        UpdateDisplayText(targetSlider.value);
        GrabbableKnob.instance._currentValue = targetSlider.value;
        GrabbableKnob.instance.UpdateRotationFromValue();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handleDisplay != null)
            handleDisplay.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDisplayText(targetSlider.value);
    }

    /// <summary>
    /// �����̴� ���� �� ��忡 �°� ȯ���Ͽ� UI �ؽ�Ʈ�� ǥ���մϴ�.
    /// - Brightness: 0~1 �� 0~10 (����)
    /// - Audio: 0~1 �� 0~30 (����)
    /// - Time: VideoPlayer�� ���� �ð��� mm:ss �������� ǥ��
    /// </summary>
    private void UpdateDisplayText(float sliderValue)
    {
        switch (handleMode)
        {
            case HandleMode.Brightness:
                int brightnessValue = Mathf.RoundToInt(sliderValue * 10f);
                if (displayText != null)
                    displayText.text = brightnessValue.ToString();
                break;

            case HandleMode.Audio:
                int audioValue = Mathf.RoundToInt(sliderValue * 30f);
                if (displayText != null)
                    displayText.text = audioValue.ToString();
                break;

        }
    }
}
