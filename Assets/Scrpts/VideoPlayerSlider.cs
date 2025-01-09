using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;

namespace VideoPlayerControlScript
{
    public class VideoPlayerSlider : MonoBehaviour
    {
        public VideoPlayer videoPlayer;     // VideoPlayer 컴포넌트
        public Slider videoSlider;          // 재생 위치를 조절할 슬라이더
        public TextMeshProUGUI timeText;    // 현재 재생 시간을 표시할 텍스트

        private bool isDragging = false;

        void Start()
        {
            // 슬라이더의 최대값을 비디오 길이에 맞게 설정
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
            videoSlider.onValueChanged.AddListener(OnSliderValueChanged);

            // EventTrigger를 사용하여 슬라이더의 클릭 이벤트 처리
            EventTrigger trigger = videoSlider.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryDown = new EventTrigger.Entry();
            entryDown.eventID = EventTriggerType.PointerDown;
            entryDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(entryDown);

            EventTrigger.Entry entryUp = new EventTrigger.Entry();
            entryUp.eventID = EventTriggerType.PointerUp;
            entryUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            trigger.triggers.Add(entryUp);
        }

        void Update()
        {
            if (!isDragging && videoPlayer.isPlaying)
            {
                UpdateSliderValue();
                UpdateTimeText();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            //videoPlayer.time = videoSlider.value * videoPlayer.length;
            //UpdateTimeText();
        }

        private void OnSliderValueChanged(float value)
        {
            if (isDragging)
            {
                videoPlayer.time = value * videoPlayer.length;
                UpdateTimeText();
            }
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            // 슬라이더 초기값 및 텍스트 초기화
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 슬라이더 값은 0~1로 정규화됨
            UpdateTimeText();
        }

        private void UpdateSliderValue()
        {
            // 현재 재생 시간을 슬라이더와 텍스트에 업데이트
            videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        }

        private void UpdateTimeText()
        {
            // 현재 재생 시간 및 총 시간 계산
            double currentTime = videoPlayer.time;
            double totalTime = videoPlayer.length;

            // 텍스트 형식: "현재시간 / 총시간 (분:초)"
            string currentTimeString = FormatTime(currentTime);
            string totalTimeString = FormatTime(totalTime);

            timeText.text = $"{currentTimeString} / {totalTimeString}";
        }

        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60);
            int seconds = Mathf.FloorToInt((float)time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
