using UnityEngine;
using Oculus.Interaction;

public class RespawnTangram : MonoBehaviour
{
    [Header("Tangram Parent")]
    [SerializeField] private GameObject tangramParent;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    // �ڽ� Transform���� ������ �迭
    private Transform[] childTransforms;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private Rigidbody[] childRigidbodies; // ���� ������ Rigidbody�� �پ��ִٸ�

    private void Awake()
    {
        if (tangramParent == null)
        {
            Debug.LogWarning("Tangram �θ� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.", this);
            return;
        }

        // �ڽĵ�(�ڱ� �ڽ� ����)�� ��� ��������
        childTransforms = tangramParent.GetComponentsInChildren<Transform>();

        // �迭 ũ�⸸ŭ �ʱ�ȭ
        originalPositions = new Vector3[childTransforms.Length];
        originalRotations = new Quaternion[childTransforms.Length];
        childRigidbodies = new Rigidbody[childTransforms.Length];

        // 0��°�� �θ� �ڽ��̹Ƿ�, ���� �������� 1��°������ ���� ����
        for (int i = 0; i < childTransforms.Length; i++)
        {
            // �ʱ� local ��ġ/ȸ�� ����
            originalPositions[i] = childTransforms[i].localPosition;
            originalRotations[i] = childTransforms[i].localRotation;

            // Ȥ�� �ڽ� ������ Rigidbody�� �ִٸ� ���� ����
            childRigidbodies[i] = childTransforms[i].GetComponent<Rigidbody>();
        }

        // PokeInteractable ����
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
        // ���� ��ȭ �ø��� ����
        Respawn();
    }

    private void Respawn()
    {
        if (childTransforms == null) return;

        // �ڽĵ��� ��ġ/ȸ�� �ʱ�ȭ
        for (int i = 0; i < childTransforms.Length; i++)
        {
            childTransforms[i].localPosition = originalPositions[i];
            childTransforms[i].localRotation = originalRotations[i];

            // ���� ���µ� �ʱ�ȭ
            if (childRigidbodies[i] != null)
            {
                childRigidbodies[i].velocity = Vector3.zero;
                childRigidbodies[i].angularVelocity = Vector3.zero;
            }
        }
    }
}
