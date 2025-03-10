using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Oculus.Interaction;

namespace VideoPlayerControlScript
{
    public class VideoPlayerControls : MonoBehaviour
    {
        public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트
        public TextMeshProUGUI timeText;    // 메인 타임 표시 UI (00:00 / 00:00 등)
        public TextMeshProUGUI videoHandleTimeText; // 핸들 타임 표시 UI

        [Header("Play Button")]
        public GameObject playButton; // Play 버튼
        public Sprite playIcon; // 재생 아이콘
        public Sprite pauseIcon; // 일시정지 아이콘
        private Image buttonImage; // 버튼 이미지

        [Header("Sliders")]
        public Slider videoSlider; // 재생 위치를 조절할 슬라이더
        public Slider audioSlider; // 오디오 볼륨 조절 슬라이더
        public Slider brightnessSlider; // 화면 밝기 조절 슬라이더

        [Header("HandleDisplaySelectors")]
        public HandleDisplaySelector videoHandleDisplaySelector;
        public HandleDisplaySelector audioHandleDisplaySelector;
        public HandleDisplaySelector brightnessHandleDisplaySelector;

        [Header("Knobs")]
        public GrabbableKnob videoKnob;
        public GrabbableKnob audioKnob;
        public GrabbableKnob brightnessKnob;

        public Object screenDimmer; // 화면 어둡게 하는 오브젝트
        private Material screenDimmerMaterial; // 화면 어둡게 하는 오브젝트의 Material

        public bool isDragging = false;

        void Start()
        {
            // 동영상 준비가 완료되면 슬라이더 설정
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // 버튼의 이미지 컴포넌트 참조
            buttonImage = playButton.GetComponent<Image>();

            // 초기 상태 설정
            UpdateButtonIcon();

            // VideoPlayer의 끝 이벤트 등록
            videoPlayer.loopPointReached += OnVideoEnd;

            // 비디오의 초기 음량을 현재 오디오 슬라이더의 값으로 설정
            videoPlayer.SetDirectAudioVolume(0, audioSlider.value);

            // Screen Dimmer의 Material 참조
            screenDimmerMaterial = screenDimmer.GetComponent<Renderer>().material;

            // 슬라이더 값에 따라 초기 Screen Dimmer의 alpha 값 설정
            SetBrightness(brightnessSlider.value);

            // 오디오 슬라이더 이벤트 등록
            audioSlider.onValueChanged.AddListener(SetAudioVolume);

            // 밝기 슬라이더 이벤트 등록
            brightnessSlider.onValueChanged.AddListener(SetBrightness);

            videoKnob.SetKnobValue(videoSlider.value);
            audioKnob.SetKnobValue(audioSlider.value);
            brightnessKnob.SetKnobValue(brightnessSlider.value);

            videoPlayer.Play();
            videoPlayer.Pause();
        }

        void Update()
        {
            UpdateTimeText(videoSlider.value);

            // 드래그 중이 아니고 비디오가 재생중이라면 슬라이더 자동 업데이트
            if (!isDragging)
            {
                UpdateVideoSliderValue();
            }
            else if (isDragging)
            {
                SetVideoTime();
            }
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // 0~1 정규화
            UpdateTimeText(videoSlider.value);
        }

        private void UpdateVideoSliderValue()
        {
            if (videoPlayer.length > 0)
            {
                videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
                videoKnob.SetKnobValue(videoSlider.value);
            }
            else
            {
                videoSlider.value = 0;
            }
        }

        public void UpdateTimeText(float sliderValue)
        {
            if (timeText == null) return;

            string currentTimeString = videoHandleTimeText.text; // 비디오의 HandleDisplay를 가져와서 현재 시간으로 표시, 이렇게 함으로써 핸들과 시간을 동기화
            string totalTimeString = FormatTime(videoPlayer.length);

            timeText.text = $"{currentTimeString} / {totalTimeString}";
        }


        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60f);
            int seconds = Mathf.FloorToInt((float)time % 60f);
            return $"{minutes:D1}:{seconds:D2}";
        }

        public void TogglePlayPause()
        {
            // 영상 재생 상태 토글
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }

            // 버튼 아이콘 업데이트
            UpdateButtonIcon();
        }

        void UpdateButtonIcon()
        {
            // 재생 상태에 따라 아이콘 변경
            if (videoPlayer.isPlaying)
            {
                buttonImage.sprite = pauseIcon;
            }
            else
            {
                buttonImage.sprite = playIcon;
            }
        }

        void OnVideoEnd(VideoPlayer vp)
        {
            // 영상이 끝났을 때 아이콘을 재생 아이콘으로 변경
            buttonImage.sprite = playIcon;
            videoPlayer.Play();
            videoPlayer.Pause();
        }

        void OnDestroy()
        {
            // 이벤트 등록 해제
            videoPlayer.loopPointReached -= OnVideoEnd;
        }

        void SetVideoTime()
        {
            // 슬라이더 값에 따라 비디오 시간 설정
            videoPlayer.time = videoSlider.value * videoPlayer.length;
            videoKnob.SetKnobValue(videoSlider.value);
        }

        void SetAudioVolume(float volume)
        {
            // 오디오 볼륨 설정
            videoPlayer.SetDirectAudioVolume(0, volume);
        }

        void SetBrightness(float brightness)
        {
            float mappedBrightness = Mathf.Lerp(0.2f, 1.0f, brightness);
 
            // Screen Dimmer의 alpha 값 설정 (brightness 값이 클수록 alpha 값이 작아지도록 반대로 설정)
            Color color = screenDimmerMaterial.color;
            color.a = 1.0f - mappedBrightness;
            screenDimmerMaterial.color = color;
        }

        public void ResetPlayer()
        {
            videoPlayer.Pause();
            videoPlayer.time = 0;
            videoSlider.value = 0;
            audioSlider.value = 0.5f;
            brightnessSlider.value = 0.5f;
            videoHandleDisplaySelector.UpdateDisplayText(videoSlider.value);
            audioHandleDisplaySelector.UpdateDisplayText(audioSlider.value);
            brightnessHandleDisplaySelector.UpdateDisplayText(brightnessSlider.value);
        }
    }
}
