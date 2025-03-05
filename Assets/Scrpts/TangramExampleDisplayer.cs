using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TangramExampleDisplayer : MonoBehaviour
{
    // 캔버스에 있는 이미지 컴포넌트를 연결합니다.
    public Image displayImage;

    // Tangram 폴더에 있는 모든 스프라이트를 담을 리스트
    private List<Sprite> allSprites = new List<Sprite>();
    // 현재 주기(셋)에 남은 스프라이트들
    private List<Sprite> currentCycle = new List<Sprite>();

    void Start()
    {
        // Resources/Tangram 폴더 내 모든 스프라이트 로드
        allSprites.AddRange(Resources.LoadAll<Sprite>("Tangram"));

        if (allSprites.Count == 0)
        {
            Debug.LogWarning("Tangram 폴더에 이미지가 없습니다.");
            return;
        }

        // 섞기만 하고, 이미지는 표시하지 않음
        ShuffleCycle();
        displayImage.sprite = null; // 초기에는 빈 화면
    }

    public void OnPokeButton()
    {
        DisplayNextImage();
    }

    // 다음 이미지를 표시하는 함수
    private void DisplayNextImage()
    {
        // 현재 주기 내에 이미지가 없으면 다시 섞음
        if (currentCycle.Count == 0)
        {
            ShuffleCycle();
        }

        // 리스트의 첫 번째 이미지 가져오기
        Sprite nextSprite = currentCycle[0];
        // 해당 이미지 제거하여 중복 표시 방지
        currentCycle.RemoveAt(0);
        // 이미지 컴포넌트에 스프라이트 설정
        displayImage.sprite = nextSprite;
    }

    // allSprites 리스트를 기반으로 currentCycle 리스트를 랜덤하게 섞는 함수
    private void ShuffleCycle()
    {
        // 모든 스프라이트로 리스트 초기화
        currentCycle = new List<Sprite>(allSprites);

        // Fisher-Yates 셔플 알고리즘 사용
        for (int i = 0; i < currentCycle.Count; i++)
        {
            int randomIndex = Random.Range(i, currentCycle.Count);
            // 스와핑
            Sprite temp = currentCycle[i];
            currentCycle[i] = currentCycle[randomIndex];
            currentCycle[randomIndex] = temp;
        }
    }
}
