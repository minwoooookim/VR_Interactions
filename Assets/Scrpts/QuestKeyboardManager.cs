using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestKeyboardManager : MonoBehaviour
{
    // �ν����Ϳ��� ������ ��ǲ�ʵ�
    public TMP_InputField targetInputField;

    // Ű���� �ν��Ͻ��� ������ ����
    private TouchScreenKeyboard overlayKeyboard;

    void Start()
    {
        // ��ǲ�ʵ尡 ���õ� �� OpenKeyboard()�� ȣ��ǵ��� ������ ���
        targetInputField.onSelect.AddListener(delegate { OpenKeyboard(); });
    }

    // Ű���� ȣ�� �޼���
    void OpenKeyboard()
    {
        // ������ �����ִ� Ű���尡 ������ �ݱ�(�ɼ�)
        if (overlayKeyboard != null)
        {
            overlayKeyboard.active = false;
            overlayKeyboard = null;
        }
        // �⺻ Ű���� Ÿ������ Ű���� ����
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        // Ű���尡 Ȱ��ȭ�Ǿ� �ִٸ�, �Էµ� �ؽ�Ʈ�� ��ǲ�ʵ忡 �ǽð� �ݿ�
        if (overlayKeyboard != null)
        {
            targetInputField.text = overlayKeyboard.text;

            // �ʿ� ��, Ű���尡 ���� ��쿡 ���� ó���� �߰��� �� ����
            if (overlayKeyboard.status == TouchScreenKeyboard.Status.Done ||
                overlayKeyboard.status == TouchScreenKeyboard.Status.Canceled)
            {
                overlayKeyboard = null;
            }
        }
    }
}
