using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SentenceDisplayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public TMP_Text testSentenceText;   // ���� ǥ�õǴ� ����
    [SerializeField] public TMP_InputField inputField;     // ����ڰ� Ÿ�����ϴ� InputField

    [Header("Text Asset")]
    public TextAsset phrasesFile;                        // �ν����Ϳ��� �Ҵ��� �ؽ�Ʈ ����

    private List<string> allWords = new List<string>();   // examplesFile���� �о�� ��� ����
    // ���� ���̺��� ���� ������ ���� ������� �����ϴ� ��ųʸ�
    private Dictionary<int, List<string>> availableWordsByLength = new Dictionary<int, List<string>>();

    private void Start()
    {
        if (phrasesFile != null)
        {
            // TextAsset�� ������ �� ������ �и� (���� ���� ����)
            string[] lines = phrasesFile.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            allWords.AddRange(lines);

            // ���� ���̺��� availableWordsByLength �ʱ�ȭ
            foreach (string sentence in allWords)
            {
                int len = sentence.Length;
                if (availableWordsByLength.ContainsKey(len))
                {
                    availableWordsByLength[len].Add(sentence);
                }
                else
                {
                    availableWordsByLength[len] = new List<string>() { sentence };
                }
            }
        }
        else
        {
            Debug.LogError("TextAsset examplesFile�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        testSentenceText.text = string.Empty;
        inputField.text = string.Empty;
    }

    // �ܺο��� ȣ���� �� ���ϴ� ���̸� ���ڷ� ����
    public void ShowNextSentence(int desiredLength)
    {
        ShowRandomSentence(desiredLength);
    }

    public string GetCurrentSentence()
    {
        return testSentenceText.text;
    }

    // ���ϴ� ������ ����� �� ���� ��µ��� ���� ������ ���� ����
    private void ShowRandomSentence(int desiredLength)
    {
        string nextWord = string.Empty;

        // ������ ������ ���� ����Ʈ�� ���ų� ��� ���Ǿ��ٸ� �缳��
        if (!availableWordsByLength.ContainsKey(desiredLength) || availableWordsByLength[desiredLength].Count == 0)
        {
            List<string> wordsOfDesiredLength = allWords.Where(s => s.Length == desiredLength).ToList();
            // ���� ǥ�õ� ������ �ִٸ� �ߺ� ����
            if (!string.IsNullOrEmpty(testSentenceText.text))
            {
                wordsOfDesiredLength.Remove(testSentenceText.text);
            }
            availableWordsByLength[desiredLength] = wordsOfDesiredLength;
        }

        List<string> availableList = availableWordsByLength[desiredLength];
        int index = Random.Range(0, availableList.Count);
        nextWord = availableList[index];
        availableList.RemoveAt(index);

        testSentenceText.text = nextWord;
        inputField.text = string.Empty;
    }
}
