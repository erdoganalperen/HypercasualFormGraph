using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/BaseFormWithData", order = 2)]
public class BaseFormClass : AbstractFormBase<FormStateManager>
{
    public Form data;
    public BaseFormClass(Forms formType) : base(formType)
    {
    }
    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        Debug.Log(data.gameObject.name);
    }
    public override void OnUpdate(FormStateManager manager)
    {
        base.OnUpdate(manager);
    }
}
