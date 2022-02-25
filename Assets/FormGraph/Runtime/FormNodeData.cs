using System;
using UnityEngine;

[Serializable]
public class FormNodeData
{
    public string Guid;
    public string FormName;
    public Vector2 Position;
    public bool IsBranch;
    public BaseForm BaseForm;
}
