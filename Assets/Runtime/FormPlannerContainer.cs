using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FormPlannerContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<FormNodeData> FormNodeDatas = new List<FormNodeData>();
}
