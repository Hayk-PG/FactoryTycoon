using UnityEngine;
using System;

[Serializable]
public class ButtonWithSelectableObject 
{
    [SerializeField] private Btn _btn;
    [SerializeField] private SelectableObjectInfo _selectableObject;
    [SerializeField] private Transform _dummy;

    public Btn Btn => _btn;
    public SelectableObjectInfo SelectableObjectInfo => _selectableObject;
    public Transform Dummy => _dummy;
}
