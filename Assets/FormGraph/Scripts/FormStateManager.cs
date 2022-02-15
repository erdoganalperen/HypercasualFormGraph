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
    [SerializeField] private Branches _currentBranch=Branches.branch1;
    public List<AbstractFormBase> _formStateList;
    private void Awake()
    {
        //Application.targetFrameRate = 144;
        _formGraphParser = new FormGraphParser(graph);
        FormGraphParser.CurrentBranch = _currentBranch;
    }
    private void OnValidate()
    {
        FormGraphParser.CurrentBranch = _currentBranch;
    }
    private void Start()
    {
        SwitchState(_formStateList[0]);
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
    public bool NextForm()
    {
        var tempNextFormState = _formGraphParser.GetNextForm();
        if (tempNextFormState == _currentState.FormType) return false;

        AbstractFormBase abstractFormBase = _formStateList.FirstOrDefault(x => x.FormType == tempNextFormState);
        if (abstractFormBase == null) return false;

        Forms nextFormState = _formGraphParser.ProceedToNextForm();
        SwitchState(abstractFormBase);
        return true;
    }

    [NaughtyAttributes.Button("Previous")]
    public bool PreviousForm()
    {
        Forms previousFormState = _formGraphParser.ProceedToPreviousForm();
        if (previousFormState == _currentState.FormType) return false;

        AbstractFormBase abstractFormBase = _formStateList.FirstOrDefault(x => x.FormType == previousFormState);
        if (abstractFormBase == null) return false;
        SwitchState(abstractFormBase);
        return true;
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
        if(_currentState != null) _currentState.OnExit(this);
        _currentState = state;
        _currentState.OnStart(this);
    }
}
