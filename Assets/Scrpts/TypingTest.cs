using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;      // "Test Word"를 표시할 TextMeshPro 텍스트
    [SerializeField] private TMP_InputField inputField;  // 사용자가 타이핑하는 TextMeshPro InputField

    private List<string> phrases = new List<string>();   // phrases.txt에서 읽어온 문장 목록

    private void Start()
    {
        // 1. phrases.txt 파일에서 문장들 읽어오기
        string filePath = Path.Combine(Application.dataPath, "phrases.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            phrases.AddRange(lines);
        }
        else
        {
            Debug.LogError($"phrases.txt 파일을 찾을 수 없습니다. 경로: {filePath}");
        }

        // 문장이 하나 이상 있으면 첫 번째 문장 표시
        if (phrases.Count > 0)
        {
            ShowRandomPhrase();
        }
    }

    private void Update()
    {
        // Test Word의 내용과 InputField의 내용이 일치하면
        if (inputField.text == testWordText.text)
        {
            // 다음 문장 표시
            ShowRandomPhrase();

            // InputField 내용 삭제(초기화)
            inputField.text = string.Empty;
        }
    }

    // 랜덤하게 문장을 골라서 표시하는 메서드
    private void ShowRandomPhrase()
    {
        int randomIndex = Random.Range(0, phrases.Count);
        testWordText.text = phrases[randomIndex];
    }
}
