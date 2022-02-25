using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleForm : BaseFormClass
{
    public PurpleForm(Forms formType) : base(formType) { }

    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}

[Serializable]
public class Form{
    public GameObject gameObject;
}