using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class VideoExampleDisplayer : MonoBehaviour
{
    // 캔버스에 있는 텍스트 컴포넌트를 연결합니다.
    public TextMeshProUGUI videoExamplesText;
    // videoexamples_for_unity.txt 파일을 연결합니다. (Resources 폴더 혹은 에디터에서 직접 할당)
    public TextAsset examplesFile;

    // 파일에서 불러온 예시들을 저장할 리스트
    private List<string> availableExamples = new List<string>();
    private string currentExample;

    void Start()
    {
        // 파일이 할당되어 있으면 내용을 파싱합니다.
        if (examplesFile != null)
        {
            // 예시들은 빈 줄(혹은 \r\n\r\n)을 기준으로 구분되어 있다고 가정합니다.
            string[] examples = Regex.Split(examplesFile.text, @"\r?\n\r?\n");
            foreach (string example in examples)
            {
                // 앞뒤 공백 제거 후 리스트에 추가
                if (!string.IsNullOrWhiteSpace(example))
                    availableExamples.Add(example.Trim());
            }
        }
        else
        {
            Debug.LogError("예시 파일이 할당되지 않았습니다!");
        }
    }

    public void ShowNextExample()
    {
        DisplayNextExample();
    }

    public string GetCurrentExample()
    {
        if (videoExamplesText != null && !string.IsNullOrEmpty(videoExamplesText.text))
        {
            return currentExample;
        }
        else
        {
            return "현재 표시된 예시가 없습니다.";
        }
    }

    public void PrintReady()
    {
        videoExamplesText.text = "준비";
    }
    public void PrintCompleted()
    {
        videoExamplesText.text = "완료";
    }

    // 다음 예시를 랜덤하게 표시하는 함수
    private void DisplayNextExample()
    {
        if (availableExamples.Count > 0)
        {
            // 랜덤 인덱스 선택
            int randomIndex = Random.Range(0, availableExamples.Count);
            string nextExample = availableExamples[randomIndex];

            currentExample = nextExample;

            // 줄 단위로 분리하고 첫 줄을 제외한 나머지 줄들을 결합합니다.
            string[] lines = nextExample.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);
            videoExamplesText.text = lines.Length > 1 ? string.Join("\n", lines.Skip(1)) : nextExample;

            availableExamples.RemoveAt(randomIndex);
        }
        else
        {
            // 더 이상 출력할 예시가 없으면 "완료" 출력
            PrintCompleted();
        }
    }
}
