using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class VideoExampleDisplayer : MonoBehaviour
{
    // ĵ������ �ִ� �ؽ�Ʈ ������Ʈ�� �����մϴ�.
    public TextMeshProUGUI videoExamplesText;
    // videoexamples_for_unity.txt ������ �����մϴ�. (Resources ���� Ȥ�� �����Ϳ��� ���� �Ҵ�)
    public TextAsset examplesFile;

    // ���Ͽ��� �ҷ��� ���õ��� ������ ����Ʈ
    private List<string> availableExamples = new List<string>();
    private string currentExample;

    void Start()
    {
        // ������ �Ҵ�Ǿ� ������ ������ �Ľ��մϴ�.
        if (examplesFile != null)
        {
            // ���õ��� �� ��(Ȥ�� \r\n\r\n)�� �������� ���еǾ� �ִٰ� �����մϴ�.
            string[] examples = Regex.Split(examplesFile.text, @"\r?\n\r?\n");
            foreach (string example in examples)
            {
                // �յ� ���� ���� �� ����Ʈ�� �߰�
                if (!string.IsNullOrWhiteSpace(example))
                    availableExamples.Add(example.Trim());
            }
        }
        else
        {
            Debug.LogError("���� ������ �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    public void ShowNextExample()
    {
        DisplayNextExample();
    }

    public string GetCurrentExample()
    {
        if (videoExamplesText != null && !string.IsNullOrEmpty(videoExamplesText.text))
        {
            return currentExample;
        }
        else
        {
            return "���� ǥ�õ� ���ð� �����ϴ�.";
        }
    }

    public void PrintReady()
    {
        videoExamplesText.text = "�غ�";
    }
    public void PrintCompleted()
    {
        videoExamplesText.text = "�Ϸ�";
    }

    // ���� ���ø� �����ϰ� ǥ���ϴ� �Լ�
    private void DisplayNextExample()
    {
        if (availableExamples.Count > 0)
        {
            // ���� �ε��� ����
            int randomIndex = Random.Range(0, availableExamples.Count);
            string nextExample = availableExamples[randomIndex];

            currentExample = nextExample;

            // �� ������ �и��ϰ� ù ���� ������ ������ �ٵ��� �����մϴ�.
            string[] lines = nextExample.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
            videoExamplesText.text = lines.Length > 1 ? string.Join("\n", lines.Skip(1)) : nextExample;

            availableExamples.RemoveAt(randomIndex);
        }
        else
        {
            // �� �̻� ����� ���ð� ������ "�Ϸ�" ���
            PrintCompleted();
        }
    }
}
