using UnityEngine;

public class MapHelper
{
    public static MapHelper Instance;
    public const float LEFT_TOP_LAT = 58.237404433709145f;
    public const float LEFT_TOP_LONG = 92.4907995336642f;
    public const float RIGHT_BOTTOM_LAT = 50.39817431586244f;
    public const float RIGHT_BOTTOM_LONG = 116.5069128147453f;
    private const float latErrorCompensation = 13.7f;
    private const float lngErrorCompensation = 1.43f;
    public readonly Vector2Int MapSize = new(1370, 770);
    private float step_lat;
    private float step_long;

    public void Initialize()
    {
        Instance = this;

        step_lat = (LEFT_TOP_LAT - RIGHT_BOTTOM_LAT) / MapSize.y;
        step_long = (RIGHT_BOTTOM_LONG - LEFT_TOP_LONG) / MapSize.x;
    }

    public Vector2 LatLongToXY(float lat, float lng)
    {
        lng = Mathf.Clamp(lng, LEFT_TOP_LONG, RIGHT_BOTTOM_LONG);
        lat = Mathf.Clamp(lat, RIGHT_BOTTOM_LAT, LEFT_TOP_LAT);

        return new Vector2((lng - LEFT_TOP_LONG) / step_long - lngErrorCompensation,
                            -(LEFT_TOP_LAT - lat) / step_lat - latErrorCompensation);
    }

    public (float, float) XYToLatLong(Vector2 pixel)
    {
        pixel.x = Mathf.Clamp(pixel.x, 0, MapSize.x);
        pixel.y = Mathf.Clamp(pixel.y, -MapSize.y, 0);

        return (pixel.y * step_lat + LEFT_TOP_LAT, pixel.x * step_long + LEFT_TOP_LONG);
    }
}
