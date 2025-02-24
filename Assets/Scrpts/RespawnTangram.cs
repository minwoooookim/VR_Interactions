using UnityEngine;
using Oculus.Interaction;

public class RespawnTangram : MonoBehaviour
{
    [Header("Tangram Parent")]
    [SerializeField] private GameObject tangramParent;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    // 자식 Transform들을 저장할 배열
    private Transform[] childTransforms;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private Rigidbody[] childRigidbodies; // 조각 각각에 Rigidbody가 붙어있다면

    private void Awake()
    {
        if (tangramParent == null)
        {
            Debug.LogWarning("Tangram 부모 오브젝트가 할당되지 않았습니다.", this);
            return;
        }

        // 자식들(자기 자신 포함)을 모두 가져오기
        childTransforms = tangramParent.GetComponentsInChildren<Transform>();

        // 배열 크기만큼 초기화
        originalPositions = new Vector3[childTransforms.Length];
        originalRotations = new Quaternion[childTransforms.Length];
        childRigidbodies = new Rigidbody[childTransforms.Length];

        // 0번째는 부모 자신이므로, 실제 조각들은 1번째부터일 수도 있음
        for (int i = 0; i < childTransforms.Length; i++)
        {
            // 초기 local 위치/회전 저장
            originalPositions[i] = childTransforms[i].localPosition;
            originalRotations[i] = childTransforms[i].localRotation;

            // 혹시 자식 조각에 Rigidbody가 있다면 따로 저장
            childRigidbodies[i] = childTransforms[i].GetComponent<Rigidbody>();
        }

        // PokeInteractable 연결
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
        // 상태 변화 시마다 리셋
        Respawn();
    }

    private void Respawn()
    {
        if (childTransforms == null) return;

        // 자식들의 위치/회전 초기화
        for (int i = 0; i < childTransforms.Length; i++)
        {
            childTransforms[i].localPosition = originalPositions[i];
            childTransforms[i].localRotation = originalRotations[i];

            // 물리 상태도 초기화
            if (childRigidbodies[i] != null)
            {
                childRigidbodies[i].velocity = Vector3.zero;
                childRigidbodies[i].angularVelocity = Vector3.zero;
            }
        }
    }
}
