using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleForm : FormBaseState
{
    public PurpleForm(Forms formType) : base(formType) { }

    public override void OnStart(FormStateManager manager)
    {
        Debug.Log($"form {FormType} has started");
        manager.CloseAllFormsExceptSpecified(FormType);
    }

    public override void OnTrigger(FormStateManager manager)
    {
    }

    public override void OnUpdate(FormStateManager manager)
    {
    }

}
