using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class TypingTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text testWordText;      // "Test Word"�� ǥ���� TextMeshPro �ؽ�Ʈ
    [SerializeField] private TMP_InputField inputField;  // ����ڰ� Ÿ�����ϴ� TextMeshPro InputField

    private List<string> phrases = new List<string>();   // phrases.txt���� �о�� ���� ���

    private void Start()
    {
        // 1. phrases.txt ���Ͽ��� ����� �о����
        string filePath = Path.Combine(Application.dataPath, "phrases.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            phrases.AddRange(lines);
        }
        else
        {
            Debug.LogError($"phrases.txt ������ ã�� �� �����ϴ�. ���: {filePath}");
        }

        // ������ �ϳ� �̻� ������ ù ��° ���� ǥ��
        if (phrases.Count > 0)
        {
            ShowRandomPhrase();
        }
    }

    private void Update()
    {
        // Test Word�� ����� InputField�� ������ ��ġ�ϸ�
        if (inputField.text == testWordText.text)
        {
            // ���� ���� ǥ��
            ShowRandomPhrase();

            // InputField ���� ����(�ʱ�ȭ)
            inputField.text = string.Empty;
        }
    }

    // �����ϰ� ������ ��� ǥ���ϴ� �޼���
    private void ShowRandomPhrase()
    {
        int randomIndex = Random.Range(0, phrases.Count);
        testWordText.text = phrases[randomIndex];
    }
}
