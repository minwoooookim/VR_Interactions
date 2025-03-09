using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using VideoplayerTesterNamespace;

public class NextExampleButton : MonoBehaviour
{
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private VideoplayerTester Tester;

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
            Tester.SetCompleted();
        }
    }
}
