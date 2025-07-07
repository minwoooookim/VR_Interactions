using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoPlayerControlScript;

namespace VideoplayerTesterNamespace
{
    public class RealVideoplayerTester : MonoBehaviour
    {
        [SerializeField] private VideoExampleDisplayer videoExampleDisplayer;
        [SerializeField] private RealVideoPlayerControls videoPlayerControls;
        [SerializeField] private TMP_Text countdownText;

        [Header("Handle Display")]
        [SerializeField] private TMP_Text videoHandleDisplay;
        [SerializeField] private TMP_Text audioHandleDisplay;
        [SerializeField] private TMP_Text brightnessHandleDisplay;

        // ���������� ����� ������
        private bool isTestRunning = false;
        private const int repeatCount = 5; // �׽�Ʈ ��ü �ݺ� Ƚ��
        private float sessionTotalTime = 0f; // ���� ���� ������ �ð�
        private string filePath = string.Empty; // ��� ���� ��� (�ݺ����� �����ϹǷ�, �� �� ���)

        private bool isCompleted = false; // ����ڰ� ĥ�����̸� �Ϸ� �� ���� ��ư�� �������� ����

        // �ܺο��� ���� ��ư ȣ�� �� ��� (�ѹ� ������ �ߺ� �Է� ����)
        public void StartTest()
        {
            if (isTestRunning)
                return; // �̹� ���� ���̸� �ߺ� ���� ����

            isTestRunning = true;

            // ���� ��� ���
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // �÷��̾� ��ȣ�� ����Ͽ� ������ ����
            string playerNum = DataHolder.playerNumber.ToString();
            string folderName = $"results_{playerNum}";
            string resultsPath = Path.Combine(desktopPath, folderName);

            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }

            string fileName = $"Player_{playerNum}_Videoplayer_Real.txt";
            filePath = Path.Combine(resultsPath, fileName);

            // �ڷ�ƾ ����
            StartCoroutine(TestRoutine());
        }

        private IEnumerator TestRoutine()
        {
            // ��ü �׽�Ʈ �ݺ�: 3�� �ݺ� �� ����
            for (int i = 0; i < repeatCount; i++)
            {
                videoExampleDisplayer.PrintReady();

                // 3�� ī��Ʈ�ٿ� ����
                yield return StartCoroutine(CountdownCoroutine(3));

                videoExampleDisplayer.ShowNextExample();

                // �ð� ���� ����
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // ���� ���� ���� ����
                string exampleContent = videoExampleDisplayer.GetCurrentExample();

                // ����ڰ� �½�ũ�� �Ϸ��� ������ ���
                while (!isCompleted)
                {
                    yield return null;
                }

                // �ð� ���� ����
                stopwatch.Stop();
                float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
                sessionTotalTime += elapsedSeconds;

                // ����� ���� �ҿ� �ð� ��� (Total Time ����� ���� ����)
                SaveTestResult(exampleContent, elapsedSeconds);

                isCompleted = !isCompleted;

                // ���� �÷��̾� �ʱ�ȭ
                videoPlayerControls.ResetPlayer();
            }

            // ��ü �׽�Ʈ�� ���� �� ���� �ð�(Total Time)�� ������ ���
            SaveTotalTime(sessionTotalTime);

            // �׽�Ʈ ���� �� �ʱ� ���·� ����
            sessionTotalTime = 0f;
            isTestRunning = false;
            videoExampleDisplayer.PrintCompleted();
            countdownText.text = "�Ϸ�";
        }

        private IEnumerator CountdownCoroutine(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            countdownText.text = string.Empty;
        }

        public void SetCompleted()
        {
            isCompleted = true;
        }

        public void CheckIsCompleted()
        {
            // ���� ���� �ؽ�Ʈ�� ������
            string exampleText = videoExampleDisplayer.GetCurrentExample();
            if (string.IsNullOrEmpty(exampleText))
                return;

            // �ٹٲ� �������� �и� (���� ������ �ּ� 4��)
            string[] lines = exampleText.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 4)
                return;

            // "�ð�: 0:00"���� "�ð�: " ���� �� ���� �κ��� expectedTime (��: "0:00")
            string expectedTime = lines[1].Replace("Time: ", "").Trim();
            // "����: 0"���� "����: " ���� �� ���� �κ��� expectedAudio (����)
            string expectedAudio = lines[2].Replace("Volume: ", "").Trim();
            // "���: 0"���� "���: " ���� �� ���� �κ��� expectedBrightness (����)
            string expectedBrightness = lines[3].Replace("Brightness: ", "").Trim();

            // handle�� �ؽ�Ʈ�� ���� ���� �� ��
            if (videoHandleDisplay.text == expectedTime &&
                audioHandleDisplay.text == expectedAudio &&
                brightnessHandleDisplay.text == expectedBrightness)
            {
                SetCompleted();
            }
        }

        private void SaveTestResult(string exampleContent, float elapsedSeconds)
        {
            // ����� ���� ���� (elapsedSeconds�� �Ҽ��� ��° �ڸ����� ǥ��)
            string record = $"{exampleContent}\n{elapsedSeconds:F3}";

            // ������ �����ϸ� ���� �� �߰�, ������ ���� ����
            if (File.Exists(filePath))
            {
                File.AppendAllText(filePath, Environment.NewLine + record);
            }
            else
            {
                File.WriteAllText(filePath, record);
            }
        }

        // ���� �ð�(Total Time)�� �������� �����ϴ� �Լ�
        private void SaveTotalTime(float totalTime)
        {
            string record = Environment.NewLine + $"Total Time: {totalTime:F3}";
            File.AppendAllText(filePath, record);
        }
    }
}