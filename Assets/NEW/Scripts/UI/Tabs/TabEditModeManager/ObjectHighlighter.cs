using UnityEngine;
using Pautik;
using System;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _tileLayerMask;
    private RaycastHit _hit;

    private Ray Ray => CameraPoint.ScreenPointToRay(_camera, Input.mousePosition);

    public event Action<Vector3> OnTileHighlight;
    
    
    
    
    private void Update()
    {

    }

    public void DetectRayCastHit()
    {
        if (IsRaycastHit(Ray, Mathf.Infinity, _tileLayerMask))
        {
            RaiseOnTileHighlightEvent(_hit.transform.position);
        }
    }

    private void RaiseOnTileHighlightEvent(Vector3 position)
    {
        OnTileHighlight?.Invoke(position);
    }

    private bool IsRaycastHit(Ray ray, float distance, LayerMask layerMask)
    {
        return Physics.Raycast(ray, out _hit, distance, layerMask);
    }
}
