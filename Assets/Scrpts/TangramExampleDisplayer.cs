using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TangramExampleDisplayer : MonoBehaviour
{
    // ĵ������ �ִ� �̹��� ������Ʈ�� �����մϴ�.
    public Image displayImage;

    // Tangram ������ �ִ� ��� ��������Ʈ�� ���� ����Ʈ
    private List<Sprite> allSprites = new List<Sprite>();
    // ���� �ֱ�(��)�� ���� ��������Ʈ��
    private List<Sprite> currentCycle = new List<Sprite>();

    void Start()
    {
        // Resources/Tangram ���� �� ��� ��������Ʈ �ε�
        allSprites.AddRange(Resources.LoadAll<Sprite>("Tangram"));

        if (allSprites.Count == 0)
        {
            Debug.LogWarning("Tangram ������ �̹����� �����ϴ�.");
            return;
        }

        // ���⸸ �ϰ�, �̹����� ǥ������ ����
        ShuffleCycle();
        displayImage.sprite = null; // �ʱ⿡�� �� ȭ��
    }

    public void OnPokeButton()
    {
        DisplayNextImage();
    }

    // ���� �̹����� ǥ���ϴ� �Լ�
    private void DisplayNextImage()
    {
        // ���� �ֱ� ���� �̹����� ������ �ٽ� ����
        if (currentCycle.Count == 0)
        {
            ShuffleCycle();
        }

        // ����Ʈ�� ù ��° �̹��� ��������
        Sprite nextSprite = currentCycle[0];
        // �ش� �̹��� �����Ͽ� �ߺ� ǥ�� ����
        currentCycle.RemoveAt(0);
        // �̹��� ������Ʈ�� ��������Ʈ ����
        displayImage.sprite = nextSprite;
    }

    // allSprites ����Ʈ�� ������� currentCycle ����Ʈ�� �����ϰ� ���� �Լ�
    private void ShuffleCycle()
    {
        // ��� ��������Ʈ�� ����Ʈ �ʱ�ȭ
        currentCycle = new List<Sprite>(allSprites);

        // Fisher-Yates ���� �˰��� ���
        for (int i = 0; i < currentCycle.Count; i++)
        {
            int randomIndex = Random.Range(i, currentCycle.Count);
            // ������
            Sprite temp = currentCycle[i];
            currentCycle[i] = currentCycle[randomIndex];
            currentCycle[randomIndex] = temp;
        }
    }
}
