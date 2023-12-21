using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_SO", menuName = "Datas/Data_SO")]
public class Data_SO<T> : ScriptableObject
{
    private T value;

    public T SetValue { set { AssignValue(value); } }
    public T GetValue { get { return ReturnValue(); } }


    protected virtual T ReturnValue ()
    {
        return value;
    }

    protected virtual void AssignValue(T newValue)
    {
         value = newValue;
    }
}
