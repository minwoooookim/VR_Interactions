using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestKeyboardManager : MonoBehaviour
{
    // 인스펙터에서 연결할 인풋필드
    public TMP_InputField targetInputField;

    // 키보드 인스턴스를 저장할 변수
    private TouchScreenKeyboard overlayKeyboard;

    void Start()
    {
        // 인풋필드가 선택될 때 OpenKeyboard()가 호출되도록 리스너 등록
        targetInputField.onSelect.AddListener(delegate { OpenKeyboard(); });
    }

    // 키보드 호출 메서드
    void OpenKeyboard()
    {
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

            // 필요 시, 키보드가 닫힌 경우에 대한 처리도 추가할 수 있음
            if (overlayKeyboard.status == TouchScreenKeyboard.Status.Done ||
                overlayKeyboard.status == TouchScreenKeyboard.Status.Canceled)
            {
                overlayKeyboard = null;
            }
        }
    }
}
