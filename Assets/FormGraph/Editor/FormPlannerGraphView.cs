using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
public class FormPlannerGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    private NodeSearchWindow _searchWindow;
    private BaseForm _baseForm;
    public FormPlannerGraphView(EditorWindow window)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("FormPlannerGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
        
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0,grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryNode());
        AddSearchWindow(window);
    }

    private void AddSearchWindow(EditorWindow window)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(window,this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }
    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }
    private FormNode GenerateEntryNode()
    {
        var node = new FormNode()
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            FormName = "Entry",
            EntryPoint = true
        };
        
        //generating output port for entry node
        var generatedPort = GeneratePort(node, Direction.Output,Port.Capacity.Single);
        generatedPort.portName = "Entry";
        node.outputContainer.Add(generatedPort);
        //
        node.capabilities &= ~Capabilities.Deletable;
        //
        node.RefreshExpandedState();
        node.RefreshPorts();
        //
        node.SetPosition(new Rect(0,0,100,100));
        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if(startPort!=port&&startPort.node!=port.node&&startPort.direction!=port.direction)
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }

    public FormNode CreateBranchNode(Vector2 position)
    {
        var formBranchNode = new FormNode()
        {
            GUID = Guid.NewGuid().ToString(),
            title = "Form branch",
            FormName = "Form branch",
            BranchNode = true,
        };
        var inputPort = GeneratePort(formBranchNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        formBranchNode.inputContainer.Add(inputPort);
        //
        foreach (Branches form in Enum.GetValues(typeof(Branches)))
        {
            AddOutput(formBranchNode,form.ToString());
            //AddEnumOutput(formBranchNode,form);
        }
        //
        formBranchNode.SetPosition(new Rect(position,
            defaultNodeSize));
        //
        formBranchNode.RefreshExpandedState();
        formBranchNode.RefreshPorts();
        formBranchNode.SetPosition(new Rect(position,defaultNodeSize));
        return formBranchNode;
    }

    public void AddBranchNode(Vector2 position)
    {
        AddElement(CreateBranchNode(position));
    }
    public void AddNode(string nodeName,Vector2 mousePosition)
    {
        AddElement(CreateFormNode(nodeName,mousePosition));
    }
    public FormNode CreateFormNode(string nodeName,Vector2 position, UnityEngine.Object baseForm=null)
    {
        var formNode = new FormNode()
        {
            title = baseForm ? baseForm.name : nodeName,
            FormName = nodeName,
            GUID = Guid.NewGuid().ToString(),
            BaseForm = baseForm as BaseForm
        };
        formNode.SetPosition(new Rect(position,
            defaultNodeSize));
        //INPUT CONTAINER
        var inputPort = GeneratePort(formNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        formNode.inputContainer.Add(inputPort);
        //OUTPUT CONTAINER
        AddOutput(formNode);
        //MAIN CONTAINER

        //create object field in main container
        var objField = new ObjectField()
        {
            objectType = typeof(BaseForm),
            allowSceneObjects = false,
            value = _baseForm
        };
        objField.RegisterValueChangedCallback(v => { _baseForm = v.newValue as BaseForm; formNode.title = _baseForm.name; formNode.BaseForm = _baseForm; });
        objField.SetValueWithoutNotify(baseForm);
        formNode.mainContainer.Add(objField);

        // create text field in main container
        //var textField = new TextField(string.Empty);
        //textField.RegisterValueChangedCallback(evt =>
        //{
        //    formNode.FormName = evt.newValue;
        //    formNode.title = evt.newValue;
        //});
        //textField.SetValueWithoutNotify(formNode.title);
        //formNode.mainContainer.Add(textField);
        //

        formNode.RefreshExpandedState();
        formNode.RefreshPorts();
        formNode.SetPosition(new Rect(position,defaultNodeSize));
        return formNode;
    }


    public void AddEnumOutput(Node node,Branches formPlan)
    {
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.styleSheets.Add(Resources.Load<StyleSheet>("Output"));

        // remove old label
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);
        //
        generatedPort.portName = formPlan.ToString();
        //
        generatedPort.contentContainer.Add(new Label(" "));
        //
        var enumField = new EnumField(formPlan);
        generatedPort.contentContainer.Add(enumField);
        //
        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
    public void AddOutput(Node node,string overridenPortName="")
    {
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.styleSheets.Add(Resources.Load<StyleSheet>("Output"));

        // remove old label
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);
        //
        //var outputPortCount = node.outputContainer.Query("connector").ToList().Count;
        //
        generatedPort.portName = string.IsNullOrEmpty(overridenPortName) ? "Next" : overridenPortName;
        //
        generatedPort.contentContainer.Add(string.IsNullOrEmpty(overridenPortName)
            ? new Label("Next")
            : new Label(overridenPortName));
        // output delete button
        // var deleteButton = new Button(() => RemovePort(node, generatedPort))
        // {
        //     text = "x"
        // };
        // generatedPort.contentContainer.Add(deleteButton);
        //
        node.outputContainer.Add(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    private void RemovePort(Node formNode, Port generatedPort)
    {
        var targetEdge = edges.ToList()
            .Where(x => x.output.portName == generatedPort.name && x.output.node == generatedPort.node);
        if(targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        };
        
        formNode.outputContainer.Remove(generatedPort);
        formNode.RefreshPorts();
        formNode.RefreshExpandedState();
    }
}