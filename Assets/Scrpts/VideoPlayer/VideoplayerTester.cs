using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoPlayerControlScript;

namespace VideoplayerTesterNamespace
{
    public class VideoplayerTester : MonoBehaviour
    {
        [SerializeField] private VideoExampleDisplayer videoExampleDisplayer;
        [SerializeField] private VideoPlayerControls videoPlayerControls;
        [SerializeField] private TMP_Text playerNumber;
        [SerializeField] private ToggleGroup InteractionSelector;
        [SerializeField] private TMP_Text countdownText;

        [Header("Handle Display")]
        [SerializeField] private TMP_Text videoHandleDisplay;
        [SerializeField] private TMP_Text audioHandleDisplay;
        [SerializeField] private TMP_Text brightnessHandleDisplay;

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

            // 플레이어 번호를 사용하여 폴더명 설정
            string playerNum = playerNumber.text;
            string folderName = $"results_{playerNum}";
            string resultsPath = Path.Combine(desktopPath, folderName);

            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }

            Toggle activeToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
            string toggleName = activeToggle != null ? activeToggle.name : "Unknown";
            string fileName = $"Player_{playerNum}_Videoplayer_{toggleName}.txt";
            filePath = Path.Combine(resultsPath, fileName);

            // 코루틴 시작
            StartCoroutine(TestRoutine());
        }

        private IEnumerator TestRoutine()
        {
            // 전체 테스트 반복: 3번 반복 후 종료
            for (int i = 0; i < repeatCount; i++)
            {
                videoExampleDisplayer.PrintReady();

                // 3초 카운트다운 시작
                yield return StartCoroutine(CountdownCoroutine(3));

                videoExampleDisplayer.ShowNextExample();

                // 시간 측정 시작
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // 현재 예시 내용 저장
                string exampleContent = videoExampleDisplayer.GetCurrentExample();

                // 사용자가 태스크를 완료할 때까지 대기
                while (!isCompleted)
                {
                    yield return null;
                }

                // 시간 측정 종료
                stopwatch.Stop();
                float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
                sessionTotalTime += elapsedSeconds;

                // 문장과 개별 소요 시간 기록 (Total Time 기록은 하지 않음)
                SaveTestResult(exampleContent, elapsedSeconds);

                isCompleted = !isCompleted;

                // 비디오 플레이어 초기화
                videoPlayerControls.ResetPlayer();
            }

            // 전체 테스트가 끝난 후 누적 시간(Total Time)만 별도로 기록
            SaveTotalTime(sessionTotalTime);

            // 테스트 종료 후 초기 상태로 복귀
            sessionTotalTime = 0f;
            isTestRunning = false;
            videoExampleDisplayer.PrintCompleted();
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

        public void CheckIsCompleted()
        {
            // 현재 예시 텍스트를 가져옵니다.
            string exampleText = videoExampleDisplayer.GetCurrentExample();
            if (string.IsNullOrEmpty(exampleText))
                return;

            // 줄바꿈을 기준으로 분리합니다.
            string[] lines = exampleText.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            // 예시 형식은 최소 4줄이어야 합니다.
            if (lines.Length < 4)
                return;

            // lines[0]은 예시 이름(예: Example3)이고, 나머지는 "레이블: 값" 형식입니다.
            // 각각의 라인에서 공백 제거 후 비교합니다.
            string expectedTime = lines[1].Trim();       // "시간: 0:00"
            string expectedAudio = lines[2].Trim();        // "음량: 0"
            string expectedBrightness = lines[3].Trim();   // "밝기: 0"

            // handle의 텍스트도 공백 제거 후 비교
            if (videoHandleDisplay.text.Trim() == expectedTime &&
                audioHandleDisplay.text.Trim() == expectedAudio &&
                brightnessHandleDisplay.text.Trim() == expectedBrightness)
            {
                SetCompleted();
            }
        }


        private void SaveTestResult(string exampleContent, float elapsedSeconds)
        {
            // 기록할 내용 구성 (elapsedSeconds는 소수점 셋째 자리까지 표현)
            string record = $"{exampleContent}\n{elapsedSeconds:F3}";

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
}