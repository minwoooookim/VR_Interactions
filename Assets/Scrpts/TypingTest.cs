using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;     // ���� ǥ�õǴ� �ܾ�
    [SerializeField] private TMP_InputField inputField;   // ����ڰ� Ÿ�����ϴ� InputField

    private List<string> allWords = new List<string>();         // words.txt���� �о�� ��� �ܾ�
    private List<string> availableAllWords = new List<string>();  // ���� ������ ���� ��� �ܾ�

    // ���� 6�� ���� �з� �ڵ� (�� �̻� ������� ����)
    // private List<string> shortWords = new List<string>();         // ���̰� 6�� ������ �ܾ�� (����)
    // private List<string> availableShortWords = new List<string>();  // ���� ������ ���� 6�� ���� �ܾ��
    //
    // private List<string> longWords = new List<string>();          // ���̰� 6�� �ʰ��� �ܾ�� (����)
    // private List<string> availableLongWords = new List<string>();   // ���� ������ ���� 6�� �ʰ� �ܾ��

    private void Start()
    {
        // words.txt ���Ͽ��� �ܾ�� �о����
        string filePath = Path.Combine(Application.dataPath, "words.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);
            availableAllWords.AddRange(lines);

            // ���� �ܾ� ���̺� �з� ���� (�� �̻� ������� ����)
            /*
            foreach (string word in allWords)
            {
                if (word.Length <= 6)
                {
                    shortWords.Add(word);
                }
                else
                {
                    longWords.Add(word);
                }
            }

            // available ����Ʈ�� �ʱ�ȭ
            availableShortWords.AddRange(shortWords);
            availableLongWords.AddRange(longWords);
            */
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
    // �������� ���� �ܾ��� ���̿� ���� �з�������, ����� ��ü �ܾ�� �ߺ� ���� ���� ������.
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
            // ���� ���� ���� ���� (�� �̻� ������� ����)
            /*
            string previousWord = testWordText.text;
            if (previousWord.Length <= 6)
            {
                if (availableLongWords.Count == 0)
                {
                    availableLongWords.AddRange(longWords);
                    availableLongWords.Remove(previousWord);
                }
                if (availableLongWords.Count > 0)
                {
                    int index = Random.Range(0, availableLongWords.Count);
                    nextWord = availableLongWords[index];
                    availableLongWords.RemoveAt(index);
                }
                else
                {
                    Debug.LogWarning("6�� �ʰ� �ܾ �����ϴ�.");
                    nextWord = previousWord;
                }
            }
            else
            {
                if (availableShortWords.Count == 0)
                {
                    availableShortWords.AddRange(shortWords);
                    availableShortWords.Remove(previousWord);
                }
                if (availableShortWords.Count > 0)
                {
                    int index = Random.Range(0, availableShortWords.Count);
                    nextWord = availableShortWords[index];
                    availableShortWords.RemoveAt(index);
                }
                else
                {
                    Debug.LogWarning("6�� ���� �ܾ �����ϴ�.");
                    nextWord = previousWord;
                }
            }
            */

            // �� ����: availableAllWords���� �ߺ����� �ʰ� ���� ����
            if (availableAllWords.Count == 0)
            {
                // ��� �ܾ ����ߴٸ� ���� ����Ʈ���� ���� �ܾ �����ϰ� �缳��
                availableAllWords.AddRange(allWords);
                availableAllWords.Remove(testWordText.text);
            }
            int index = Random.Range(0, availableAllWords.Count);
            nextWord = availableAllWords[index];
            availableAllWords.RemoveAt(index);
        }

        testWordText.text = nextWord;
    }
}
