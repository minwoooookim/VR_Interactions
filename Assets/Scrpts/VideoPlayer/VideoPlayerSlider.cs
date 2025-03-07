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
        [SerializeField] private VideoPlayerControls videoPlayerControls;
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
            //v.videoPlayer.time = videoSlider.value * v.videoPlayer.length;
        }

    }
}
