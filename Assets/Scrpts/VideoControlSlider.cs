using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace VideoPlayerControlScript
{
    public class VideoControlSlider : MonoBehaviour
    {
        public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
        public Slider videoSlider;     // ��� ��ġ�� ������ �����̴�
        public TextMeshProUGUI timeText;          // ���� ��� �ð��� ǥ���� �ؽ�Ʈ

        private bool isDragging = false;

        void Start()
        {
            // �����̴��� �ִ밪�� ���� ���̿� �°� ����
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
        }

        void Update()
        {
            if (!isDragging && videoPlayer.isPlaying)
            {
                // ���� ��� �ð��� �����̴��� �ؽ�Ʈ�� ������Ʈ
                videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
                UpdateTimeText();
            }
        }

        public void OnSliderValueChanged()
        {
            isDragging = true;
        }

        public void OnSliderDragEnd()
        {
            isDragging = false;

            // �����̴� ���� ���� ���� ��� ��ġ ����
            videoPlayer.time = videoSlider.value * videoPlayer.length;
        }

        private void OnVideoPrepared(VideoPlayer vp)
        {
            // �����̴� �ʱⰪ �� �ؽ�Ʈ �ʱ�ȭ
            videoSlider.value = 0;
            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            // ���� ��� �ð� �� �� �ð� ���
            double currentTime = videoPlayer.time;
            double totalTime = videoPlayer.length;

            // �ؽ�Ʈ ����: "����ð� / �ѽð� (��:��)"
            string currentTimeString = FormatTime(currentTime);
            string totalTimeString = FormatTime(totalTime);

            //timeText.text = $"{currentTimeString} / {totalTimeString}";
            timeText.text = $"{currentTimeString}";
        }

        private string FormatTime(double time)
        {
            int minutes = Mathf.FloorToInt((float)time / 60);
            int seconds = Mathf.FloorToInt((float)time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}