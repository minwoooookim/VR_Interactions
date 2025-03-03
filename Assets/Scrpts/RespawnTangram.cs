using UnityEngine;
using Oculus.Interaction;

public class RespawnTangram : MonoBehaviour
{
    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    [Header("Tangram Parents")]
    [SerializeField] private GameObject tangramParent1;
    [SerializeField] private GameObject tangramParent2;

    

    // ������ �Ǵ� tangramParent1�� �ڽ� Transform ���� ����
    private Transform[] referenceChildTransforms;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private Rigidbody[] childRigidbodies; // �ڽ� ���� ������ Rigidbody

    private void Awake()
    {
        if (tangramParent1 == null)
        {
            Debug.LogWarning("Tangram Parent1�� �Ҵ���� �ʾҽ��ϴ�.", this);
            return;
        }

        // ��Ȱ�� ��ü�� �����Ͽ� �ڽĵ�(�ڱ� �ڽ� ����)�� ��� ������
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
            Debug.LogWarning("PokeInteractable�� �Ҵ���� �ʾҽ��ϴ�.", this);
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

        // ��Ȱ�� ������ �ڽĵ鵵 �����Ͽ� ������
        Transform[] children = tangramParent.GetComponentsInChildren<Transform>(true);

        // referenceChildTransforms �迭�� �ڽ� ���� �����ϴٴ� �����Ͽ� ������� �ʱ�ȭ
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
