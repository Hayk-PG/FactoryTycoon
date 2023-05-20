using UnityEngine;
using Pautik;

public class ObjectSelectionManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Selectable Objects")]
    [SerializeField] private ButtonWithSelectableObject[] _buttonWithSelectableObjects;
    [SerializeField] private SelectableObjectInfo _selectedObject;

    [Header("Tile Highlighting")]
    [SerializeField] private ObjectHighlighter _objectHighlighter;

    [SerializeField] private bool _isHandlerDragging;




    private void OnEnable()
    {
        SubscribeToBtnEvents();
    }

    private void Update()
    {
        DetectRayCastHit();
    }

    private void SubscribeToBtnEvents()
    {
        for (int i = 0; i < _buttonWithSelectableObjects.Length; i++)
        {
            ButtonWithSelectableObject buttonWithSelectableObject = _buttonWithSelectableObjects[i];
            buttonWithSelectableObject.Btn.OnPointerUpHandler += () => OnPointerUpHandler(buttonWithSelectableObject.SelectableObjectInfo);
            buttonWithSelectableObject.Btn.OnPointerDownHandler += () => OnPointerDownHandler(buttonWithSelectableObject.SelectableObjectInfo);
            buttonWithSelectableObject.Btn.OnBeginDragHandler += () => OnBeginDragHandler(buttonWithSelectableObject.SelectableObjectInfo);
            buttonWithSelectableObject.Btn.OnEndDragHandler += () => OnEndDragHandler(buttonWithSelectableObject.SelectableObjectInfo);
        }
    }

    private void OnPointerUpHandler(SelectableObjectInfo selectableObject)
    {

    }

    private void OnPointerDownHandler(SelectableObjectInfo selectableObject)
    {
        SelectObject(selectableObject);
    }

    private void OnBeginDragHandler(SelectableObjectInfo selectableObject)
    {
        DetectHandlerDrag(true);      
    }

    private void OnEndDragHandler(SelectableObjectInfo selectableObject)
    {       
        DetectHandlerDrag(false);
    }

    private void SelectObject(SelectableObjectInfo selectableObject)
    {
        bool hasSelectableObject = selectableObject != null;

        if (!hasSelectableObject)
        {
            return;
        }

        _selectedObject = selectableObject;
    }

    private void DetectHandlerDrag(bool isHandlerDragging)
    {
        _isHandlerDragging = isHandlerDragging;
    }

    private void DetectRayCastHit()
    {
        if (_isHandlerDragging)
        {
            _objectHighlighter.DetectRayCastHit();
        }
    }

    public void SetActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}
