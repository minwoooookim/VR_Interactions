using UnityEngine;
using Oculus.Interaction;

public class RespawnTangram : MonoBehaviour
{
    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    [Header("Tangram Parents")]
    [SerializeField] private GameObject tangramParent1;
    [SerializeField] private GameObject tangramParent2;

    

    // 기준이 되는 tangramParent1의 자식 Transform 정보 저장
    private Transform[] referenceChildTransforms;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private Rigidbody[] childRigidbodies; // 자식 조각 각각의 Rigidbody

    private void Awake()
    {
        if (tangramParent1 == null)
        {
            Debug.LogWarning("Tangram Parent1이 할당되지 않았습니다.", this);
            return;
        }

        // 비활성 객체도 포함하여 자식들(자기 자신 포함)을 모두 가져옴
        referenceChildTransforms = tangramParent1.GetComponentsInChildren<Transform>(true);

        int count = referenceChildTransforms.Length;
        originalPositions = new Vector3[count];
        originalRotations = new Quaternion[count];
        childRigidbodies = new Rigidbody[count];

        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = referenceChildTransforms[i].localPosition;
            originalRotations[i] = referenceChildTransforms[i].localRotation;
            childRigidbodies[i] = referenceChildTransforms[i].GetComponent<Rigidbody>();
        }

        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged += HandleStateChanged;
        }
        else
        {
            Debug.LogWarning("PokeInteractable이 할당되지 않았습니다.", this);
        }
    }

    private void OnDestroy()
    {
        if (pokeInteractable != null)
        {
            pokeInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        Respawn();
    }

    private void Respawn()
    {
        ResetTangram(tangramParent1);
        ResetTangram(tangramParent2);
    }

    private void ResetTangram(GameObject tangramParent)
    {
        if (tangramParent == null) return;

        // 비활성 상태의 자식들도 포함하여 가져옴
        Transform[] children = tangramParent.GetComponentsInChildren<Transform>(true);

        // referenceChildTransforms 배열과 자식 수가 동일하다는 가정하에 순서대로 초기화
        for (int i = 0; i < children.Length && i < originalPositions.Length; i++)
        {
            children[i].localPosition = originalPositions[i];
            children[i].localRotation = originalRotations[i];

            if (childRigidbodies[i] != null)
            {
                childRigidbodies[i].velocity = Vector3.zero;
                childRigidbodies[i].angularVelocity = Vector3.zero;
            }
        }
    }
}
