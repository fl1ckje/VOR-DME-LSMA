using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BeaconManager : MonoBehaviour
{
    public static BeaconManager Instance;

    [SerializeField]
    private GameObject defaultBeaconPrefab;

    [SerializeField]
    private GameObject customBeaconPrefab;

    public List<Beacon> beacons = new()
    {
        new(52.283920052645854f, 104.28749346364783f, "Иркутск", BeaconType.VORDME, BeaconImpl.DEFAULT),
        new(51.83649802605531f, 107.58217090901114f, "Улан-Удэ", BeaconType.VORDME, BeaconImpl.DEFAULT),
        new(52.05368214721413f, 113.46639143197622f, "Чита", BeaconType.VORDME, BeaconImpl.DEFAULT),
        new(56.29262613720093f, 101.71032953908764f, "Братск", BeaconType.VORDME, BeaconImpl.DEFAULT),

        new(53.39154577756098f, 109.00635888385294f, "Усть-Баргузин", BeaconType.DME, BeaconImpl.DEFAULT),
        new(52.51695723137947f, 111.53375523122716f, "Сосново-Озерское", BeaconType.DME, BeaconImpl.CUSTOM),
        new(55.78296050231323f, 109.54890162529887f, "Нижнеангарск", BeaconType.DME, BeaconImpl.CUSTOM)
    };

    public void Initialize()
    {
        Instance = this;

        SpawnBeacons();
    }

    private void Update()
    {
        if (Bootstrap.Instance.aircraft.isMoving)
        {
            SortBeaconsByDistance();
        }
    }

    private void SpawnBeacons()
    {
        for (int i = 0; i < beacons.Count; i++)
        {
            Vector2 position = MapUtils.Instance.LatLongToXY(beacons[i].Lat, beacons[i].Lng);

            GameObject instance = Instantiate(beacons[i].impl switch
            {
                BeaconImpl.DEFAULT => defaultBeaconPrefab,
                BeaconImpl.CUSTOM => customBeaconPrefab,
                _ => throw new NotImplementedException(),
            },
            Bootstrap.Instance.map);

            instance.GetComponent<RectTransform>().anchoredPosition = position;

            if (beacons[i].impl == BeaconImpl.CUSTOM)
            {
                instance.GetComponent<Text>().text = beacons[i].name;
            }

            instance.name = beacons[i].name;
            beacons[i].Instance = instance;
        }
    }

    public void SortBeaconsByDistance()
    {
        for (int i = 0; i < beacons.Count; i++)
        {
            beacons[i].distance = Vector2.Distance(Bootstrap.Instance.aircraftTransform.position,
                                                            beacons[i].Instance.transform.position);
        }

        beacons = beacons.OrderBy(b => b.distance).ToList();
    }
}
