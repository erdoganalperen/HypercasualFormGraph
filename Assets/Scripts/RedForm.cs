using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedForm : FormBaseState
{
    public RedForm(Forms type):base(type)
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
