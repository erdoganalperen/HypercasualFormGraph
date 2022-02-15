using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FormStateScriptableObject", order = 1)]
public class GreenForm : BaseForm
{
    public GreenForm(Forms formType) : base(formType) { }
    public int deneme;
    public override void OnStart(FormStateManager manager)
    {
        base.OnStart(manager);
        manager.CloseAllFormsExceptSpecified(FormType);
    }
}
