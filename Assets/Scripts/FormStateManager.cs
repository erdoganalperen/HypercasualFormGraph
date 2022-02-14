using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormStateManager : MonoBehaviour
{
    //graph fields
    private FormGraphParser _formGraphParser;
    [SerializeField] private FormPlannerContainer graph;
    //other
    private AbstractFormBase _currentState;
    private List<AbstractFormBase> _formStateList;
    public List<GreenForm> _test;
    private void Awake()
    {
        //Application.targetFrameRate = 144;
        _formGraphParser = new FormGraphParser(graph);
        // creating instance of every state and add to list
        _formStateList = new List<AbstractFormBase>()
        {
            new RedForm(Forms.red),
            new GreenForm(Forms.green),
            new BlueForm(Forms.blue),
            new PurpleForm(Forms.purple)
        };
        foreach (var item in _test)
        {
            item.Init(item.FormType);
        }
    }

    private void Start()
    {
        //SwitchState(_formStateList[0]);
        SwitchState(_test[0]);
    }

    private void Update()
    {
        _currentState.OnUpdate(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentState.OnTrigger(this);
    }

    [NaughtyAttributes.Button("Next")]
    public void NextForm()
    {
        Forms nextFormState = _formGraphParser.ProceedToNextForm();
        if (nextFormState == _currentState.FormType) return;

        SwitchState(_formStateList.FirstOrDefault(x=>x.FormType==nextFormState));
    }
    [NaughtyAttributes.Button("Previous")]
    public void PreviousForm()
    {
        Forms previousFormState = _formGraphParser.ProceedToPreviousForm();
        if (previousFormState == _currentState.FormType) return;

        SwitchState(_formStateList.FirstOrDefault(x => x.FormType == previousFormState));
    }
    public void CloseAllFormsExceptSpecified(Forms form)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == form.ToString())
            {
                child.gameObject.SetActive(true);
                continue;
            }
            child.gameObject.SetActive(false);
        }
    }
    private void SwitchState(AbstractFormBase state)
    {
        _currentState = state;
        state.OnStart(this);
    }
}
