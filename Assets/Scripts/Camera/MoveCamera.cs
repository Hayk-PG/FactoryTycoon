using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour
{
    public enum TOUCH_MOVEMENT { TOUCHED, MOVED, RELEASED}
    public TOUCH_MOVEMENT touchMovement;

    [Header("MOVEMENT VALUES")]
    [SerializeField] float smoothTime;
    [SerializeField] float maxSpeed;

    float speed = 1;
    float second;
    float y = 2.02f;

    Vector3 direction;
    Vector3 currentVelocity;
    Touch touch;

    [Header("CHILD CAMERAS")]
    [SerializeField] Camera[] childCameras; 

    void Awake()
    {
        touchMovement = TOUCH_MOVEMENT.RELEASED;
    }

    void Update()
    {
        SyncChildCamerasOrtographicSize();

        if (Input.touchCount == 1)
        {
            GetTouchPhases();
        }

        else if(Input.touchCount == 2)
        {
            Pinch();
        }

        else
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (touchMovement == TOUCH_MOVEMENT.MOVED)
        {
            second += Time.fixedDeltaTime;

            Move(second >= 0.1f);
        }
        if (touchMovement == TOUCH_MOVEMENT.RELEASED)
        {
            second = 0;
        }
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            
        }
    }

    void SyncChildCamerasOrtographicSize()
    {
        foreach (var camera in childCameras)
        {
            if(camera.orthographicSize != GetComponent<Camera>().orthographicSize)
            {
                camera.orthographicSize = GetComponent<Camera>().orthographicSize;
            }
        }
    }

    void GetTouchPhases()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchMovement = TOUCH_MOVEMENT.TOUCHED;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject())
        {
            touchMovement = TOUCH_MOVEMENT.MOVED;
            direction = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchMovement = TOUCH_MOVEMENT.RELEASED;
        }
    }

    void Move(bool canMove)
    {
        if (canMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, direction, ref currentVelocity, 0.1f, maxSpeed * Time.fixedDeltaTime);
        }
    }

    void Pinch()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector3 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector3 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        Zoom(difference * 0.1f * Time.deltaTime);
    }

    void Zoom(float value)
    {
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize - value, 2.05f, 3.05f);
    }











}
