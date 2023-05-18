

public interface IItemDeformation 
{
    event System.Action<bool> OnFinishItemDeformation;
    UnityEngine.Renderer ItemMeshRend { get;}
    IMachineController Machine { get; set; }
    float DeformationValue { get; set; }
    bool StartDeformation { get; set; }
    bool HasDeformationFinished { get; set; }



}
