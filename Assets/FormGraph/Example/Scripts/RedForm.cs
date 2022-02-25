using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedForm : BaseFormClass
{
    public RedForm(Forms type):base(type) { }

    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
