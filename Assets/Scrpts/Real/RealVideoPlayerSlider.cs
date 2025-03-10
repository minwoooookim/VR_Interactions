using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;

namespace VideoPlayerControlScript
{
    public class RealVideoPlayerSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private RealVideoPlayerControls videoPlayerControls;
        public Slider videoSlider;

        public void OnPointerDown(PointerEventData eventData)
        {
            videoPlayerControls.isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            videoPlayerControls.isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

    }
}
