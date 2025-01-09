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
        public VideoPlayer videoPlayer;     // VideoPlayer ������Ʈ
        public Slider videoSlider;          // ��� ��ġ�� ������ �����̴�
        public TextMeshProUGUI timeText;    // ���� ��� �ð��� ǥ���� �ؽ�Ʈ

        private bool isDragging = false;

        void Start()
        {
            // �����̴��� �ִ밪�� ���� ���̿� �°� ����
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.Prepare();
            videoSlider.onValueChanged.AddListener(OnSliderValueChanged);

            // EventTrigger�� ����Ͽ� �����̴��� Ŭ�� �̺�Ʈ ó��
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
            // �����̴� �ʱⰪ �� �ؽ�Ʈ �ʱ�ȭ
            videoSlider.value = 0;
            videoSlider.maxValue = 1; // �����̴� ���� 0~1�� ����ȭ��
            UpdateTimeText();
        }

        private void UpdateSliderValue()
        {
            // ���� ��� �ð��� �����̴��� �ؽ�Ʈ�� ������Ʈ
            videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
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
