using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenForm : FormBaseState
{
    public GreenForm(Forms formType) : base(formType)
    {
    }
    public override void OnStart(FormStateManager manager)
    {
        Debug.Log($"form {FormType} has started");
    }

    public override void OnUpdate(FormStateManager manager)
    {
    }

}
