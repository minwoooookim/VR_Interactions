using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SentenceDisplayer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public TMP_Text testSentenceText;   // 현재 표시되는 문장
    [SerializeField] public TMP_InputField inputField;     // 사용자가 타이핑하는 InputField

    [Header("Text Asset")]
    public TextAsset phrasesFile;                        // 인스펙터에서 할당할 텍스트 에셋

    private List<string> allWords = new List<string>();   // examplesFile에서 읽어온 모든 문장
    // 문장 길이별로 아직 사용되지 않은 문장들을 저장하는 딕셔너리
    private Dictionary<int, List<string>> availableWordsByLength = new Dictionary<int, List<string>>();

    private void Start()
    {
        if (phrasesFile != null)
        {
            // TextAsset의 내용을 줄 단위로 분리 (개행 문자 기준)
            string[] lines = phrasesFile.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            allWords.AddRange(lines);

            // 문장 길이별로 availableWordsByLength 초기화
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
            Debug.LogError("TextAsset examplesFile이 할당되지 않았습니다.");
        }

        testSentenceText.text = string.Empty;
        inputField.text = string.Empty;
    }

    // 외부에서 호출할 때 원하는 길이를 인자로 전달
    public void ShowNextSentence(int desiredLength)
    {
        ShowRandomSentence(desiredLength);
    }

    public string GetCurrentSentence()
    {
        return testSentenceText.text;
    }

    // 원하는 길이의 문장들 중 아직 출력되지 않은 문장을 랜덤 선택
    private void ShowRandomSentence(int desiredLength)
    {
        string nextWord = string.Empty;

        // 선택한 길이의 문장 리스트가 없거나 모두 사용되었다면 재설정
        if (!availableWordsByLength.ContainsKey(desiredLength) || availableWordsByLength[desiredLength].Count == 0)
        {
            List<string> wordsOfDesiredLength = allWords.Where(s => s.Length == desiredLength).ToList();
            // 현재 표시된 문장이 있다면 중복 제거
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
