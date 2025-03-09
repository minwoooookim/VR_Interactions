using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshPro 사용 시

public class SceneChanger : MonoBehaviour
{
    // TMP_InputField 컴포넌트 참조 (유니티 에디터에서 할당)
    public TMP_InputField tmpInputField;

    // "VR_Interactions" 씬으로 전환하면서 입력값을 전달하는 함수
    public void LoadVRInteractions()
    {
        int value = 0;
        // 입력받은 텍스트를 정수로 파싱
        if (int.TryParse(tmpInputField.text, out value))
        {
            DataHolder.playerNumber = value;
            SceneManager.LoadScene("VR_Interactions");
        }
        else
        {
            Debug.LogWarning("입력된 값이 정수가 아닙니다.");
        }
    }
}
