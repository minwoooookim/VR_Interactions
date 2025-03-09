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
        // pokeInteractable�� �Ҵ�Ǿ� ������ ���� ���� �̺�Ʈ�� ����
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged += HandleStateChanged;
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� ����
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        // PokeInteractable�� ���� ����(Select)�� �Ǹ� ����
        if (args.NewState == InteractableState.Select)
        {
            videoplayerTester.StartTest();
        }
    }
}
