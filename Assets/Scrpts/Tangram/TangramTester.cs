using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TangramTester : MonoBehaviour
{
    [SerializeField] private TangramExampleDisplayer tangramExampleDisplayer;
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private ToggleGroup InteractionSelector;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TangramRespawner tangramRespawner;

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
        string resultsPath = Path.Combine(desktopPath, "results");
        if (!Directory.Exists(resultsPath))
        {
            Directory.CreateDirectory(resultsPath);
        }
        string playerNum = playerNumber.text;
        Toggle activeToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
        string toggleName = activeToggle != null ? activeToggle.name : "Unknown";
        string fileName = $"Player_{playerNum}_Tangram_{toggleName}.txt";
        filePath = Path.Combine(resultsPath, fileName);

        // �ڷ�ƾ ����
        StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine()
    {
        // ��ü �׽�Ʈ �ݺ�: 3�� �ݺ� �� ����
        for (int i = 0; i < repeatCount; i++)
        {
            // 3�� ī��Ʈ�ٿ� ����
            yield return StartCoroutine(CountdownCoroutine(3));

            tangramExampleDisplayer.ShowNextImage();

            // �ð� ���� ����
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // ���� �̹����� ����
            string imageName = tangramExampleDisplayer.GetCurrentImageName();

            // ����ڰ� ���� ��ư�� ���� ������ ���
            while (!isCompleted)
            {
                yield return null;
            }

            // �ð� ���� ����
            stopwatch.Stop();
            float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
            sessionTotalTime += elapsedSeconds;

            // ����� ���� �ҿ� �ð� ��� (Total Time ����� ���� ����)
            SaveTestResult(imageName, elapsedSeconds);

            isCompleted = !isCompleted;
            tangramExampleDisplayer.EmptyImage();
            tangramRespawner.RespawnTangram();
        }

        // ��ü �׽�Ʈ�� ���� �� ���� �ð�(Total Time)�� ������ ���
        SaveTotalTime(sessionTotalTime);

        // �׽�Ʈ ���� �� �ʱ� ���·� ����
        sessionTotalTime = 0f;
        isTestRunning = false;
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

    private void SaveTestResult(string imageName, float elapsedSeconds)
    {
        // ����� ���� ���� (elapsedSeconds�� �Ҽ��� ��° �ڸ����� ǥ��)
        string record = $"{imageName}\n{elapsedSeconds:F3}";

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
