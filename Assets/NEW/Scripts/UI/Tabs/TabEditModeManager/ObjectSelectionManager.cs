using UnityEngine;
using Pautik;

public class ObjectSelectionManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Selectable Objects")]
    [SerializeField] private ButtonWithSelectableObject[] _buttonWithSelectableObjects;
    private SelectableObjectInfo _selectedObject;
    private Transform _dummy;

    [Header("Tile Highlighting")]
    [SerializeField] private ObjectPlacementValidator _objectPlacementValidator;

    [SerializeField] private bool _isHandlerDragging;




    private void OnEnable()
    {
        SubscribeToBtnEvents();
    }

    private void Update()
    {
        RequestObjectPlacementValidation();
    }

    private void SubscribeToBtnEvents()
    {
        for (int i = 0; i < _buttonWithSelectableObjects.Length; i++)
        {
            ButtonWithSelectableObject buttonWithSelectableObject = _buttonWithSelectableObjects[i];
            buttonWithSelectableObject.Btn.OnPointerUpHandler += () => OnPointerUpHandler(buttonWithSelectableObject.SelectableObjectInfo);
            buttonWithSelectableObject.Btn.OnPointerDownHandler += () => OnPointerDownHandler(buttonWithSelectableObject.SelectableObjectInfo, buttonWithSelectableObject.Dummy);
            buttonWithSelectableObject.Btn.OnBeginDragHandler += () => OnBeginDragHandler(buttonWithSelectableObject.SelectableObjectInfo);
            buttonWithSelectableObject.Btn.OnEndDragHandler += () => OnEndDragHandler(buttonWithSelectableObject.SelectableObjectInfo);
        }
    }

    private void OnPointerUpHandler(SelectableObjectInfo selectableObject)
    {
        // TODO: Implement pointer up handler logic
    }

    private void OnPointerDownHandler(SelectableObjectInfo selectableObject, Transform dummy)
    {
        SelectObject(selectableObject, dummy);
    }

    private void OnBeginDragHandler(SelectableObjectInfo selectableObject)
    {
        DetectHandlerDrag(true);      
    }

    private void OnEndDragHandler(SelectableObjectInfo selectableObject)
    {       
        DetectHandlerDrag(false);
        PlaceSelectedObject();
    }

    private void SelectObject(SelectableObjectInfo selectableObject, Transform dummy)
    {
        bool hasSelectableObject = selectableObject != null;

        if (!hasSelectableObject)
        {
            return;
        }

        _selectedObject = selectableObject;
        _dummy = dummy;
    }

    private void DetectHandlerDrag(bool isHandlerDragging)
    {
        _isHandlerDragging = isHandlerDragging;
    }

    private void RequestObjectPlacementValidation()
    {
        if (_isHandlerDragging)
        {
            _objectPlacementValidator.RequestObjectPlacementValidation(_selectedObject, _dummy);
        }
    }

    private void PlaceSelectedObject()
    {
        _objectPlacementValidator.PlaceSelectedObject(_selectedObject, _dummy);
    }

    public void SetActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}
