using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshPro ��� ��

public class SceneChanger : MonoBehaviour
{
    // TMP_InputField ������Ʈ ���� (����Ƽ �����Ϳ��� �Ҵ�)
    public TMP_InputField tmpInputField;

    // "VR_Interactions" ������ ��ȯ�ϸ鼭 �Է°��� �����ϴ� �Լ�
    public void LoadVRInteractions()
    {
        int value = 0;
        // �Է¹��� �ؽ�Ʈ�� ������ �Ľ�
        if (int.TryParse(tmpInputField.text, out value))
        {
            DataHolder.playerNumber = value;
            SceneManager.LoadScene("VR_Interactions");
        }
        else
        {
            Debug.LogWarning("�Էµ� ���� ������ �ƴմϴ�.");
        }
    }
}
