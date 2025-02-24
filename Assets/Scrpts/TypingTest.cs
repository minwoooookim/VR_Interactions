using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;     // 현재 표시되는 단어
    [SerializeField] private TMP_InputField inputField;   // 사용자가 타이핑하는 InputField

    private List<string> allWords = new List<string>();      // words.txt에서 읽어온 모든 단어
    private List<string> shortWords = new List<string>();      // 길이가 5자 이하인 단어들
    private List<string> longWords = new List<string>();       // 길이가 5자 초과인 단어들

    private void Start()
    {
        // words.txt 파일에서 단어들 읽어오기
        string filePath = Path.Combine(Application.dataPath, "words.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);

            // 단어들을 길이에 따라 분류하기
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
            Debug.LogError($"words.txt 파일을 찾을 수 없습니다. 경로: {filePath}");
        }

        // 단어가 하나 이상 있으면 첫 단어를 표시
        if (allWords.Count > 0)
        {
            ShowRandomWord(firstWord: true);
        }
    }

    private void Update()
    {
        // InputField에 입력한 내용이 표시된 단어와 같으면
        if (inputField.text == testWordText.text)
        {
            ShowRandomWord();
            inputField.text = string.Empty;
        }
    }

    // 단어를 랜덤으로 선택하여 표시하는 메서드
    // firstWord가 true면 전체 단어 목록에서 랜덤 선택
    private void ShowRandomWord(bool firstWord = false)
    {
        string nextWord = string.Empty;

        if (firstWord)
        {
            // 첫 단어는 전체 단어에서 랜덤 선택
            int index = Random.Range(0, allWords.Count);
            nextWord = allWords[index];
        }
        else
        {
            // 이전 단어 가져오기
            string previousWord = testWordText.text;

            // 이전 단어가 5자 이하이면 longWords에서 선택
            if (previousWord.Length <= 5)
            {
                if (longWords.Count > 0)
                {
                    int index = Random.Range(0, longWords.Count);
                    nextWord = longWords[index];
                }
                else
                {
                    Debug.LogWarning("5자 초과 단어가 없습니다.");
                    nextWord = previousWord; // 대체 로직
                }
            }
            // 이전 단어가 5자 초과이면 shortWords에서 선택
            else
            {
                if (shortWords.Count > 0)
                {
                    int index = Random.Range(0, shortWords.Count);
                    nextWord = shortWords[index];
                }
                else
                {
                    Debug.LogWarning("5자 이하 단어가 없습니다.");
                    nextWord = previousWord; // 대체 로직
                }
            }
        }

        testWordText.text = nextWord;
    }
}
