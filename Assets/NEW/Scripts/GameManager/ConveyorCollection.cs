using UnityEngine;

public class ConveyorCollection : BaseCollection<Vector3, ConveyorReplacementManager>
{
    protected override void Awake()
    {
        
    }

    public override void Add(Vector3 key, ConveyorReplacementManager value)
    {
        base.Add(key, value);
    }
}
