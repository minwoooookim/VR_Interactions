using Oculus.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Slider targetSlider;
    [SerializeField] private GrabbableKnob targetKnob;
    [SerializeField] private GameObject handleDisplay;  
    [SerializeField] private HandleDisplaySelector handleDisplaySelector;

    private void Start()
    {
        handleDisplay.SetActive(false);

        // 초기 텍스트 업데이트
        handleDisplaySelector.UpdateDisplayText(targetSlider.value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handleDisplay.SetActive(true);
        handleDisplaySelector.UpdateDisplayText(targetSlider.value);
        targetKnob.SetKnobValue(targetSlider.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleDisplay.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        handleDisplaySelector.UpdateDisplayText(targetSlider.value);
        targetKnob.SetKnobValue(targetSlider.value);
    }



}
