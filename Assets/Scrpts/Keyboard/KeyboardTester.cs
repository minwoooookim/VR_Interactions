using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardTester : MonoBehaviour
{
    [SerializeField] private SentenceDisplayer sentenceDisplayer;
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private ToggleGroup InteractionSelector;
    [SerializeField] private TMP_Text countdownText;

    private bool isTestRunning = false; // 시작버튼 중복 입력 방지를 위한 flag 
    private const int repeatCount = 3; // 테스트 전체 반복 횟수
    private float sessionTotalTime = 0f; // 세션 동안 누적된 시간
    private string filePath = string.Empty; // 결과 파일 경로 (반복마다 동일하므로, 한 번 계산)

    // 외부 버튼에서 호출하는 시작 함수
    public void StartTest()
    {
        if (isTestRunning)
            return; // 이미 진행 중이면 중복 실행 방지

        isTestRunning = true;

        // 파일 경로 계산
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string resultsPath = Path.Combine(desktopPath, "results");
        if (!Directory.Exists(resultsPath))
        {
            Directory.CreateDirectory(resultsPath);
        }
        string playerNum = playerNumber.text;
        Toggle activeToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
        string toggleName = activeToggle != null ? activeToggle.name : "Unknown";
        string fileName = $"Player_{playerNum}_Keyboard_{toggleName}.txt";
        filePath = Path.Combine(resultsPath, fileName);

        // 코루틴 시작
        StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine()
    {
        // 3초 카운트다운 시작
        yield return StartCoroutine(CountdownCoroutine(3));

        // 전체 테스트 반복: 3번 반복 후 종료
        for (int i = 0; i < repeatCount; i++)
        {
            // 첫 번째 호출 시 ShowNextWord(19)로 시작, 이후 20, 21 순으로 호출
            sentenceDisplayer.ShowNextSentence(19 + i);

            // 시간 측정 시작
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // 현재 문장 저장
            string currentSentence = sentenceDisplayer.GetCurrentSentence();

            // 사용자가 입력한 문장과 testSentenceText의 문장이 같아질 때까지 대기
            while (sentenceDisplayer.inputField.text != sentenceDisplayer.testSentenceText.text)
            {
                yield return null;
            }

            // 시간 측정 종료
            stopwatch.Stop();
            float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
            sessionTotalTime += elapsedSeconds;

            // 문장과 개별 소요 시간 기록 (Total Time 기록은 하지 않음)
            SaveSentenceResult(currentSentence, elapsedSeconds);

            // ※ 대기시간 없이 바로 다음 문장 진행 (대기 코루틴 제거)
        }

        // 전체 테스트가 끝난 후 누적 시간(Total Time)만 별도로 기록
        SaveTotalTime(sessionTotalTime);

        // 테스트 종료 후 초기 상태로 복귀
        sentenceDisplayer.testSentenceText.text = string.Empty;
        sentenceDisplayer.inputField.text = string.Empty;
        sessionTotalTime = 0f;
        isTestRunning = false;
        countdownText.text = "완료";
    }

    // 3초 카운트다운 코루틴 (초 단위로 countdownText에 표시)
    private IEnumerator CountdownCoroutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = string.Empty;
    }

    // 각 문장 결과를 저장하는 함수 (문장과 해당 소요시간만 기록)
    private void SaveSentenceResult(string sentence, float elapsedSeconds)
    {
        // 기록할 내용 구성 (elapsedSeconds는 소수점 셋째 자리까지 표현)
        string record = $"{sentence}\n{elapsedSeconds:F3}";

        // 파일이 존재하면 개행 후 추가, 없으면 새로 생성
        if (File.Exists(filePath))
        {
            File.AppendAllText(filePath, Environment.NewLine + record);
        }
        else
        {
            File.WriteAllText(filePath, record);
        }
    }

    // 누적 시간(Total Time)을 마지막에 저장하는 함수
    private void SaveTotalTime(float totalTime)
    {
        string record = Environment.NewLine + $"Total Time: {totalTime:F3}";
        File.AppendAllText(filePath, record);
    }
}
