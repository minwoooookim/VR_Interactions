using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;

namespace VideoPlayerControlScript
{
    public class VideoControlSlider : MonoBehaviour
        //, IPointerDownHandler, IPointerUpHandler
    {
        public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트
        public Slider videoSlider;     // 재생 위치를 조절할 슬라이더
        public TextMeshProUGUI timeText;          // 현재 재생 시간을 표시할 텍스트

        private bool isDragging = false;

        void Start()
        {
            // 슬라이더의 최대값을 비디오 길이에 맞게 설정
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // 슬라이더 이벤트 리스너 추가
            videoSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        }

        void Update()
        {
            if (!isDragging && videoPlayer.isPlaying)
            {
                // 현재 재생 시간을 슬라이더와 텍스트에 업데이트
                videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
                UpdateTimeText();
            }
        }

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    // 슬라이더 클릭 시작
        //    isDragging = true;
        //    videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        //}

        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    // 슬라이더 클릭 종료
        //    isDragging = false;
        //    // 슬라이더 값에 따라 비디오 재생 위치 설정
        //    videoPlayer.time = videoSlider.value * videoPlayer.length;
        //}

        public void OnSliderValueChanged()
        {
            if (true
                //isDragging
                )
            {
                // 슬라이더 값에 따라 비디오 재생 위치 설정
                videoPlayer.time = videoSlider.value * videoPlayer.length;
            }
        }

        public void OnSliderDragStart()
        {
            isDragging = true;
        }

        public void OnSliderDragEnd()
        {
            isDragging = false;
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            // 슬라이더 초기값 및 텍스트 초기화
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 슬라이더 값은 0~1로 정규화됨
            UpdateTimeText();
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
