using System.Collections;
using System.Collections.Generic;
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
    }
}
