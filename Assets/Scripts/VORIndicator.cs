using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VORIndicator : MonoBehaviour
{
    [SerializeField]
    private RectTransform singleArrow;

    [SerializeField]
    private RectTransform doubleArrow;
    private const float ARROWS_ROTATION_SPEED = 1000f;

    [SerializeField]
    private List<Beacon> closestBeacons;
    private Vector3 dir1, dir2;
    private float angle1, angle2;
    private Quaternion targetRot1, targetRot2;
    Quaternion angle1Clamped, angle2Clamped;
    public event BeaconsDataHandler ClosestBeaconsChangedEvent;

    public void Initialize()
    {
        GetVORBeaconsAndRotations();
        singleArrow.rotation = targetRot1;
        doubleArrow.rotation = targetRot2;
    }

    private void Update()
    {
        if (Bootstrap.Instance.aircraft.isMoving)
        {
            GetVORBeaconsAndRotations();
            OnClosestBeaconsChange();
            singleArrow.rotation = Quaternion.RotateTowards(singleArrow.rotation, targetRot1, ARROWS_ROTATION_SPEED * Time.deltaTime);
            doubleArrow.rotation = Quaternion.RotateTowards(doubleArrow.rotation, targetRot2, ARROWS_ROTATION_SPEED * Time.deltaTime);
        }
    }

    private void GetVORBeaconsAndRotations()
    {
        closestBeacons = BeaconManager.Instance.beacons.Where(beacon =>
        {
            return beacon.type == BeaconType.VOR || beacon.type == BeaconType.VORDME;
        }).Take(2).ToList();

        dir1 = closestBeacons[0].Instance.transform.position - Bootstrap.Instance.aircraftTransform.position;
        dir2 = closestBeacons[1].Instance.transform.position - Bootstrap.Instance.aircraftTransform.position;

        angle1 = Mathf.Atan2(dir1.normalized.y, dir1.normalized.x) * Mathf.Rad2Deg - 90f;
        angle2 = Mathf.Atan2(dir2.normalized.y, dir2.normalized.x) * Mathf.Rad2Deg - 90f;

        targetRot1 = Quaternion.Euler(new Vector3(0f, 0f, angle1));
        targetRot2 = Quaternion.Euler(new Vector3(0f, 0f, angle2));
    }

    public void OnClosestBeaconsChange()
    {
        angle1Clamped = Quaternion.Euler(0f, 0f, angle1);
        angle1Clamped.w *= -1f;
        angle2Clamped = Quaternion.Euler(0f, 0f, angle2);
        angle2Clamped.w *= -1f;

        ClosestBeaconsChangedEvent?.Invoke(
            (closestBeacons[0].name, closestBeacons[1].name),
            (angle1Clamped.eulerAngles.z, angle2Clamped.eulerAngles.z)
        );
    }
}

