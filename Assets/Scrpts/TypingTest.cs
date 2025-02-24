using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;     // ���� ǥ�õǴ� �ܾ�
    [SerializeField] private TMP_InputField inputField;   // ����ڰ� Ÿ�����ϴ� InputField

    private List<string> allWords = new List<string>();      // words.txt���� �о�� ��� �ܾ�
    private List<string> shortWords = new List<string>();      // ���̰� 5�� ������ �ܾ��
    private List<string> longWords = new List<string>();       // ���̰� 5�� �ʰ��� �ܾ��

    private void Start()
    {
        // words.txt ���Ͽ��� �ܾ�� �о����
        string filePath = Path.Combine(Application.dataPath, "words.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);

            // �ܾ���� ���̿� ���� �з��ϱ�
            foreach (string word in allWords)
            {
                if (word.Length <= 5)
                {
                    shortWords.Add(word);
                }
                else
                {
                    longWords.Add(word);
                }
            }
        }
        else
        {
            Debug.LogError($"words.txt ������ ã�� �� �����ϴ�. ���: {filePath}");
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
        if (inputField.text == testWordText.text)
        {
            ShowRandomWord();
            inputField.text = string.Empty;
        }
    }

    // �ܾ �������� �����Ͽ� ǥ���ϴ� �޼���
    // firstWord�� true�� ��ü �ܾ� ��Ͽ��� ���� ����
    private void ShowRandomWord(bool firstWord = false)
    {
        string nextWord = string.Empty;

        if (firstWord)
        {
            // ù �ܾ�� ��ü �ܾ�� ���� ����
            int index = Random.Range(0, allWords.Count);
            nextWord = allWords[index];
        }
        else
        {
            // ���� �ܾ� ��������
            string previousWord = testWordText.text;

            // ���� �ܾ 5�� �����̸� longWords���� ����
            if (previousWord.Length <= 5)
            {
                if (longWords.Count > 0)
                {
                    int index = Random.Range(0, longWords.Count);
                    nextWord = longWords[index];
                }
                else
                {
                    Debug.LogWarning("5�� �ʰ� �ܾ �����ϴ�.");
                    nextWord = previousWord; // ��ü ����
                }
            }
            // ���� �ܾ 5�� �ʰ��̸� shortWords���� ����
            else
            {
                if (shortWords.Count > 0)
                {
                    int index = Random.Range(0, shortWords.Count);
                    nextWord = shortWords[index];
                }
                else
                {
                    Debug.LogWarning("5�� ���� �ܾ �����ϴ�.");
                    nextWord = previousWord; // ��ü ����
                }
            }
        }

        testWordText.text = nextWord;
    }
}
