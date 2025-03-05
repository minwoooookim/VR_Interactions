using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class NextTangramExampleButton : MonoBehaviour
{
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private TangramExampleDisplayer tangramExampleDisplayer;

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
        // PokeInteractable�� ���� ����(Select)�� �Ǹ� TangramExampleDisplayer�� OnPokeButton() ȣ��
        if (args.NewState == InteractableState.Select)
        {
            tangramExampleDisplayer.OnPokeButton();
        }
    }
}
