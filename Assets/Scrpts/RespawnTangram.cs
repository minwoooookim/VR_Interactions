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

    // �� parent�� �ڽ� ������ ������ ������ ����ü
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
        // parent1�� �ڽ� ���� ���� (��Ȱ�� ��ü ����)
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
            Debug.LogWarning("Tangram Parent1�� �Ҵ���� �ʾҽ��ϴ�.", this);
        }

        // parent2�� �ڽ� ���� ���� (��Ȱ�� ��ü ����)
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
            Debug.LogWarning("Tangram Parent2�� �Ҵ���� �ʾҽ��ϴ�.", this);
        }

        // PokeInteractable �̺�Ʈ ����
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
        // ��ư(�Ǵ� �ش� �̺�Ʈ)�� ������ ȣ��
        Respawn();
    }

    // �� parent ��� ������ ����
    public void Respawn()
    {
        ResetParent(childData1);
        ResetParent(childData2);
    }

    // �����ص� �ڽ� ������ �������� �ʱ�ȭ
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
