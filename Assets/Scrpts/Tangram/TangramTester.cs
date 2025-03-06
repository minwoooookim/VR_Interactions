using System;
using System.Collections;
using System.Collections.Generic;
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

    // ���������� ����� ������
    private string imageName;
    private float startTime;
    private bool testStarted = false;

    void Start()
    {
        // �ʱ�ȭ �ڵ尡 �ʿ��ϸ� �߰�
    }

    void Update()
    {
        // �ʿ信 ���� Update���� ���� �߰� ����
    }

    // �ܺο��� ���� ��ư ȣ�� �� ���
    public void OnStartButton()
    {
        // �̹� �׽�Ʈ�� �������̸� �ߺ� ���� ����
        if (!testStarted)
        {
            StartCoroutine(CountdownCoroutine());
        }
        testStarted = true;
    }

    // 3�� ī��Ʈ�ٿ� �ڷ�ƾ
    private IEnumerator CountdownCoroutine()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        // ī��Ʈ�ٿ� ���� �� �ؽ�Ʈ ����
        countdownText.text = "";
        // ���� �̹����� ǥ���ϰ� �ð� ���� ����
        tangramExampleDisplayer.ShowNextImage();
        startTime = Time.time;
        // ���� �̹��� �̸� ����
        imageName = tangramExampleDisplayer.GetCurrentImageName();
    }

    // �ܺο��� �Ϸ� ��ư ȣ�� �� ���
    public void OnCompleteButton()
    {
        if (!testStarted)
        {
            Debug.LogWarning("�׽�Ʈ�� ���۵��� �ʾҽ��ϴ�.");
            return;
        }

        // �ð� ���� ���� �� ��� �ð� ���
        float elapsedTime = Time.time - startTime;
        // �̹��� ���� ȣ��
        tangramExampleDisplayer.EmptyImage();
        // ��� ���� ���� Ȥ�� ���� �߰�
        WriteResultToFile(imageName, elapsedTime);
        // �׽�Ʈ ���� �ʱ�ȭ (�ʿ� ��)
        testStarted = false;
    }

    // ����� ���Ϸ� �����ϴ� �Լ� (���� ��ϰ� �� �ð��� �ջ��Ͽ� Total Time ���)
    private void WriteResultToFile(string imageName, float elapsedTime)
    {
        // ����ȭ���� results ���� ��� ����
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string resultsFolder = Path.Combine(desktopPath, "results");

        // ������ ������ ����
        if (!Directory.Exists(resultsFolder))
        {
            Directory.CreateDirectory(resultsFolder);
        }

        // InteractionSelector���� ���� ���õ� ����� �̸� ��������
        Toggle selectedToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
        string toggleName = selectedToggle != null ? selectedToggle.name : "None";

        // ���ϸ� ����
        string fileName = $"Player_{playerNumber.text}_Tangram_{toggleName}.txt";
        string filePath = Path.Combine(resultsFolder, fileName);

        float totalTime = 0f;
        List<string> lines = new List<string>();

        // ������ �����ϸ� ���� ���� �о����
        if (File.Exists(filePath))
        {
            lines = File.ReadAllLines(filePath).ToList();
            // ���� ������ ���� "Total Time"���� �����ϸ� ����
            if (lines.Count > 0 && lines[lines.Count - 1].StartsWith("Total Time"))
            {
                lines.RemoveAt(lines.Count - 1);
            }
            // ���� ������ ��� �ð� ���� �ջ� (�Ǽ��� �Ľ� ������ ���θ�)
            foreach (string line in lines)
            {
                if (float.TryParse(line, out float time))
                {
                    totalTime += time;
                }
            }
        }

        // ���� ����� �ð� �߰�
        totalTime += elapsedTime;

        // ����� ����: �̹��� �̸��� �ð� (��, �Ҽ��� ��°�ڸ�)
        string contentToAppend = $"{imageName}\n{elapsedTime.ToString("F3")}";

        // ���ο� ��� �߰� (���� ���� �ڿ�)
        lines.Add(imageName);
        lines.Add(elapsedTime.ToString("F3"));
        // Total Time ��� �߰�
        lines.Add("Total Time: " + totalTime.ToString("F3"));

        // ���� ���� ���� (��ü ���� �����)
        File.WriteAllLines(filePath, lines);
    }
}
