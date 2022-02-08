using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class FormParser : MonoBehaviour
{
    [SerializeField] private FormPlannerContainer formPlanner;
    private FormPlans _currentFormPlan;
    private string _currentNodeGuid;
    private void Awake()
    {
        _currentFormPlan = FormPlans.form1;
    }

    private void Start()
    {
        var entryNode = formPlanner.NodeLinks.First();
        _currentNodeGuid = entryNode.BaseNodeGuid;
        ProceedToNextForm();
    }

    [Button("Proceed")]
    private void ProceedToNextForm()
    {
        print(_currentNodeGuid);
        var targetNode = GetTargetNodeByCurrentGuid(_currentNodeGuid);
        if (targetNode == null)
        {
            print("There is no target node");return;
        }
        if (targetNode.IsBranch)
        {
            var link = formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == targetNode.Guid && x.PortName == _currentFormPlan.ToString());
            if (link != null)
            {
                print($"Changing from{GetNodeByGuid(_currentNodeGuid).FormName} to {GetNodeByGuid(link.TargetNodeGuid).FormName}");
                _currentNodeGuid = link.TargetNodeGuid;
            }
        }
        else
        {
            _currentNodeGuid = GetNodeByGuid(targetNode.Guid).Guid;
            print($"Changing to {GetNodeByGuid(_currentNodeGuid).FormName}");
        }
    }

    FormNodeData GetNodeByGuid(string guid)
    {
        return formPlanner.FormNodeDatas.FirstOrDefault(x => x.Guid == guid);
    }

    FormNodeData GetTargetNodeByCurrentGuid(string currentGuid)
    {
        var guid=formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == currentGuid)?.TargetNodeGuid;
        return GetNodeByGuid(guid);
    }
}
