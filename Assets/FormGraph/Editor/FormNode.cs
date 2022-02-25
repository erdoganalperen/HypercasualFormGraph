using UnityEditor.Experimental.GraphView;

public class FormNode : Node
{
    public string GUID;
    public string FormName;
    public bool EntryPoint = false;
    public bool BranchNode = false;
    public BaseForm BaseForm = null;
}
