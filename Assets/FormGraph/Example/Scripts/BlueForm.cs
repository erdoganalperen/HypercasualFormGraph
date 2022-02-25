using UnityEngine;

public class BlueForm : BaseForm
{
    public BlueForm(Forms formType) : base(formType) { }

    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
