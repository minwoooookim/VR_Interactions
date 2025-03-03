using UnityEngine;
using Oculus.Interaction;

public class RayGrabRelativeInteractable : RayInteractable
{
    private bool _isGrabbed = false;
    private Pose _grabOffset;
    private Transform _grabberTransform;

    /// <summary>
    /// Grab�� ���۵� �� ȣ���մϴ�.
    /// grabber(��: ��Ʈ�ѷ� �Ǵ� ��)�� Transform�� �����ϸ�,
    /// grab ���� ������ ��� �������� ����մϴ�.
    /// </summary>
    public void BeginGrab(Transform grabber)
    {
        if (grabber == null)
        {
            Debug.LogWarning("BeginGrab: grabber is null.");
            return;
        }
        _grabberTransform = grabber;
        Pose grabberPose = new Pose(grabber.position, grabber.rotation);
        Pose objectPose = new Pose(transform.position, transform.rotation);
        _grabOffset = InverseTransformPose(grabberPose, objectPose);
        _isGrabbed = true;
    }

    /// <summary>
    /// Grab�� ����� �� ȣ���մϴ�.
    /// </summary>
    public void EndGrab()
    {
        _isGrabbed = false;
        _grabberTransform = null;
    }

    private void Update()
    {
        if (_isGrabbed && _grabberTransform != null)
        {
            // ���� grabber�� Pose�� ��ϵ� �������� �����Ͽ� target Pose�� ���
            Pose grabberPose = new Pose(_grabberTransform.position, _grabberTransform.rotation);
            Pose targetPose = MultiplyPose(grabberPose, _grabOffset);
            transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
        }
    }

    /// <summary>
    /// reference�� ����ȯ�� �����Ͽ�, reference ������ pose�� ����մϴ�.
    /// ��, �� Pose ������ ��� Pose(offset)�� ���մϴ�.
    /// </summary>
    public static Pose InverseTransformPose(Pose reference, Pose pose)
    {
        Quaternion invRot = Quaternion.Inverse(reference.rotation);
        Vector3 position = invRot * (pose.position - reference.position);
        Quaternion rotation = invRot * pose.rotation;
        return new Pose(position, rotation);
    }

    /// <summary>
    /// �� Pose�� ���Ͽ�, a ���ؿ��� b�� ������� ��ȯ�� ������ Pose�� ��ȯ�մϴ�.
    /// </summary>
    public static Pose MultiplyPose(Pose a, Pose b)
    {
        Vector3 position = a.position + a.rotation * b.position;
        Quaternion rotation = a.rotation * b.rotation;
        return new Pose(position, rotation);
    }
}
