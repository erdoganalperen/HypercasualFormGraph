using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/BaseForm", order = 1)]
public class BaseForm : AbstractFormBase<FormStateManager>
{
    public BaseForm(Forms formType) : base(formType)
    {
    }
    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);

    }
    public override void OnUpdate(FormStateManager manager)
    {
        base.OnUpdate(manager);
    }
}

