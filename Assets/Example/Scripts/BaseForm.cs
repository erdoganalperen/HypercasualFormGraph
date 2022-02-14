using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FormStateScriptableObject", order = 1)]
public class BaseForm : AbstractFormBase
{
    public int deneme;
    public BaseForm(Forms formType) : base(formType)
    {
    }
}
