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
        public VideoPlayer videoPlayer;     // VideoPlayer 컴포넌트
        public Slider videoSlider;          // 재생 위치를 조절할 슬라이더

        [Header("Time Text UI")]
        public TextMeshProUGUI timeText;    // 메인 타임 표시 UI (00:00 / 00:00 등)

        [Header("Handle Display UI")]
        public GameObject handleDisplay;           // 핸들 클릭 시 나타날 오브젝트
        public TextMeshProUGUI handleDisplayText;  // 핸들 위에 표시될 시간 텍스트

        private bool isDragging = false;

        void Start()
        {
            // 동영상 준비가 완료되면 슬라이더 설정
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // 시작 시 핸들 디스플레이 비활성화
            if (handleDisplay != null)
                handleDisplay.SetActive(false);

            // 초기 텍스트 업데이트
            UpdateTimeText(videoSlider.value);
        }

        void Update()
        {
            // 드래그 중이 아니라면 슬라이더 자동 업데이트
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
            // 사용자가 슬라이더를 드래그하는 동안의 처리
            videoPlayer.time = videoSlider.value * videoPlayer.length;
            UpdateTimeText(videoSlider.value);
            UpdateDisplayText(videoSlider.value);
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 0~1 정규화
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
