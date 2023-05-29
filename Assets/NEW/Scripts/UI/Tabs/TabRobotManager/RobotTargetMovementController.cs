using UnityEngine;

/// <summary>
/// Class responsible for controlling the movement of a robot target based on mouse input.
/// </summary>
public class RobotTargetMovementController    : MonoBehaviour
{
    [Header("UI Elements Transforms")]
    [SerializeField] private Canvas _canvas; // Reference to the canvas component
    [SerializeField] private RectTransform _targetScreenTr; // UI element representing the screen position for the target
    [SerializeField] private RectTransform _canvasTr; // Canvas object containing the UI elements
    [SerializeField] private RectTransform _targetTr; // Target object to be moved

    private Vector2 _halfDisplaySize; // Half the size of the display
    private Vector2 _desiredPosition; // Desired position for the target
    private Vector2 _targetHalfSize; // Half the size of the target icon
    private Vector2 _rangeMin; // Minimum boundary limits
    private Vector2 _rangeMax; // Maximum boundary limits
    private Vector2 _targetPosition; // Actual position of the target
    private Vector2 _normalizedMousePosition; // Normalized mouse position within the range

    private object[] _data = new object[1]; // Data array for sending to the robot

    private bool IsPressedToMove => Input.GetMouseButton(0); // Flag indicating if mouse button is pressed to move the target




    private void Awake()
    {
        GetHalfDisplaySize();
        GetTargetHalfSize();
        GetBoundaryLimits();
    }

    private void OnEnable()
    {
        References.Manager.RobotTaskManager.OnRobotTask += RobotTaskManager_OnRobotTask;
    }

    private void Update()
    {
        Process();
    }
  
    private void RobotTaskManager_OnRobotTask(RobotTaskType robotTaskType, object[] data)
    {
        Initialize(robotTaskType, data);
    }

    // Initializes the target position based on the received robot task.
    private void Initialize(RobotTaskType robotTaskType, object[] data)
    {
        if (robotTaskType == RobotTaskType.InitializeMoveScreenTarget)
        {
            float x = Mathf.Lerp(_rangeMin.x, _rangeMax.x, (float)data[0]);
            float y = Mathf.Lerp(_rangeMin.y, _rangeMax.y, (float)data[1]);

            _targetTr.anchoredPosition = new Vector2(x, 0);
        }
    }

    // Performs the main processing logic for moving the target based on mouse input.
    private void Process()
    {
        if (!IsPressedToMove)
        {
            return;
        }

        Vector2 mousePosition = ((Vector2)Input.mousePosition - _halfDisplaySize) / _canvasTr.localScale.x;

        bool isWithinRange = mousePosition.x > _rangeMin.x - 250f && mousePosition.x < _rangeMax.x + 250f && mousePosition.y > _rangeMin.y - 250f && mousePosition.y < _rangeMax.y + 250f;

        SetDesiredPosition(isWithinRange, mousePosition);
        CalculateTargetPosition();
        MoveTowardsTargetPosition();
        CalculateNormalizedMousePosition();
        SendDataToRobot();
    }

    // Retrieves half the size of the display.
    private void GetHalfDisplaySize()
    {
        _halfDisplaySize = _canvas.renderingDisplaySize / 2;
    }

    // Retrieves half the size of the target object.
    private void GetTargetHalfSize()
    {
        _targetHalfSize = _targetTr.sizeDelta / 2;
    }

    // Retrieves the boundary limits for the target movement.
    private void GetBoundaryLimits()
    {
        _rangeMin.x = -HorizontalBoundary(-_targetHalfSize.x);
        _rangeMax.x = HorizontalBoundary(-_targetHalfSize.x);
        _rangeMin.y = -VerticalBoundary(-_targetHalfSize.y);
        _rangeMax.y = VerticalBoundary(-_targetHalfSize.y);
    }

    // Sets the desired position for the target based on mouse input and range.
    private void SetDesiredPosition(bool isWithinRange, Vector2 position)
    {
        if (isWithinRange)
        {
            _desiredPosition = position;
        }
    }

    // Calculates the target position based on the desired position and boundary limits.
    private void CalculateTargetPosition()
    {
        _targetPosition.x = _desiredPosition.x <= _rangeMin.x ? _rangeMin.x : _desiredPosition.x >= _rangeMax.x ? _rangeMax.x : _desiredPosition.x;
        _targetPosition.y = _desiredPosition.y <= _rangeMin.y ? _rangeMin.y : _desiredPosition.y >= _rangeMax.y ? _rangeMax.y : _desiredPosition.y;
    }

    // Moves the target towards the calculated target position.
    private void MoveTowardsTargetPosition()
    {
        _targetTr.anchoredPosition = _targetPosition;
    }

    // Calculates the normalized mouse position within the range.
    private void CalculateNormalizedMousePosition()
    {
        _normalizedMousePosition.x = Mathf.InverseLerp(_rangeMin.x, _rangeMax.x, _targetTr.anchoredPosition.x);
        _normalizedMousePosition.y = Mathf.InverseLerp(_rangeMin.y, _rangeMax.y, _targetTr.anchoredPosition.y);
    }

    // Sends the normalized mouse position data to the robot.
    private void SendDataToRobot()
    {
        _data[0] = _normalizedMousePosition;

        References.Manager.RobotTaskManager.RaiseRobotTaskEvent(RobotTaskType.Move, _data);
    }

    // Calculates the horizontal boundary limit based on the target size.
    private float HorizontalBoundary(float size = 0)
    {
        return (_targetScreenTr.rect.size.x / 2) + size;
    }

    // Calculates the vertical boundary limit based on the target size.
    private float VerticalBoundary(float size = 0)
    {
        return (_targetScreenTr.rect.size.y / 2) + size;
    }
}