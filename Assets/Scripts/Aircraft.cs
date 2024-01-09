using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Aircraft : MonoBehaviour
{
    private WayDrawer wayDrawer;
    private RectTransform rectTransform;
    private Vector3[] positions;
    private int wayPointIndex = 0;
    public bool isMoving;
    private const float MOVE_SPEED = 1.5f;
    private float targetAngle;
    private Vector2 targetDirection;
    private const float START_LAT = 52.29775689091742f;
    private const float START_LONG = 104.2721954266937f;
    public event Position2DHandler PositionChangedEvent;

    public void Initialize()
    {
        wayDrawer = WayDrawer.Instance;
        rectTransform = GetComponent<RectTransform>();
        SetPosition(START_LAT, START_LONG);
    }

    public void OnPositionChange()
    {
        PositionChangedEvent?.Invoke(MapHelper.Instance.XYToLatLong(rectTransform.anchoredPosition));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            wayDrawer.CreateMultipleWaypoints();
        }

        if (Input.GetMouseButtonDown(0))
        {
            wayDrawer.CreateSingleWaypoint();
        }

        if (Input.GetMouseButtonUp(0) && wayDrawer.MousePosInMapBounds())
        {
            GetWaypoints();
        }

        Move();
    }

    public void SetPosition(float lat, float lng)
    {
        lng = Mathf.Clamp(lng, MapHelper.LEFT_TOP_LONG, MapHelper.RIGHT_BOTTOM_LONG);
        lat = Mathf.Clamp(lat, MapHelper.RIGHT_BOTTOM_LAT, MapHelper.LEFT_TOP_LAT);

        rectTransform.anchoredPosition = MapHelper.Instance.LatLongToXY(lat, lng);
    }

    public void SetPosition(Vector2 pixel)
    {
        pixel.x = Mathf.Clamp(pixel.x, 0, MapHelper.Instance.MapSize.x);
        pixel.y = Mathf.Clamp(pixel.y, 0, -MapHelper.Instance.MapSize.y);

        rectTransform.anchoredPosition = pixel;
    }

    private void GetWaypoints()
    {
        positions = new Vector3[wayDrawer.line.positionCount];
        wayDrawer.line.GetPositions(positions);

        // позже надо сделать переключатель для этой штуки,
        // чтобы в тестах не ждать ВС к подходу слишком долго
        // transform.position = positions[0];

        isMoving = true;
        wayPointIndex = 0;
    }

    private void Move()
    {
        if (isMoving && positions.Length > 0)
        {
            Vector2 targetPosition = positions[wayPointIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, MOVE_SPEED * Time.deltaTime);

            targetDirection = targetPosition - (Vector2)transform.position;
            targetAngle = Mathf.Atan2(targetDirection.normalized.y, targetDirection.normalized.x) * Mathf.Rad2Deg + 90f;
            if (targetAngle != 90f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), 0.3f);

            if (Vector2.Distance(transform.position, targetPosition) < 0.0005f)
            {
                wayPointIndex++;
            }

            if (wayPointIndex > positions.Length - 1)
            {
                isMoving = false;
            }

            OnPositionChange();
        }
    }
}