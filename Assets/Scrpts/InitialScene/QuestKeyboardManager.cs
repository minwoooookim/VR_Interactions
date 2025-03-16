using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestKeyboardManager : MonoBehaviour
{
    // 인스펙터에서 연결할 인풋필드
    public TMP_InputField targetInputField;

    // 키보드 인스턴스를 저장할 변수
    private TouchScreenKeyboard overlayKeyboard;

    void Start()
    {
        // 인풋필드가 선택될 때 코루틴을 통해 OpenKeyboardWithDelay() 호출
        targetInputField.onSelect.AddListener(delegate { StartCoroutine(OpenKeyboardWithDelay()); });
    }

    // 키보드 호출 코루틴 (한 프레임 대기 후 실행)
    IEnumerator OpenKeyboardWithDelay()
    {
        yield return null; // UI 업데이트가 완료된 후 한 프레임 대기

        // 기존에 열려있는 키보드가 있으면 닫기(옵션)
        if (overlayKeyboard != null)
        {
            overlayKeyboard.active = false;
            overlayKeyboard = null;
        }

        // 기본 키보드 타입으로 키보드 열기
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        // 키보드가 활성화되어 있다면, 입력된 텍스트를 인풋필드에 실시간 반영
        if (overlayKeyboard != null)
        {
            targetInputField.text = overlayKeyboard.text;

            // 키보드가 닫힌 경우 처리
            if (overlayKeyboard.status == TouchScreenKeyboard.Status.Done ||
                overlayKeyboard.status == TouchScreenKeyboard.Status.Canceled)
            {
                overlayKeyboard = null;
            }
        }
    }
}
