using System;
using System.Linq;
using UnityEngine;

public class FormGraphParser
{
    private FormPlannerContainer _formPlanner;
    private string _currentNodeGuid;
    //enums
    public static Branches CurrentBranch;
    //
    public FormGraphParser(FormPlannerContainer graph)
    {
        _formPlanner = graph;
        _currentNodeGuid = _formPlanner.NodeLinks.First().BaseNodeGuid;
        ProceedToNextForm();
    }
    public Forms GetNextForm()
    {
        var nextNode = GetNextFormNodeByCurrentGuid(_currentNodeGuid);
        string nextNodeGuid;
        if (nextNode == null)
        {
            Debug.Log("There is no next node"); return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
        }
        if (nextNode.IsBranch)
        {
            var link = _formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == nextNode.Guid && x.PortName == CurrentBranch.ToString());
            if (link != null)
            {
                nextNodeGuid = link.TargetNodeGuid;
            }
            else
            {
                Debug.Log("There is no next node"); return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
            }
        }
        else
        {
            nextNodeGuid = GetNodeByGuid(nextNode.Guid).Guid;
        }

        return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(nextNodeGuid).FormName);
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
            var link = _formPlanner.NodeLinks.FirstOrDefault(x => x.BaseNodeGuid == nextNode.Guid && x.PortName == CurrentBranch.ToString());
            if (link != null)
            {
                Debug.Log(link.PortName);
                _currentNodeGuid = link.TargetNodeGuid;
            }
            else
            {
                Debug.Log("There is no next node"); return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
            }
        }
        else
        {
            _currentNodeGuid = GetNodeByGuid(nextNode.Guid).Guid;
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
            Debug.Log("Previous node is a branch, there is no turning back from branch"); return (Forms)Enum.Parse(typeof(Forms), GetNodeByGuid(_currentNodeGuid).FormName);
        }
        else
        {
            _currentNodeGuid = GetNodeByGuid(previousNode.Guid).Guid;
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
