using UnityEngine;

public class GreenForm : BaseForm
{
    public GreenForm(Forms formType) : base(formType) { }
    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
