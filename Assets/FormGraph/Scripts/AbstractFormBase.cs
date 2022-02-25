using UnityEngine;

/// <summary>
/// Base class that contain simple start, update and exit methods
/// </summary>
/// <typeparam name="T">Manager type that functions use</typeparam>
public abstract class AbstractFormBase<T> : ScriptableObject
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

    public virtual void OnStart(T manager)
    {
        Debug.Log($"<color=#ffa500ff>{FormType}</color> form has started");
    }

    public virtual void OnUpdate(T manager)
    {
        Debug.Log($"form {FormType} is updating");
    }

    public virtual void OnExit(T manager)
    {
        Debug.Log($"Exited from {FormType}.");

    }
    public virtual void OnTrigger(T manager, Collider other)
    {
        Debug.Log($"form {FormType} has triggered");
    }
}

/// <summary>
/// Base class that contain simple start, update and exit methods, also contain a class for custom data
/// </summary>
/// <typeparam name="T"><Manager that functions use/typeparam>
/// <typeparam name="T2">Data class</typeparam>
public abstract class AbstractFormBase<T,T2> : ScriptableObject
{
    public Forms FormType;
    public T2 data;
    protected AbstractFormBase(Forms formType)
    {
        Init(formType);
    }

    public void Init(Forms type)
    {
        FormType = type;
    }

    public virtual void OnStart(T manager)
    {
        Debug.Log($"form {FormType} has started");
    }

    public virtual void OnUpdate(T manager)
    {
        Debug.Log($"form {FormType} is updating");
    }

    public virtual void OnExit(T manager)
    {
        Debug.Log($"Exited from {FormType}.");

    }
    public virtual void OnTrigger(T manager,Collider other)
    {
        Debug.Log($"form {FormType} has triggered");
    }
}
