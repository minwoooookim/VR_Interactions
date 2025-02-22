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
        [Header("Video Components")]
        public VideoPlayer videoPlayer;     // VideoPlayer ������Ʈ
        public Slider videoSlider;          // ��� ��ġ�� ������ �����̴�

        [Header("Time Text UI")]
        public TextMeshProUGUI timeText;    // ���� Ÿ�� ǥ�� UI (00:00 / 00:00 ��)

        [Header("Handle Display UI")]
        public GameObject handleDisplay;           // �ڵ� Ŭ�� �� ��Ÿ�� ������Ʈ
        public TextMeshProUGUI handleDisplayText;  // �ڵ� ���� ǥ�õ� �ð� �ؽ�Ʈ

        private bool isDragging = false;

        void Start()
        {
            // ������ �غ� �Ϸ�Ǹ� �����̴� ����
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // ���� �� �ڵ� ���÷��� ��Ȱ��ȭ
            if (handleDisplay != null)
                handleDisplay.SetActive(false);

            // �ʱ� �ؽ�Ʈ ������Ʈ
            UpdateTimeText(videoSlider.value);
        }

        void Update()
        {
            // �巡�� ���� �ƴ϶�� �����̴� �ڵ� ������Ʈ
            if (!isDragging)
            {
                UpdateSliderValue();
                UpdateTimeText(videoSlider.value);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            if (handleDisplay != null)
                handleDisplay.SetActive(true);
            UpdateDisplayText(videoSlider.value);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            if (handleDisplay != null)
                handleDisplay.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // ����ڰ� �����̴��� �巡���ϴ� ������ ó��
            videoPlayer.time = videoSlider.value * videoPlayer.length;
            UpdateTimeText(videoSlider.value);
            UpdateDisplayText(videoSlider.value);
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 0~1 ����ȭ
            UpdateTimeText(videoSlider.value);
        }

        private void UpdateSliderValue()
        {
            if (videoPlayer.length > 0)
            {
                videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
            }
            else
            {
                videoSlider.value = 0;
            }
        }

        private void UpdateTimeText(float sliderValue)
        {
            if (timeText == null) return;

            double currentTime = sliderValue * videoPlayer.length;
            double totalTime = videoPlayer.length;

            string currentTimeString = FormatTime(currentTime);
            string totalTimeString = FormatTime(totalTime);

            timeText.text = $"{currentTimeString} / {totalTimeString}";
        }

        private void UpdateDisplayText(float sliderValue)
        {
            if (handleDisplay != null && handleDisplay.activeSelf && handleDisplayText != null)
            {
                double currentTime = sliderValue * videoPlayer.length;
                handleDisplayText.text = FormatTime(currentTime);
            }
        }

        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60f);
            int seconds = Mathf.FloorToInt((float)time % 60f);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
