using Oculus.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VideoplayerTesterNamespace;

public class SliderEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Slider thisSlider;
    [SerializeField] private GrabbableKnob targetKnob;
    [SerializeField] private GameObject handleDisplay;  
    [SerializeField] private HandleDisplaySelector handleDisplaySelector;
    [SerializeField] private VideoplayerTester videoTester;

    private void Start()
    {
        //handleDisplay.SetActive(false);
        handleDisplay.SetActive(true);
        handleDisplaySelector.UpdateDisplayText(thisSlider.value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //handleDisplay.SetActive(true);
        handleDisplaySelector.UpdateDisplayText(thisSlider.value);
        targetKnob.SetKnobValue(thisSlider.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //handleDisplay.SetActive(false);
        videoTester.CheckIsCompleted();
    }

    public void OnDrag(PointerEventData eventData)
    {
        handleDisplaySelector.UpdateDisplayText(thisSlider.value);
        targetKnob.SetKnobValue(thisSlider.value);
    }
}
