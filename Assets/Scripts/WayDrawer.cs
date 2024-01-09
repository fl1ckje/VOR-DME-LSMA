using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WayDrawer : MonoBehaviour
{
    public static WayDrawer Instance;
    public LineRenderer line;
    private const float LINE_WIDTH = 0.015f;
    private Vector3 previousPosition;
    private const float MIN_WAYPOINT_DISTANCE = 0.05f;
    private RectTransform mapTransform;
    private readonly Vector3[] mapBounds = new Vector3[4];
    private Vector3 mouseWorldPos;
    public event Position2DHandler MousePositionChangedEvent;
    private Vector2 mouseAnchoredPos;
    private Vector2 mouseAnchoredOffset;

    public void Initialize()
    {
        Instance = this;

        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.startWidth = line.endWidth = LINE_WIDTH;

        previousPosition = transform.position;

        mapTransform = Bootstrap.Instance.map;
        mapTransform.GetWorldCorners(mapBounds);

        mouseAnchoredOffset = new(MapHelper.Instance.MapSize.x / 2f, -MapHelper.Instance.MapSize.y / 2f);
    }

    public void OnMousePositionChange()
    {
        mouseAnchoredPos = Bootstrap.Instance.map.InverseTransformPoint(mouseWorldPos);
        mouseAnchoredPos += mouseAnchoredOffset;

        MousePositionChangedEvent?.Invoke(MapHelper.Instance.XYToLatLong(mouseAnchoredPos));
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse X") != 0f || Input.GetAxisRaw("Mouse Y") != 0f)
        {
            GetMousePosition();

            if (MousePosInMapBounds())
            {
                OnMousePositionChange();
            }
        }
    }

    public bool MousePosInMapBounds()
    {
        return mouseWorldPos.x > mapBounds[1].x && mouseWorldPos.x < mapBounds[2].x &&
                mouseWorldPos.y > mapBounds[0].y && mouseWorldPos.y < mapBounds[1].y;
    }

    private void GetMousePosition()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Bootstrap.Instance.map,
                                    Input.mousePosition, Camera.main, out mouseWorldPos);

        mouseWorldPos.x = Mathf.Clamp(mouseWorldPos.x, mapBounds[1].x, mapBounds[2].x);
        mouseWorldPos.y = Mathf.Clamp(mouseWorldPos.y, mapBounds[0].y, mapBounds[1].y);
    }

    public void CreateSingleWaypoint()
    {
        if (!MousePosInMapBounds())
            return;

        line.positionCount = 1;
        line.SetPosition(0, mouseWorldPos);
    }

    public void CreateMultipleWaypoints()
    {
        if (!MousePosInMapBounds())
            return;

        if (Vector3.Distance(mouseWorldPos, previousPosition) > MIN_WAYPOINT_DISTANCE)
        {
            line.positionCount++;

            if (previousPosition == transform.position)
            {
                line.SetPosition(0, mouseWorldPos);
            }
            else
            {
                line.SetPosition(line.positionCount - 1, mouseWorldPos);
            }

            previousPosition = mouseWorldPos;
        }
    }
}
