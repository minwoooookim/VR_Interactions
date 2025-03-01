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
    [Header("슬라이더 관련")]
    [SerializeField] private Slider targetSlider;           // 연결된 슬라이더
    [SerializeField] private HandleMode handleMode = HandleMode.Brightness; // 모드 선택
   

    [Header("UI 표시 관련")]
    [SerializeField] private GameObject handleDisplay;        // 클릭/드래그 중 표시할 오브젝트
    [SerializeField] private TextMeshProUGUI displayText;       // 현재 값을 표시할 텍스트

    private void Start()
    {
        if (handleDisplay != null)
            handleDisplay.SetActive(false);

        // 초기 텍스트 업데이트
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
    /// 슬라이더 값을 각 모드에 맞게 환산하여 UI 텍스트에 표시합니다.
    /// - Brightness: 0~1 → 0~10 (정수)
    /// - Audio: 0~1 → 0~30 (정수)
    /// - Time: VideoPlayer의 현재 시간을 mm:ss 형식으로 표시
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
