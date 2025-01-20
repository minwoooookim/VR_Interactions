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
        public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ

        public Button playButton; // Play ��ư
        public Sprite playIcon; // ��� ������
        public Sprite pauseIcon; // �Ͻ����� ������
        private Image buttonImage; // ��ư �̹���

        public Slider audioSlider; // ����� ���� ���� �����̴�
        public Slider brightnessSlider; // ȭ�� ��� ���� �����̴�

        public Object screenDimmer; // ȭ�� ��Ӱ� �ϴ� ������Ʈ
        private Material screenDimmerMaterial; // ȭ�� ��Ӱ� �ϴ� ������Ʈ�� Material

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

            // ������ �ʱ� ������ ���� ����� �����̴��� ������ ����
            videoPlayer.SetDirectAudioVolume(0, audioSlider.value);

            // Screen Dimmer�� Material ����
            screenDimmerMaterial = screenDimmer.GetComponent<Renderer>().material;

            // �����̴� ���� ���� �ʱ� Screen Dimmer�� alpha �� ����
            SetBrightness(brightnessSlider.value);

            // ����� �����̴� �̺�Ʈ ���
            audioSlider.onValueChanged.AddListener(SetAudioVolume);

            // ��� �����̴� �̺�Ʈ ���
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
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

        void SetAudioVolume(float volume)
        {
            // ����� ���� ����
            videoPlayer.SetDirectAudioVolume(0, volume);
        }

        void SetBrightness(float brightness)
        {
            // Screen Dimmer�� alpha �� ���� (brightness ���� Ŭ���� alpha ���� �۾������� �ݴ�� ����)
            Color color = screenDimmerMaterial.color;
            color.a = 1.0f - brightness;
            screenDimmerMaterial.color = color;
        }
    }
}
