using UnityEngine;
using TMPro;

public class SetPlayerNumber : MonoBehaviour
{
    // TextMeshPro ������Ʈ ���� (����Ƽ �����Ϳ��� �Ҵ�)
    public TMP_Text playerNumber;

    void Start()
    {
        // DataHolder�� ����� ���� ���ڿ��� ��ȯ�Ͽ� ���
        playerNumber.text = DataHolder.playerNumber.ToString();
    }
}
