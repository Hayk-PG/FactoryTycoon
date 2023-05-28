using UnityEngine;
using UnityEngine.UI;

public class RobotJointRotationSlider : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private Slider _slider;

    private object[] _data = new object[2] { 0, 0 };




    private void OnEnable()
    {
        // Subscribe to the OnRobotTask event in the RobotTaskManager
        References.Manager.RobotTaskManager.OnRobotTask += OnControllSlider;

        // Add a listener to handle value changes in the slider
        _slider.onValueChanged.AddListener(OnSliderValueChange);
    }

    private void OnControllSlider(RobotTaskType robotTaskType, object[] data)
    {
        if(robotTaskType != RobotTaskType.SetJointSliderValues)
        {
            return;
        }

        // Joint index
        _data[0] = (int)data[0];

        // Slider values
        _slider.minValue = (float)data[1];
        _slider.maxValue = (float)data[2];
        _slider.value = (float)data[3];      
    }

    private void OnSliderValueChange(float value)
    {
        _data[1] = value;

        // Raise an event to notify the RobotTaskManager about the slider value change
        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.GetJointSliderValues, _data);
    }
}
