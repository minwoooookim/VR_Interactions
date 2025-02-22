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
            UpdateSliderValue();
            UpdateTimeText(videoSlider.value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (handleDisplay != null)
                handleDisplay.SetActive(true);

            UpdateDisplayText(videoSlider.value);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (handleDisplay != null)
                handleDisplay.SetActive(false);
        }

        /// <summary>
        /// �����̴� ���� ����� ������ ȣ�� (�巡�� �߿��� ȣ��)
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            // �����̴� ���� ���� ��� �ð��� �ݿ� (0~1 ����)
            videoPlayer.time = videoSlider.value * videoPlayer.length;

            // ���� Ÿ�� �ؽ�Ʈ ����
            UpdateTimeText(videoSlider.value);

            // �ڵ� ���÷��� �ؽ�Ʈ ����
            UpdateDisplayText(videoSlider.value);
        }

        /// <summary>
        /// ������ �غ�Ǹ� �����̴� ����
        /// </summary>
        private void OnVideoPrepared(VideoPlayer vp)
        {
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 0~1 ����ȭ
            UpdateTimeText(videoSlider.value);
        }

        /// <summary>
        /// ��� ���� ���� �ð��� ���� �����̴� ���� ������Ʈ
        /// </summary>
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

        /// <summary>
        /// ���� Ÿ�� �ؽ�Ʈ (00:00 / 00:00) ������Ʈ
        /// </summary>
        private void UpdateTimeText(float sliderValue)
        {
            if (timeText == null) return;

            double currentTime = sliderValue * videoPlayer.length;
            double totalTime = videoPlayer.length;

            string currentTimeString = FormatTime(currentTime);
            string totalTimeString = FormatTime(totalTime);

            timeText.text = $"{currentTimeString} / {totalTimeString}";
        }

        /// <summary>
        /// �ڵ� ���÷��� �ؽ�Ʈ ����
        /// </summary>
        private void UpdateDisplayText(float sliderValue)
        {
            if (handleDisplay != null && handleDisplay.activeSelf && handleDisplayText != null)
            {
                double currentTime = sliderValue * videoPlayer.length;
                handleDisplayText.text = FormatTime(currentTime);
            }
        }

        /// <summary>
        /// �� ������ �ð��� mm:ss �������� ��ȯ
        /// </summary>
        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60f);
            int seconds = Mathf.FloorToInt((float)time % 60f);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
