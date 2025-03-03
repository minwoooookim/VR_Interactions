using UnityEngine;
using Oculus.Interaction;

public class RayGrabRelativeInteractable : RayInteractable
{
    private bool _isGrabbed = false;
    private Pose _grabOffset;
    private Transform _grabberTransform;

    /// <summary>
    /// Grab이 시작될 때 호출합니다.
    /// grabber(예: 컨트롤러 또는 손)의 Transform을 전달하면,
    /// grab 시작 시점의 상대 오프셋을 기록합니다.
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
    /// Grab이 종료될 때 호출합니다.
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
            // 현재 grabber의 Pose에 기록된 오프셋을 적용하여 target Pose를 계산
            Pose grabberPose = new Pose(_grabberTransform.position, _grabberTransform.rotation);
            Pose targetPose = MultiplyPose(grabberPose, _grabOffset);
            transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
        }
    }

    /// <summary>
    /// reference의 역변환을 적용하여, reference 기준의 pose를 계산합니다.
    /// 즉, 두 Pose 사이의 상대 Pose(offset)를 구합니다.
    /// </summary>
    public static Pose InverseTransformPose(Pose reference, Pose pose)
    {
        Quaternion invRot = Quaternion.Inverse(reference.rotation);
        Vector3 position = invRot * (pose.position - reference.position);
        Quaternion rotation = invRot * pose.rotation;
        return new Pose(position, rotation);
    }

    /// <summary>
    /// 두 Pose를 곱하여, a 기준에서 b의 상대적인 변환을 적용한 Pose를 반환합니다.
    /// </summary>
    public static Pose MultiplyPose(Pose a, Pose b)
    {
        Vector3 position = a.position + a.rotation * b.position;
        Quaternion rotation = a.rotation * b.rotation;
        return new Pose(position, rotation);
    }
}
