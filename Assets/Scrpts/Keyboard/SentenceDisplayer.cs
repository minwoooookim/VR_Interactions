using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SentenceDisplayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public TMP_Text testSentenceText;     // ���� ǥ�õǴ� �ܾ�
    [SerializeField] public TMP_InputField inputField;   // ����ڰ� Ÿ�����ϴ� InputField

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

        testSentenceText.text = string.Empty;
        inputField.text = string.Empty;
    }

    public void ShowNextWord()
    {
        ShowRandomWord();
    }

    public void ShowFirstWord()
    {
        ShowRandomWord(firstWord: true);
    }

    public string GetCurrentSentence()
    {
        return testSentenceText.text;
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
        inputField.text = string.Empty;
    }
}
