using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SentenceDisplayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testSentenceText;     // ���� ǥ�õǴ� �ܾ�
    [SerializeField] private TMP_InputField inputField;   // ����ڰ� Ÿ�����ϴ� InputField

    private List<string> allWords = new List<string>();         // phrases.txt���� �о�� ��� �ܾ�
    private List<string> availableAllWords = new List<string>();  // ���� ������ ���� ��� �ܾ�


    private void Start()
    {
        // phrases.txt ���Ͽ��� �ܾ�� �о����
        string filePath = Path.Combine(Application.dataPath, "phrases.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);
            availableAllWords.AddRange(lines);
        }
        else
        {
            Debug.LogError($"phrases.txt ������ ã�� �� �����ϴ�. ���: {filePath}");
        }

        // �ܾ �ϳ� �̻� ������ ù �ܾ ǥ��
        if (allWords.Count > 0)
        {
            ShowRandomWord(firstWord: true);
        }
    }

    private void Update()
    {
        // InputField�� �Է��� ������ ǥ�õ� �ܾ�� ������
        if (inputField.text == testSentenceText.text)
        {
            ShowRandomWord();
            inputField.text = string.Empty;
        }
    }

    // �ܾ �������� �����Ͽ� ǥ���ϴ� �޼���
    private void ShowRandomWord(bool firstWord = false)
    {
        string nextWord = string.Empty;

        // ù �ܾ� ������ availableAllWords���� ����
        if (firstWord)
        {
            if (availableAllWords.Count == 0)
            {
                availableAllWords.AddRange(allWords);
            }
            int index = Random.Range(0, availableAllWords.Count);
            nextWord = availableAllWords[index];
            availableAllWords.RemoveAt(index);
        }
        else
        {
            // availableAllWords���� �ߺ����� �ʰ� ���� ����
            if (availableAllWords.Count == 0)
            {
                // ��� �ܾ ����ߴٸ� ���� ����Ʈ���� ���� �ܾ �����ϰ� �缳��
                availableAllWords.AddRange(allWords);
                availableAllWords.Remove(testSentenceText.text);
            }
            int index = Random.Range(0, availableAllWords.Count);
            nextWord = availableAllWords[index];
            availableAllWords.RemoveAt(index);
        }

        testSentenceText.text = nextWord;
    }
}
