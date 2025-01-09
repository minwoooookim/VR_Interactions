using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace VideoPlayerControlScript
{
    public class VideoControls : MonoBehaviour
    {
        public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
        public Button playButton; // Play ��ư
        public Sprite playIcon; // ��� ������
        public Sprite pauseIcon; // �Ͻ����� ������
        private Image buttonImage; // ��ư �̹���

        void Start()
        {
            // ��ư�� �̹��� ������Ʈ ����
            buttonImage = playButton.GetComponent<Image>();

            // ��ư�� Ŭ�� �̺�Ʈ ���
            playButton.onClick.AddListener(TogglePlayPause);

            // �ʱ� ���� ����
            UpdateButtonIcon();

            // VideoPlayer�� �� �̺�Ʈ ���
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        void TogglePlayPause()
        {
            // ���� ��� ���� ���
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }

            // ��ư ������ ������Ʈ
            UpdateButtonIcon();
        }

        void UpdateButtonIcon()
        {
            // ��� ���¿� ���� ������ ����
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
            // ������ ������ �� �������� ��� ���������� ����
            buttonImage.sprite = playIcon;
        }

        void OnDestroy()
        {
            // �̺�Ʈ ��� ����
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
