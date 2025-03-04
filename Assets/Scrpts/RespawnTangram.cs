using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class RespawnTangram : MonoBehaviour
{
    [Header("Tangram Parents")]
    [SerializeField] private GameObject tangramParent1;
    [SerializeField] private GameObject tangramParent2;

    [Header("Poke Interactable")]
    [SerializeField] private PokeInteractable pokeInteractable;

    // 각 parent의 자식 정보를 별도로 저장할 구조체
    private class ChildData
    {
        public Transform transform;
        public Vector3 originalLocalPosition;
        public Quaternion originalLocalRotation;
        public Rigidbody rigidbody;
    }

    private List<ChildData> childData1 = new List<ChildData>();
    private List<ChildData> childData2 = new List<ChildData>();

    private void Awake()
    {
        // parent1의 자식 정보 저장 (비활성 객체 포함)
        if (tangramParent1 != null)
        {
            Transform[] transforms = tangramParent1.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in transforms)
            {
                ChildData data = new ChildData();
                data.transform = t;
                data.originalLocalPosition = t.localPosition;
                data.originalLocalRotation = t.localRotation;
                data.rigidbody = t.GetComponent<Rigidbody>();
                childData1.Add(data);
            }
        }
        else
        {
            Debug.LogWarning("Tangram Parent1이 할당되지 않았습니다.", this);
        }

        // parent2의 자식 정보 저장 (비활성 객체 포함)
        if (tangramParent2 != null)
        {
            Transform[] transforms = tangramParent2.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in transforms)
            {
                ChildData data = new ChildData();
                data.transform = t;
                data.originalLocalPosition = t.localPosition;
                data.originalLocalRotation = t.localRotation;
                data.rigidbody = t.GetComponent<Rigidbody>();
                childData2.Add(data);
            }
        }
        else
        {
            Debug.LogWarning("Tangram Parent2가 할당되지 않았습니다.", this);
        }

        // PokeInteractable 이벤트 연결
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
        // 버튼(또는 해당 이벤트)로 리스폰 호출
        Respawn();
    }

    // 두 parent 모두 리스폰 실행
    public void Respawn()
    {
        ResetParent(childData1);
        ResetParent(childData2);
    }

    // 저장해둔 자식 정보를 기준으로 초기화
    private void ResetParent(List<ChildData> dataList)
    {
        foreach (ChildData data in dataList)
        {
            if (data.transform != null)
            {
                data.transform.localPosition = data.originalLocalPosition;
                data.transform.localRotation = data.originalLocalRotation;

                if (data.rigidbody != null)
                {
                    data.rigidbody.velocity = Vector3.zero;
                    data.rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}
