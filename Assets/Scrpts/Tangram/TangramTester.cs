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
    [SerializeField] private TangramRespawner tangramRespawner;

    // ���������� ����� ������
    private string imageName;
    private float startTime;
    private bool testStarted = false;
    private int roundCount = 0; // ����� ���� �� (�ִ� 5ȸ)

    // �ܺο��� ���� ��ư ȣ�� �� ���
    public void OnStartButton()
    {
        // �̹� �׽�Ʈ�� ���� ���̰ų� 5ȸ�� ��� �Ϸ�Ǿ����� �������� ����
        if (!testStarted && roundCount < 5)
        {
            testStarted = true;
            StartCoroutine(CountdownCoroutine());
        }
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

    // �ܺο��� �Ϸ�(����) ��ư ȣ�� �� ���
    public void OnNextButton()
    {
        if (!testStarted)
        {
            Debug.LogWarning("�׽�Ʈ�� ���۵��� �ʾҽ��ϴ�.");
            return;
        }

        // �ð� ���� ���� �� ��� �ð� ���
        float elapsedTime = Time.time - startTime;
        // �̹��� ���� �� ���ο� Ÿ�� ���ġ
        tangramExampleDisplayer.EmptyImage();
        tangramRespawner.RespawnTangram();

        // ���� ���� ����
        roundCount++;

        // 5��° ����� ���� ���(���� ����)�� ���Ͽ� �ۼ�
        bool isFinalRound = (roundCount == 5);
        WriteResultToFile(imageName, elapsedTime, isFinalRound);

        // ���� ������ ���尡 �ƴ϶�� �ٽ� ���� ���� ���� �����ϵ��� testStarted �ʱ�ȭ
        testStarted = false;
    }

    // ����� ���Ϸ� �����ϴ� �Լ�
    // isFinalRound�� true�̸� ���� ��ϵ� �ð��� �� �ð��� �ջ��Ͽ� Total Time ����� �߰�
    private void WriteResultToFile(string imageName, float elapsedTime, bool isFinalRound)
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

        // ���ϸ� ���� : Player_(playerNumber)_Keyboard_(��� �̸�).txt
        string fileName = $"Player_{playerNumber.text}_Keyboard_{toggleName}.txt";
        string filePath = Path.Combine(resultsFolder, fileName);

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
        }

        // ���� ����� ���� : �̹��� �̸��� ��� �ð� (��, �Ҽ��� ��°�ڸ�)
        lines.Add(imageName);
        lines.Add(elapsedTime.ToString("F3"));

        // ������ ������ ���, ������ ��ϵ� �ð��� �� �ð��� �ջ��Ͽ� Total Time ��� �߰�
        if (isFinalRound)
        {
            float totalTime = 0f;
            foreach (string line in lines)
            {
                // �̹��� �̸��� �ǳʶٰ�, �Ǽ� ��ȯ ������ ���� �ջ�
                if (float.TryParse(line, out float time))
                {
                    totalTime += time;
                }
            }
            lines.Add("Total Time: " + totalTime.ToString("F3"));
        }

        // ���Ͽ� ��ü ���� �����
        File.WriteAllLines(filePath, lines);
    }

}
