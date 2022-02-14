using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleForm : AbstractFormBase
{
    public PurpleForm(Forms formType) : base(formType) { }

    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
