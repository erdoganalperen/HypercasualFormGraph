using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormStateManager : MonoBehaviour
{
    public FormParser formGraphParser;
    private FormBaseState _currentState;
    private List<FormBaseState> formStates;
    public bool upgrade,degrade;

    private void Awake()
    {
        Application.targetFrameRate = 144;

        formStates = new List<FormBaseState>()
        {
            new RedForm(Forms.red),
            new GreenForm(Forms.green),
            new BlueForm(Forms.blue)
        };
    }

    private void Start()
    {
        _currentState = formStates[0];
        _currentState.OnStart(this);
    }

    private void Update()
    {
        _currentState.OnUpdate(this);
        if (upgrade)
        {
            UpgradeForm();
            upgrade = false;
        }
        if (degrade)
        {
            DegradeForm();
            degrade = false;
        }
    }
    
    void UpgradeForm()
    {
        var nextFormState = formGraphParser.ProceedToNextForm();
        SwitchState(formStates.FirstOrDefault(x=>x.FormType==nextFormState));
    }
    void DegradeForm()
    {
    }
    void SwitchState(FormBaseState state)
    {
        _currentState = state;
        state.OnStart(this);
    }
}
