using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SentenceDisplayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public TMP_Text testSentenceText;     // 현재 표시되는 단어
    [SerializeField] public TMP_InputField inputField;   // 사용자가 타이핑하는 InputField

    private List<string> allWords = new List<string>();         // phrases.txt에서 읽어온 모든 단어
    private List<string> availableAllWords = new List<string>();  // 아직 사용되지 않은 모든 단어


    private void Start()
    {
        // phrases.txt 파일에서 단어들 읽어오기
        string filePath = Path.Combine(Application.dataPath, "phrases.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);
            availableAllWords.AddRange(lines);
        }
        else
        {
            Debug.LogError($"phrases.txt 파일을 찾을 수 없습니다. 경로: {filePath}");
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

    // 단어를 랜덤으로 선택하여 표시하는 메서드
    private void ShowRandomWord(bool firstWord = false)
    {
        string nextWord = string.Empty;

        // 첫 단어 선택은 availableAllWords에서 진행
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
            // availableAllWords에서 중복되지 않게 랜덤 선택
            if (availableAllWords.Count == 0)
            {
                // 모든 단어를 사용했다면 원본 리스트에서 현재 단어를 제외하고 재설정
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
