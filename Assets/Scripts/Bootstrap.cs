using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap Instance;
    private MapHelper mapUtils;

    [Header("Editor-linked references")]
    public RectTransform map;
    public WayDrawer wayDrawer;
    public BeaconManager beaconManager;
    public VORIndicator vorIndicator;
    public DMEIndicator dmeIndicator;

    [SerializeField]
    private UIUpdater uiUpdater;

    [SerializeField]
    private GameObject aircraftPrefab;

    [Header("Runtime-linked references")]
    public RectTransform aircraftTransform;
    public Aircraft aircraft;

    private void Awake()
    {
        Instance = this;

        mapUtils = new MapHelper();
        mapUtils.Initialize();

        wayDrawer.Initialize();

        beaconManager.Initialize();

        aircraftTransform = Instantiate(aircraftPrefab, map).GetComponent<RectTransform>();
        aircraft = aircraftTransform.GetComponent<Aircraft>();
        aircraft.Initialize();

        vorIndicator.Initialize();
        dmeIndicator.Initialize();
        
        uiUpdater.Initialize();
    }
}
