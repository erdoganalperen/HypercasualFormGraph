using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private FormPlannerGraphView _graphView;
    private EditorWindow _window;
    private Texture2D _indentationIcon;
    public void Init(EditorWindow window,FormPlannerGraphView graphView)
    {
        _graphView = graphView;
        _window = window;

        _indentationIcon = new Texture2D(1, 1);
        _indentationIcon.SetPixel(0,0,new Color(0,0,0,0));
        _indentationIcon.Apply();
    }
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Crate element"), 0),
            new SearchTreeGroupEntry(new GUIContent("Forms"),1),
            new SearchTreeEntry(new GUIContent("Form",_indentationIcon))
            {
                userData = new FormNode(),level = 2
            },
            new SearchTreeEntry(new GUIContent("Form Branch",_indentationIcon))
            {
                userData = new FormBranchNode(),level = 2
            },
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
            context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        switch (SearchTreeEntry.userData)
        {
            case FormNode formNode:
                _graphView.AddNode("form node",localMousePosition);
                return true;
            case FormBranchNode formBranchNode:
                _graphView.AddBranchNode(localMousePosition);
                return true;
            default:
                return false;
        }
    }
}
