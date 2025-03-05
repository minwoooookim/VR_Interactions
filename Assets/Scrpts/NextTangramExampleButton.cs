using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class NextTangramExampleButton : MonoBehaviour
{
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private TangramExampleDisplayer tangramExampleDisplayer;

    private void Awake()
    {
        // pokeInteractable이 할당되어 있으면 상태 변경 이벤트를 구독
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged += HandleStateChanged;
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // PokeInteractable이 누른 상태(Select)가 되면 TangramExampleDisplayer의 OnPokeButton() 호출
        if (args.NewState == InteractableState.Select)
        {
            tangramExampleDisplayer.OnPokeButton();
        }
    }
}
