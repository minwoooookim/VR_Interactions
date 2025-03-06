using UnityEngine;
using Oculus.Interaction; // Meta(구 Oculus) Interaction SDK 네임스페이스

public class KeyboardToggleButton : MonoBehaviour
{
    [Header("Keyboard Objects")]
    [SerializeField] private GameObject QWERTYKeyboard;
    [SerializeField] private GameObject WristKeyboard;

    [Header("Raycast Objects")]
    [SerializeField] private GameObject leftRayCaster;
    [SerializeField] private GameObject rightRayCaster;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    // WristKeyboard 현재 표시 상태
    private bool isWristKeyboardActive = false;

    // ================================
    //  버튼 색상 전환을 위한 필드 추가
    // ================================
    [Header("Button Color Settings")]
    [Tooltip("버튼의 MeshRenderer 또는 SpriteRenderer(2D UI) 등을 할당")]
    [SerializeField] private Renderer buttonRenderer;

    [Tooltip("WristKeyboard가 꺼져있을 때(기본) 버튼 색상")]
    [SerializeField] private Color colorKeyboardOff = Color.gray;

    [Tooltip("WristKeyboard가 켜져있을 때 버튼 색상")]
    [SerializeField] private Color colorKeyboardOn = Color.green;

    private void Awake()
    {
        // PokeInteractable이 할당되었는지 확인 후 이벤트 등록
        if (pokeInteractable != null)
        {
            // 버튼(포킹) 상태가 바뀔 때마다 콜백 실행
            pokeInteractable.WhenStateChanged += HandleStateChanged;
        }
        else
        {
            Debug.LogWarning("PokeInteractable이 할당되지 않았습니다.", this);
        }

        // 씬 시작 시 초기 색상 설정
        UpdateButtonColor();
    }

    private void OnDestroy()
    {
        // 씬이 언로드되거나 오브젝트 파괴 시 이벤트 해제
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    /// <summary>
    /// PokeInteractable 상태가 변할 때 호출되는 콜백.
    /// InteractableState.Select로 바뀌면 '버튼이 눌렸다'고 간주한다.
    /// </summary>
    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // 새 상태가 Select(= 눌림)일 때만 토글 로직 수행
        if (args.NewState == InteractableState.Select)
        {
            ToggleKeyboards();
        }
    }

    /// <summary>
    /// 키보드 전환 및 RayCaster 활성/비활성 제어
    /// </summary>
    private void ToggleKeyboards()
    {
        // WristKeyboard 활성 상태를 토글
        isWristKeyboardActive = !isWristKeyboardActive;
        WristKeyboard.SetActive(isWristKeyboardActive);

        // QWERTYKeyboard는 반대 상태로
        QWERTYKeyboard.SetActive(!isWristKeyboardActive);

        // WristKeyboard가 켜져 있으면 양손 RayCaster 비활성화
        bool shouldEnableRays = !isWristKeyboardActive;
        leftRayCaster.SetActive(shouldEnableRays);
        rightRayCaster.SetActive(shouldEnableRays);

        // 버튼 색상 업데이트
        UpdateButtonColor();
    }

    /// <summary>
    /// 버튼 색상을 현재 키보드 상태에 맞춰 변경
    /// </summary>
    private void UpdateButtonColor()
    {
        if (buttonRenderer == null) return;

        // WristKeyboard가 활성 상태일 때는 colorKeyboardOn, 아닐 때는 colorKeyboardOff
        buttonRenderer.material.color = isWristKeyboardActive ? colorKeyboardOn : colorKeyboardOff;
    }
}
