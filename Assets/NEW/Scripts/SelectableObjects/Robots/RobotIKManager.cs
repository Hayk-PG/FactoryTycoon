using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pautik;

public class RobotIKManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Transform _joint;
    [SerializeField] private Btn[] _buttons;

    private Vector3 _startRotation;
    private Vector3 _targetRotation;
    private float _speed = 50;
    private float _delay;
    private bool _run;
    private bool _stop;
    private bool _isStartRotationMatchPassed;




    private void OnEnable()
    {
        _buttons[0].OnSelect += SetStartRotation;
        _buttons[1].OnSelect += OnSetTargetRotation;
        _buttons[2].OnSelect += SetDelay;
        _buttons[3].OnSelect += ToggleRun;
    }

    private void Start()
    {
        SetSliderValue();
    }

    private void Update()
    {
        if (!_run)
        {
            UpdateRotation(new Vector3(0, 0, Mathf.Lerp(-50, 50, _slider.value)));

            return;
        }

        bool isRotationMatchStart = _joint.eulerAngles == _startRotation;
        bool isRotationMatchTarget = _joint.eulerAngles == _targetRotation;

        CalculateRange(out float range);

        if (!isRotationMatchStart && !_isStartRotationMatchPassed)
        {
            RunJointRotation(Angle(_startRotation), range <= 0.01f, true);
        }
        else if (!isRotationMatchTarget && _isStartRotationMatchPassed)
        {
            RunJointRotation(Angle(_targetRotation), range >= 0.99f, false);
        }
    }

    private void ToggleRun()
    {
        _run = !_run;
    }

    private void SetDelay()
    {
        _delay = 2;
    }

    private void OnSetTargetRotation()
    {
        _targetRotation = _joint.eulerAngles;
    }

    private void SetStartRotation()
    {
        _startRotation = _joint.eulerAngles;
    }

    private void SetSliderValue()
    {
        float minAngle = -50;
        float maxAngle = 50;
        float currentAngle = _joint.transform.eulerAngles.z;
        float value = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

        _slider.value = value;
    }

    private void RunJointRotation(Vector3 targetRotation, bool stopRotation, bool toggleStartRotationMatch)
    {
        bool isJointAngleLessThan = Angle(_joint.eulerAngles).z < Angle(targetRotation).z;

        CalculateJointZAngle(isJointAngleLessThan, out float angle);
        CalculateRange(out float range);
        UpdateRotation(new Vector3(0f, 0f, angle));

        if (stopRotation)
        {
            UpdateRotation(targetRotation);
            ToggleStartRotationMatch(toggleStartRotationMatch);
        }
    }

    private void CalculateJointZAngle(bool increaseAngle, out float angle)
    {
        angle = Angle(_joint.eulerAngles).z;

        if (increaseAngle)
        {
            angle += _speed * Time.deltaTime;
        }
        else
        {
            angle -= _speed * Time.deltaTime;
        }
    }

    private void CalculateRange(out float range)
    {
        range = Mathf.InverseLerp(Angle(_startRotation).z, Angle(_targetRotation).z, Angle(_joint.eulerAngles).z);
    }

    private void UpdateRotation(Vector3 rotation)
    {
        _joint.eulerAngles = rotation;
    }

    private void ToggleStartRotationMatch(bool isStartRotationMatch)
    {
        _isStartRotationMatchPassed = isStartRotationMatch;
    }

    private Vector3 Angle(Vector3 angle)
    {
        return new Vector3(Converter.ConvertAngle(angle.x), Converter.ConvertAngle(angle.y), Converter.ConvertAngle(angle.z));
    }
}