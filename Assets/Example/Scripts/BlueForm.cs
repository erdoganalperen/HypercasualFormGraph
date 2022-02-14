using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueForm : AbstractFormBase
{
    public BlueForm(Forms formType) : base(formType) { }

    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
