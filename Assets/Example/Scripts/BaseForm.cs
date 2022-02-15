using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaseFormData", order = 1)]
public class BaseForm : AbstractFormBase
{
    public BaseForm(Forms formType) : base(formType)
    {
    }
    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(base.FormType);
    }
    public override void OnUpdate(FormStateManager manager)
    {
        base.OnUpdate(manager);
    }
}
