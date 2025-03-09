using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using VideoplayerTesterNamespace;

public class StartVideoplayerTestButton : MonoBehaviour
{
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private VideoplayerTester videoplayerTester;

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
        // PokeInteractable이 누른 상태(Select)가 되면 실행
        if (args.NewState == InteractableState.Select)
        {
            videoplayerTester.StartTest();
        }
    }
}
