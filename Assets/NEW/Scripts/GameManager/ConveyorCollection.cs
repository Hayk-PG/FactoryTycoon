using UnityEngine;

public class ConveyorCollection : BaseCollection<Vector3, ConveyorSegment>
{
    protected override void Awake()
    {
        
    }

    public override void Add(Vector3 key, ConveyorSegment value)
    {
        base.Add(key, value);
    }
}
