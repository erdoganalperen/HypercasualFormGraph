using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class GraphSaveUtility
{
    private FormPlannerGraphView _targetGraphView;
    private FormPlannerContainer _containerCache;
    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<FormNode> Nodes => _targetGraphView.nodes.ToList().Cast<FormNode>().ToList();
    public static GraphSaveUtility GetInstance(FormPlannerGraphView targetGraphView)
    {
        return new GraphSaveUtility()
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string filename)
    {
        if (!Edges.Any()) return;
        var formContainer = ScriptableObject.CreateInstance<FormPlannerContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        //Saving edges and nodes
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as FormNode;
            var inputNode = connectedPorts[i].input.node as FormNode;
            formContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName, // <-
                TargetNodeGuid = inputNode.GUID
            });
        }
        foreach (var formNode in Nodes.Where(node=>!node.EntryPoint))
        {
            formContainer.FormNodeDatas.Add(new FormNodeData
            {
                Guid = formNode.GUID,
                FormName = formNode.FormName,
                Position = formNode.GetPosition().position,
                IsBranch = formNode.BranchNode,
            });
        }
        //creating file for saved datas
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        AssetDatabase.CreateAsset(formContainer,$"Assets/Resources/{filename}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string filename)
    {
        _containerCache = Resources.Load<FormPlannerContainer>(filename);
        if (_containerCache==null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                // matching correct output with input
                foreach (var item in Nodes[i].outputContainer.Children())
                {
                    if (item.Q<Port>().portName == connections[j].PortName)
                    {
                        LinkNodes(item.Q<Port>(), (Port)targetNode.inputContainer[0]);
                    }
                }

                targetNode.SetPosition(new Rect(
                    _containerCache.FormNodeDatas.First(x=>x.Guid==targetNodeGuid).Position,
                    _targetGraphView.defaultNodeSize));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        
        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.FormNodeDatas)
        {
            FormNode tempNode;
            if (nodeData.IsBranch)
                tempNode = _targetGraphView.CreateFormBranchNode(Vector2.zero);
            else 
                tempNode = _targetGraphView.CreateFormNode(nodeData.FormName,Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            //var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            //nodePorts.ForEach(x=>_targetGraphView.AddOutput(tempNode,x.PortName));
        }
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;
        foreach (var node in Nodes)
        {
            if(node.EntryPoint) continue;
            
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge=>_targetGraphView.RemoveElement(edge));
            
            _targetGraphView.RemoveElement(node);
        }
    }
}
