using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFormBase : ScriptableObject
{
    public Forms FormType;

    protected AbstractFormBase(Forms formType)
    {
        Init(formType);
    }

    public void Init(Forms type)
    {
        FormType = type;
    }

    public virtual void OnStart(FormStateManager manager)
    {
        Debug.Log($"form {FormType} has started");
    }

    public virtual void OnUpdate(FormStateManager manager)
    {
        Debug.Log($"form {FormType} is updating");
    }

    public virtual void OnExit(FormStateManager manager)
    {
        Debug.Log($"Exited from {FormType}.");

    }
    public virtual void OnTrigger(FormStateManager manager)
    {
        Debug.Log($"form {FormType} has triggered");
    }
}
