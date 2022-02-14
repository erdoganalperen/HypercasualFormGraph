using System;
using System.Linq;
using UnityEngine;

public class FormGraphParser
{
    private FormPlannerContainer _formPlanner;
    private string _currentNodeGuid;
    //enums
    private Branches _currentFormBranch;
    //
    public FormGraphParser(FormPlannerContainer graph)
    {
        _formPlanner = graph;
        _currentFormBranch = Branches.branch1;
        _currentNodeGuid = _formPlanner.NodeLinks.First().BaseNodeGuid;
        ProceedToNextForm();
    }

    public Forms ProceedToNextForm()
    {
        var nextNode = GetNextFormNodeByCurrentGuid(_currentNodeGuid);
        if (nextNode == null)
        {
            Debug.Log("There is no next node");return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
        }
        if (nextNode.IsBranch)
        {
            var link = _formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == nextNode.Guid && x.PortName == _currentFormBranch.ToString());
            if (link != null)
            {
                Debug.Log($"Upgraded from => {GetNodeByGuid(_currentNodeGuid).FormName} to => {GetNodeByGuid(link.TargetNodeGuid).FormName}");
                _currentNodeGuid = link.TargetNodeGuid;
            }
        }
        else
        {
            _currentNodeGuid = GetNodeByGuid(nextNode.Guid).Guid;
            Debug.Log($"Changing to => {GetNodeByGuid(_currentNodeGuid).FormName}");
        }
        
        return (Forms) Enum.Parse(typeof(Forms),GetNodeByGuid(_currentNodeGuid).FormName);
    }

    public Forms ProceedToPreviousForm()
    {
        var previousNode = GetPreviousNodeByCurrentGuid(_currentNodeGuid);
        if (previousNode == null)
        {
            Debug.Log("There is no previous node"); return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
        }
        if (previousNode.IsBranch)
        {
            var link = _formPlanner.NodeLinks.FirstOrDefault(x => x.TargetNodeGuid == previousNode.Guid);
            if (link != null)
            {
                Debug.Log($"Degraded from => {GetNodeByGuid(_currentNodeGuid).FormName} to => {GetNodeByGuid(link.TargetNodeGuid).FormName}");
                _currentNodeGuid = link.TargetNodeGuid;
            }
        }
        else
        {
            _currentNodeGuid = GetNodeByGuid(previousNode.Guid).Guid;
            Debug.Log($"Changing to => {GetNodeByGuid(_currentNodeGuid).FormName}");
        }

        return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
    }
    FormNodeData GetNodeByGuid(string guid)
    {
        return _formPlanner.FormNodeDatas.FirstOrDefault(x => x.Guid == guid);
    }
    FormNodeData GetNextFormNodeByCurrentGuid(string currentGuid)
    {
        return GetNodeByGuid(_formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == currentGuid)?.TargetNodeGuid);
    }
    FormNodeData GetPreviousNodeByCurrentGuid(string currentGuid)
    {
        return GetNodeByGuid(_formPlanner.NodeLinks.FirstOrDefault(x => x.TargetNodeGuid == currentGuid).BaseNodeGuid);
    }
}
