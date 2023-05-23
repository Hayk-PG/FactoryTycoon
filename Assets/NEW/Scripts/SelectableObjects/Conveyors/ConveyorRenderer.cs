using UnityEngine;
using Pautik;

public class ConveyorRenderer : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private MeshRenderer[] _meshRenderers;

    [Header("Materials")]
    [SerializeField] private Material _materialDefault;
    [SerializeField] private Material _materialHighlight;




    public void SetDefaultMaterial()
    {
        ChangeMaterial(_materialDefault);
    }

    public void SetHighlightMaterial()
    {
        ChangeMaterial(_materialHighlight);
    }

    private void ChangeMaterial(Material material)
    {
        GlobalFunctions.Loop<MeshRenderer>.Foreach(_meshRenderers, meshRenderer => meshRenderer.material = material);
    }
}
