using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardTester : MonoBehaviour
{
    [SerializeField] private SentenceDisplayer sentenceDisplayer;
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private ToggleGroup InteractionSelector;
    [SerializeField] private TMP_Text countdownText;

    private bool isTestRunning = false; // ���۹�ư �ߺ� �Է� ������ ���� flag 
    private const int repeatCount = 3; // �׽�Ʈ ��ü �ݺ� Ƚ��
    private float sessionTotalTime = 0f; // ���� ���� ������ �ð�
    private string filePath = string.Empty; // ��� ���� ��� (�ݺ����� �����ϹǷ�, �� �� ���)

    // �ܺ� ��ư���� ȣ���ϴ� ���� �Լ�
    public void StartTest()
    {
        if (isTestRunning)
            return; // �̹� ���� ���̸� �ߺ� ���� ����

        isTestRunning = true;

        // ���� ��� ���
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string resultsPath = Path.Combine(desktopPath, "results");
        if (!Directory.Exists(resultsPath))
        {
            Directory.CreateDirectory(resultsPath);
        }
        string playerNum = playerNumber.text;
        Toggle activeToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
        string toggleName = activeToggle != null ? activeToggle.name : "Unknown";
        string fileName = $"Player_{playerNum}_Keyboard_{toggleName}.txt";
        filePath = Path.Combine(resultsPath, fileName);

        // �ڷ�ƾ ����
        StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine()
    {
        // 3�� ī��Ʈ�ٿ� ����
        yield return StartCoroutine(CountdownCoroutine(3));

        // ��ü �׽�Ʈ �ݺ�: 3�� �ݺ� �� ����
        for (int i = 0; i < repeatCount; i++)
        {
            // ù ��° ȣ�� �� ShowNextWord(19)�� ����, ���� 20, 21 ������ ȣ��
            sentenceDisplayer.ShowNextSentence(19 + i);

            // �ð� ���� ����
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // ���� ���� ����
            string currentSentence = sentenceDisplayer.GetCurrentSentence();

            // ����ڰ� �Է��� ����� testSentenceText�� ������ ������ ������ ���
            while (sentenceDisplayer.inputField.text != sentenceDisplayer.testSentenceText.text)
            {
                yield return null;
            }

            // �ð� ���� ����
            stopwatch.Stop();
            float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
            sessionTotalTime += elapsedSeconds;

            // ����� ���� �ҿ� �ð� ��� (Total Time ����� ���� ����)
            SaveSentenceResult(currentSentence, elapsedSeconds);

            // �� ���ð� ���� �ٷ� ���� ���� ���� (��� �ڷ�ƾ ����)
        }

        // ��ü �׽�Ʈ�� ���� �� ���� �ð�(Total Time)�� ������ ���
        SaveTotalTime(sessionTotalTime);

        // �׽�Ʈ ���� �� �ʱ� ���·� ����
        sentenceDisplayer.testSentenceText.text = string.Empty;
        sentenceDisplayer.inputField.text = string.Empty;
        sessionTotalTime = 0f;
        isTestRunning = false;
        countdownText.text = "�Ϸ�";
    }

    // 3�� ī��Ʈ�ٿ� �ڷ�ƾ (�� ������ countdownText�� ǥ��)
    private IEnumerator CountdownCoroutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = string.Empty;
    }

    // �� ���� ����� �����ϴ� �Լ� (����� �ش� �ҿ�ð��� ���)
    private void SaveSentenceResult(string sentence, float elapsedSeconds)
    {
        // ����� ���� ���� (elapsedSeconds�� �Ҽ��� ��° �ڸ����� ǥ��)
        string record = $"{sentence}\n{elapsedSeconds:F3}";

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
