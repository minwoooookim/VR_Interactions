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
        public VideoPlayer videoPlayer; // VideoPlayer ������Ʈ
        public Slider videoSlider;     // ��� ��ġ�� ������ �����̴�
        public TextMeshProUGUI timeText;          // ���� ��� �ð��� ǥ���� �ؽ�Ʈ

        private bool isDragging = false;

        void Start()
        {
            // �����̴��� �ִ밪�� ���� ���̿� �°� ����
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();

            // �����̴� �̺�Ʈ ������ �߰�
            videoSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
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

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    // �����̴� Ŭ�� ����
        //    isDragging = true;
        //    videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        //}

        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    // �����̴� Ŭ�� ����
        //    isDragging = false;
        //    // �����̴� ���� ���� ���� ��� ��ġ ����
        //    videoPlayer.time = videoSlider.value * videoPlayer.length;
        //}

        public void OnSliderValueChanged()
        {
            if (true
                //isDragging
                )
            {
                // �����̴� ���� ���� ���� ��� ��ġ ����
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
            // �����̴� �ʱⰪ �� �ؽ�Ʈ �ʱ�ȭ
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // �����̴� ���� 0~1�� ����ȭ��
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
