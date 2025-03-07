using System;
using System.Collections;
using System.Collections.Generic;
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
    private string imageName;
    private float startTime;
    private bool testStarted = false;
    private int roundCount = 0; // 진행된 라운드 수 (최대 5회)

    // 외부에서 시작 버튼 호출 시 사용
    public void OnStartButton()
    {
        // 이미 테스트가 진행 중이거나 5회가 모두 완료되었으면 동작하지 않음
        if (!testStarted && roundCount < 5)
        {
            testStarted = true;
            StartCoroutine(CountdownCoroutine());
        }
    }

    // 3초 카운트다운 코루틴
    private IEnumerator CountdownCoroutine()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        // 카운트다운 종료 후 텍스트 비우기
        countdownText.text = "";
        // 다음 이미지를 표시하고 시간 측정 시작
        tangramExampleDisplayer.ShowNextImage();
        startTime = Time.time;
        // 현재 이미지 이름 저장
        imageName = tangramExampleDisplayer.GetCurrentImageName();
    }

    // 외부에서 완료(다음) 버튼 호출 시 사용
    public void OnNextButton()
    {
        if (!testStarted)
        {
            Debug.LogWarning("테스트가 시작되지 않았습니다.");
            return;
        }

        // 시간 측정 종료 및 경과 시간 계산
        float elapsedTime = Time.time - startTime;
        // 이미지 제거 및 새로운 타격 재배치
        tangramExampleDisplayer.EmptyImage();
        tangramRespawner.RespawnTangram();

        // 현재 라운드 증가
        roundCount++;

        // 5번째 라운드면 최종 기록(총합 포함)을 파일에 작성
        bool isFinalRound = (roundCount == 5);
        WriteResultToFile(imageName, elapsedTime, isFinalRound);

        // 만약 마지막 라운드가 아니라면 다시 다음 라운드 진행 가능하도록 testStarted 초기화
        testStarted = false;
    }

    // 결과를 파일로 저장하는 함수
    // isFinalRound가 true이면 기존 기록된 시간과 새 시간을 합산하여 Total Time 기록을 추가
    private void WriteResultToFile(string imageName, float elapsedTime, bool isFinalRound)
    {
        // 바탕화면의 results 폴더 경로 설정
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string resultsFolder = Path.Combine(desktopPath, "results");

        // 폴더가 없으면 생성
        if (!Directory.Exists(resultsFolder))
        {
            Directory.CreateDirectory(resultsFolder);
        }

        // InteractionSelector에서 현재 선택된 토글의 이름 가져오기
        Toggle selectedToggle = InteractionSelector.ActiveToggles().FirstOrDefault();
        string toggleName = selectedToggle != null ? selectedToggle.name : "None";

        // 파일명 구성 : Player_(playerNumber)_Keyboard_(토글 이름).txt
        string fileName = $"Player_{playerNumber.text}_Keyboard_{toggleName}.txt";
        string filePath = Path.Combine(resultsFolder, fileName);

        List<string> lines = new List<string>();

        // 파일이 존재하면 기존 내용 읽어오기
        if (File.Exists(filePath))
        {
            lines = File.ReadAllLines(filePath).ToList();
            // 만약 마지막 줄이 "Total Time"으로 시작하면 제거
            if (lines.Count > 0 && lines[lines.Count - 1].StartsWith("Total Time"))
            {
                lines.RemoveAt(lines.Count - 1);
            }
        }

        // 새로 기록할 내용 : 이미지 이름과 경과 시간 (초, 소수점 셋째자리)
        lines.Add(imageName);
        lines.Add(elapsedTime.ToString("F3"));

        // 마지막 라운드인 경우, 기존에 기록된 시간과 새 시간을 합산하여 Total Time 기록 추가
        if (isFinalRound)
        {
            float totalTime = 0f;
            foreach (string line in lines)
            {
                // 이미지 이름은 건너뛰고, 실수 변환 가능한 값만 합산
                if (float.TryParse(line, out float time))
                {
                    totalTime += time;
                }
            }
            lines.Add("Total Time: " + totalTime.ToString("F3"));
        }

        // 파일에 전체 내용 덮어쓰기
        File.WriteAllLines(filePath, lines);
    }

}
