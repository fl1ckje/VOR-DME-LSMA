using UnityEngine;
using UnityEngine.UI;
using System;

public class UIUpdater : MonoBehaviour
{
    [Header("Aircraft fields")]
    [SerializeField]
    private InputField aircraftLatTextField;

    [SerializeField]
    private InputField aircraftLongTextField;

    [Header("Mouse fields")]
    [SerializeField]
    private InputField mouseLatTextField;

    [SerializeField]
    private InputField mouseLongTextField;

    [Header("Short-range beacon VOR fields")]
    [SerializeField]
    private InputField shortRangeVORNameTextField;

    [SerializeField]
    private InputField shortRangeVORAzimuthTextField;

    [Header("Mid-range VOR beacon fields")]
    [SerializeField]
    private InputField midRangeVORNameTextField;

    [SerializeField]
    private InputField midRangeVORAzimuthTextField;

    [Header("Short-range beacon DME fields")]
    [SerializeField]
    private InputField shortRangeDMENameTextField;

    [SerializeField]
    private InputField shortRangeDMEDistanceTextField;

    [Header("Mid-range DME beacon fields")]
    [SerializeField]
    private InputField midRangeDMENameTextField;
    [SerializeField]
    private InputField midRangeDMEDistanceTextField;



    public void Initialize()
    {
        Bootstrap.Instance.aircraft.PositionChangedEvent += UpdateAircraftInputFields;
        Bootstrap.Instance.aircraft.OnPositionChange();

        Bootstrap.Instance.wayDrawer.MousePositionChangedEvent += UpdateMouseInputFields;
        Bootstrap.Instance.wayDrawer.OnMousePositionChange();

        Bootstrap.Instance.vorIndicator.ClosestBeaconsChangedEvent += UpdateVORTextFields;
        Bootstrap.Instance.vorIndicator.OnClosestBeaconsChange();

        Bootstrap.Instance.dmeIndicator.ClosestBeaconsChangedEvent += UpdateDMETextFields;
        Bootstrap.Instance.dmeIndicator.OnClosestBeaconsChange();
    }

    private string FormatFloat(float value) => Math.Round(value, 3).ToString();

    private void UpdateAircraftInputFields((float, float) position)
    {
        aircraftLatTextField.text = FormatFloat(position.Item1);
        aircraftLongTextField.text = FormatFloat(position.Item2);
    }

    private void UpdateMouseInputFields((float, float) position)
    {
        mouseLatTextField.text = FormatFloat(position.Item1);
        mouseLongTextField.text = FormatFloat(position.Item2);
    }

    private void UpdateVORTextFields((string, string) names, (float, float) azimuths)
    {
        shortRangeVORNameTextField.text = names.Item1;
        midRangeVORNameTextField.text = names.Item2;

        shortRangeVORAzimuthTextField.text = FormatFloat(azimuths.Item1);
        midRangeVORAzimuthTextField.text = FormatFloat(azimuths.Item2);
    }

    private void UpdateDMETextFields((string, string) names, (float, float) distances)
    {
        shortRangeDMENameTextField.text = names.Item1;
        midRangeDMENameTextField.text = names.Item2;

        shortRangeDMEDistanceTextField.text = FormatFloat(distances.Item1);
        midRangeDMEDistanceTextField.text = FormatFloat(distances.Item2);
    }
}
