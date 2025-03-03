using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;     // 현재 표시되는 단어
    [SerializeField] private TMP_InputField inputField;   // 사용자가 타이핑하는 InputField

    private List<string> allWords = new List<string>();         // words.txt에서 읽어온 모든 단어
    private List<string> availableAllWords = new List<string>();  // 아직 사용되지 않은 모든 단어

    // 기존 6자 기준 분류 코드 (더 이상 사용하지 않음)
    // private List<string> shortWords = new List<string>();         // 길이가 6자 이하인 단어들 (원본)
    // private List<string> availableShortWords = new List<string>();  // 아직 사용되지 않은 6자 이하 단어들
    //
    // private List<string> longWords = new List<string>();          // 길이가 6자 초과인 단어들 (원본)
    // private List<string> availableLongWords = new List<string>();   // 아직 사용되지 않은 6자 초과 단어들

    private void Start()
    {
        // words.txt 파일에서 단어들 읽어오기
        string filePath = Path.Combine(Application.dataPath, "words.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            allWords.AddRange(lines);
            availableAllWords.AddRange(lines);

            // 기존 단어 길이별 분류 로직 (더 이상 사용하지 않음)
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

            // available 리스트도 초기화
            availableShortWords.AddRange(shortWords);
            availableLongWords.AddRange(longWords);
            */
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
    // 기존에는 이전 단어의 길이에 따라 분류했으나, 현재는 전체 단어에서 중복 없이 랜덤 선택함.
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
            // 기존 길이 구분 로직 (더 이상 사용하지 않음)
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
                    Debug.LogWarning("6자 초과 단어가 없습니다.");
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
                    Debug.LogWarning("6자 이하 단어가 없습니다.");
                    nextWord = previousWord;
                }
            }
            */

            // 새 로직: availableAllWords에서 중복되지 않게 랜덤 선택
            if (availableAllWords.Count == 0)
            {
                // 모든 단어를 사용했다면 원본 리스트에서 현재 단어를 제외하고 재설정
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
