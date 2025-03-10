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
    [Tooltip("리스폰 후 일정 프레임 동안 리지드바디를 kinematic으로 처리하여 충돌 문제를 방지합니다.")]
    private int _sleepFrames = 0;

    // 자식 오브젝트(부모 tangramParent 제외)의 초기 상태를 저장
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
            Debug.LogWarning("Tangram 부모 오브젝트가 할당되지 않았습니다.", this);
            return;
        }

        // tangramParent의 자식들만 가져오기 (자기 자신은 제외)
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

        // 배열 크기만큼 초기화
        int count = childTransforms.Length;
        originalPositions = new Vector3[count];
        originalRotations = new Quaternion[count];
        originalScales = new Vector3[count];
        childRigidbodies = new Rigidbody[count];
        sleepCountDowns = new int[count];

        // 각 자식의 초기 상태(월드 위치, 월드 회전, 로컬 스케일) 저장
        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = childTransforms[i].position;
            originalRotations[i] = childTransforms[i].rotation;
            originalScales[i] = childTransforms[i].localScale;
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
        // 상태 변화 시마다 리스폰 수행
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
        for (int i = 0; i < count; i++)
        {
            // 저장된 초기 상태로 복원 (월드 좌표 기준)
            childTransforms[i].position = originalPositions[i];
            childTransforms[i].rotation = originalRotations[i];
            childTransforms[i].localScale = originalScales[i];

            // 리지드바디가 있다면 물리 상태 초기화
            if (childRigidbodies[i] != null)
            {
                childRigidbodies[i].velocity = Vector3.zero;
                childRigidbodies[i].angularVelocity = Vector3.zero;

                // 필요시 슬립 프레임 동안 kinematic 처리하여 ghost collision 방지
                if (!childRigidbodies[i].isKinematic && _sleepFrames > 0)
                {
                    sleepCountDowns[i] = _sleepFrames;
                    childRigidbodies[i].isKinematic = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 각 자식 리지드바디의 슬립 카운트다운을 처리
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
