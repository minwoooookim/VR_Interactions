using System;
using System.Collections;
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

    // 내부적으로 사용할 변수들
    private string imageName;
    private float startTime;
    private bool testStarted = false;

    void Start()
    {
        // 초기화 코드가 필요하면 추가
    }

    void Update()
    {
        // 필요에 따라 Update에서 로직 추가 가능
    }

    // 외부에서 시작 버튼 호출 시 사용
    public void OnStartButton()
    {
        // 이미 테스트가 진행중이면 중복 실행 방지
        if (!testStarted)
        {
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
        testStarted = true;
        // 현재 이미지 이름 저장
        imageName = tangramExampleDisplayer.GetCurrentImageName();
    }

    // 외부에서 완료 버튼 호출 시 사용
    public void OnCompleteButton()
    {
        if (!testStarted)
        {
            Debug.LogWarning("테스트가 시작되지 않았습니다.");
            return;
        }

        // 시간 측정 종료 및 경과 시간 계산
        float elapsedTime = Time.time - startTime;
        // 이미지 비우기 호출
        tangramExampleDisplayer.EmptyImage();
        // 결과 파일 생성 혹은 내용 추가
        WriteResultToFile(imageName, elapsedTime);
        // 테스트 상태 초기화 (필요 시)
        testStarted = false;
    }

    // 결과를 파일로 저장하는 함수
    private void WriteResultToFile(string imageName, float elapsedTime)
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

        // 파일명 구성
        string fileName = $"Player_{playerNumber.text}_Tangram_{toggleName}.txt";
        string filePath = Path.Combine(resultsFolder, fileName);

        // 기록할 내용: 이미지 이름과 시간 (초, 소수점 셋째자리)
        string contentToAppend = $"{imageName}\n{elapsedTime.ToString("F3")}";

        // 파일이 존재하면 개행 후 내용 추가, 아니면 새 파일 생성
        if (File.Exists(filePath))
        {
            File.AppendAllText(filePath, "\n" + contentToAppend);
        }
        else
        {
            File.WriteAllText(filePath, contentToAppend);
        }
    }
}
