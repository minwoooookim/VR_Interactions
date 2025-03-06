using System;
using System.Collections;
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
        testStarted = true;
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

    // ����� ���Ϸ� �����ϴ� �Լ�
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

        // ����� ����: �̹��� �̸��� �ð� (��, �Ҽ��� ��°�ڸ�)
        string contentToAppend = $"{imageName}\n{elapsedTime.ToString("F3")}";

        // ������ �����ϸ� ���� �� ���� �߰�, �ƴϸ� �� ���� ����
        if (File.Exists(filePath))
        {
            File.AppendAllText(filePath, "\n" + contentToAppend);
        }
        else
        {
            File.WriteAllText(filePath, contentToAppend);
        }
    }
}
