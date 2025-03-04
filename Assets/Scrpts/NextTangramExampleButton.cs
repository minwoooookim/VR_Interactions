using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class NextTangramExampleButton : MonoBehaviour
{
   
    [SerializeField] private PokeInteractable pokeInteractable;
    [SerializeField] private TangramExampleDisplayer tangramExampleDisplayer;

    private void OnDestroy()
    {
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        tangramExampleDisplayer.DisplayNextImage();
    }

}
