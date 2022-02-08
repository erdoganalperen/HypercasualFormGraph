using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormBaseState
{
    public Forms FormType;
    protected FormBaseState(Forms formType)
    {
        Init(formType);
    }
    private void Init(Forms type)
    {
        FormType = type;
    }
    public abstract void OnStart(FormStateManager manager);
    public abstract void OnUpdate(FormStateManager manager);
    public abstract void OnTrigger(FormStateManager manager);
}
