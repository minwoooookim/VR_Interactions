using UnityEngine;
using TMPro;

public class SetPlayerNumber : MonoBehaviour
{
    // TextMeshPro 컴포넌트 참조 (유니티 에디터에서 할당)
    public TMP_Text playerNumber;

    void Start()
    {
        // DataHolder에 저장된 값을 문자열로 변환하여 출력
        playerNumber.text = DataHolder.playerNumber.ToString();
    }
}
