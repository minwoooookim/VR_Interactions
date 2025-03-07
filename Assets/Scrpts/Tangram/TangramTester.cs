using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TangramTester : MonoBehaviour
{
    [SerializeField] private TangramExampleDisplayer tangramExampleDisplayer;
    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private ToggleGroup InteractionSelector;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TangramRespawner tangramRespawner;

    // 내부적으로 사용할 변수들
    private bool isTestRunning = false;
    private const int repeatCount = 5; // 테스트 전체 반복 횟수
    private float sessionTotalTime = 0f; // 세션 동안 누적된 시간
    private string filePath = string.Empty; // 결과 파일 경로 (반복마다 동일하므로, 한 번 계산)

    private bool isCompleted = false; // 사용자가 칠교놀이를 완료 후 다음 버튼을 눌렀는지 여부

    // 외부에서 시작 버튼 호출 시 사용 (한번 눌리면 중복 입력 방지)
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
        string fileName = $"Player_{playerNum}_Tangram_{toggleName}.txt";
        filePath = Path.Combine(resultsPath, fileName);

        // 코루틴 시작
        StartCoroutine(TestRoutine());
    }

    private IEnumerator TestRoutine()
    {
        // 전체 테스트 반복: 3번 반복 후 종료
        for (int i = 0; i < repeatCount; i++)
        {
            // 3초 카운트다운 시작
            yield return StartCoroutine(CountdownCoroutine(3));

            tangramExampleDisplayer.ShowNextImage();

            // 시간 측정 시작
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // 현재 이미지명 저장
            string imageName = tangramExampleDisplayer.GetCurrentImageName();

            // 사용자가 다음 버튼을 누를 때까지 대기
            while (!isCompleted)
            {
                yield return null;
            }

            // 시간 측정 종료
            stopwatch.Stop();
            float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
            sessionTotalTime += elapsedSeconds;

            // 문장과 개별 소요 시간 기록 (Total Time 기록은 하지 않음)
            SaveTestResult(imageName, elapsedSeconds);

            isCompleted = !isCompleted;
            tangramExampleDisplayer.EmptyImage();
            tangramRespawner.RespawnTangram();
        }

        // 전체 테스트가 끝난 후 누적 시간(Total Time)만 별도로 기록
        SaveTotalTime(sessionTotalTime);

        // 테스트 종료 후 초기 상태로 복귀
        sessionTotalTime = 0f;
        isTestRunning = false;
        countdownText.text = "완료";
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = string.Empty;
    }

    public void SetCompleted()
    {
        isCompleted = true;
    }

    private void SaveTestResult(string imageName, float elapsedSeconds)
    {
        // 기록할 내용 구성 (elapsedSeconds는 소수점 셋째 자리까지 표현)
        string record = $"{imageName}\n{elapsedSeconds:F3}";

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
