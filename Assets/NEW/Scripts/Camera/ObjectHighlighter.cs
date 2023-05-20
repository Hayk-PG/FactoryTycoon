using UnityEngine;
using Pautik;
using System;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _tileLayerMask;
    private RaycastHit _hit;

    // Calculate the ray from the camera to the mouse position
    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);

    // Event for tile highlighting
    public event Action<Vector3> OnTileHighlight;
    
    
    
    
    private void Update()
    {
        // Check if the raycast hits an object with the specified layer
        if (IsRaycastHit(Ray, Mathf.Infinity, _tileLayerMask))
        {
            RaiseOnTileHighlightEvent(_hit.transform.position);
        }
    }

    // Invoke the event and pass the highlighted tile position
    private void RaiseOnTileHighlightEvent(Vector3 position)
    {
        OnTileHighlight?.Invoke(position);
    }

    // Perform the raycast and check if it hits an object in the specified layer
    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    }
}
