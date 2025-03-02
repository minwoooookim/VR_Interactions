using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;

namespace VideoPlayerControlScript
{
    public class VideoPlayerSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private VideoPlayerControls v;
        public Slider videoSlider;

        public void OnPointerDown(PointerEventData eventData)
        {
            v.isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            v.isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //v.videoPlayer.time = videoSlider.value * v.videoPlayer.length;
        }

    }
}
