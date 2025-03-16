using UnityEngine;
using Oculus.Interaction;
using System.Collections.Generic;

public class TangramRespawner : MonoBehaviour
{
    [Header("Tangram Parent")]
    [SerializeField] private GameObject tangramParent;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    [Header("Respawn Settings")]
    [SerializeField]
    [Tooltip("������ �� ���� ������ ���� ������ٵ� kinematic���� ó���Ͽ� �浹 ������ �����մϴ�.")]
    private int _sleepFrames = 0;

    // �ڽ� ������Ʈ(�θ� tangramParent ����)�� �ʱ� ���¸� ����
    private Transform[] childTransforms;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private Vector3[] originalScales;
    private Rigidbody[] childRigidbodies;
    private int[] sleepCountDowns;

    private void Awake()
    {
        if (tangramParent == null)
        {
            Debug.LogWarning("Tangram �θ� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.", this);
            return;
        }

        // tangramParent�� �ڽĵ鸸 �������� (�ڱ� �ڽ��� ����)
        Transform[] allTransforms = tangramParent.GetComponentsInChildren<Transform>();
        List<Transform> childrenList = new List<Transform>();
        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i] != tangramParent.transform)
            {
                childrenList.Add(allTransforms[i]);
            }
        }
        childTransforms = childrenList.ToArray();

        // �迭 ũ�⸸ŭ �ʱ�ȭ
        int count = childTransforms.Length;
        originalPositions = new Vector3[count];
        originalRotations = new Quaternion[count];
        originalScales = new Vector3[count];
        childRigidbodies = new Rigidbody[count];
        sleepCountDowns = new int[count];

        // �� �ڽ��� �ʱ� ����(���� ��ġ, ���� ȸ��, ���� ������) ����
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = childTransforms[i].position;
            originalRotations[i] = childTransforms[i].rotation;
            originalScales[i] = childTransforms[i].localScale;
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
        // ���� ��ȭ �ø��� ������ ����
        Respawn();
    }

    public void RespawnTangram()
    {
        Respawn();
    }

    private void Respawn()
    {
        if (childTransforms == null) return;

        int count = childTransforms.Length;

        // ����, ������ٵ� �ִٸ� ���� ������ ������ �ʿ��� ��� ��� kinematic���� ��ȯ
        for (int i = 0; i < count; i++)
        {
            if (childRigidbodies[i] != null && _sleepFrames > 0)
            {
                childRigidbodies[i].isKinematic = true;
            }
        }

        // �� �ڽ� ������Ʈ�� transform�� ���� ���� �ʱ�ȭ
        for (int i = 0; i < count; i++)
        {
            childTransforms[i].position = originalPositions[i];
            childTransforms[i].rotation = originalRotations[i];
            childTransforms[i].localScale = originalScales[i];

            if (childRigidbodies[i] != null)
            {
                childRigidbodies[i].velocity = Vector3.zero;
                childRigidbodies[i].angularVelocity = Vector3.zero;

                if (_sleepFrames > 0)
                {
                    sleepCountDowns[i] = _sleepFrames;
                }
            }
        }

        // ���� ������ ��ȯ ������ ������ ������Ʈ
        Physics.SyncTransforms();
    }

    private void FixedUpdate()
    {
        // �� �ڽ� ������ٵ��� ���� ī��Ʈ�ٿ��� ó��
        int count = childRigidbodies.Length;
        for (int i = 0; i < count; i++)
        {
            if (childRigidbodies[i] != null && sleepCountDowns[i] > 0)
            {
                sleepCountDowns[i]--;
                if (sleepCountDowns[i] == 0)
                {
                    childRigidbodies[i].isKinematic = false;
                }
            }
        }
    }
}
