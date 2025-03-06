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
        public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
        public TextMeshProUGUI timeText;    // ���� Ÿ�� ǥ�� UI (00:00 / 00:00 ��)
        public GrabbableKnob targetKnob; // ������ ���

        [Header("Play Button")]
        public GameObject playButton; // Play ��ư
        public Sprite playIcon; // ��� ������
        public Sprite pauseIcon; // �Ͻ����� ������
        private Image buttonImage; // ��ư �̹���

        [Header("Sliders")]
        public Slider videoSlider; // ��� ��ġ�� ������ �����̴�
        public Slider audioSlider; // ����� ���� ���� �����̴�
        public Slider brightnessSlider; // ȭ�� ��� ���� �����̴�

        public Object screenDimmer; // ȭ�� ��Ӱ� �ϴ� ������Ʈ
        private Material screenDimmerMaterial; // ȭ�� ��Ӱ� �ϴ� ������Ʈ�� Material

        public bool isDragging = false;

        void Start()
        {
            // ������ �غ� �Ϸ�Ǹ� �����̴� ����
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // ��ư�� �̹��� ������Ʈ ����
            buttonImage = playButton.GetComponent<Image>();

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
        void Update()
        {
            UpdateTimeText(videoSlider.value);

            // �巡�� ���� �ƴ϶�� �����̴� �ڵ� ������Ʈ
            if (!isDragging)
            {
                UpdateSliderValue();
                targetKnob.SetKnobValue(videoSlider.value);
            }
            else if (isDragging)
            {
                videoPlayer.time = videoSlider.value * videoPlayer.length;
            }
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

        public void UpdateTimeText(float sliderValue)
        {
            if (timeText == null) return;

            double currentTime = sliderValue * videoPlayer.length;
            double totalTime = videoPlayer.length;

            string currentTimeString = FormatTime(currentTime);
            string totalTimeString = FormatTime(totalTime);

            timeText.text = $"{currentTimeString} / {totalTimeString}";
        }


        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60f);
            int seconds = Mathf.FloorToInt((float)time % 60f);
            return $"{minutes:D2}:{seconds:D2}";
        }

        public void TogglePlayPause()
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
            float mappedBrightness = Mathf.Lerp(0.2f, 1.0f, brightness);
 
            // Screen Dimmer�� alpha �� ���� (brightness ���� Ŭ���� alpha ���� �۾������� �ݴ�� ����)
            Color color = screenDimmerMaterial.color;
            color.a = 1.0f - mappedBrightness;
            screenDimmerMaterial.color = color;
        }
    }
}
