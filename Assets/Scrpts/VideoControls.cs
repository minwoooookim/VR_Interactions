using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VideoPlayerControlScript
{
    public class VideoControls : MonoBehaviour
    {
        public VideoPlayer videoPlayer; // VideoPlayer 컴포넌트

        public Button playButton; // Play 버튼
        public Sprite playIcon; // 재생 아이콘
        public Sprite pauseIcon; // 일시정지 아이콘
        private Image buttonImage; // 버튼 이미지

        public Slider audioSlider; // 오디오 볼륨 조절 슬라이더
        public Slider brightnessSlider; // 화면 밝기 조절 슬라이더

        public Object screenDimmer; // 화면 어둡게 하는 오브젝트
        private Material screenDimmerMaterial; // 화면 어둡게 하는 오브젝트의 Material

        void Start()
        {
            // 버튼의 이미지 컴포넌트 참조
            buttonImage = playButton.GetComponent<Image>();

            // 버튼에 클릭 이벤트 등록
            playButton.onClick.AddListener(TogglePlayPause);

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
        }

        void TogglePlayPause()
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
        }

        void OnDestroy()
        {
            // 이벤트 등록 해제
            videoPlayer.loopPointReached -= OnVideoEnd;
        }

        void SetAudioVolume(float volume)
        {
            // 오디오 볼륨 설정
            videoPlayer.SetDirectAudioVolume(0, volume);
        }

        void SetBrightness(float brightness)
        {
            // Screen Dimmer의 alpha 값 설정 (brightness 값이 클수록 alpha 값이 작아지도록 반대로 설정)
            Color color = screenDimmerMaterial.color;
            color.a = 1.0f - brightness;
            screenDimmerMaterial.color = color;
        }
    }
}
