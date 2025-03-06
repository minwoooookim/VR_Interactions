using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class CompleteTangramTestButton : MonoBehaviour
{
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private TangramTester tangramTester;

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
            tangramTester.OnCompleteButton();
        }
    }
}
