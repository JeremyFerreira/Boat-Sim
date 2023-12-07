using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InputData/button")]
public class InputButtonScriptableObject : InputScriptableObject<bool>
{
    public bool value;
    public override void ChangeValue (bool newValue)
    {
        value = newValue;
        base.ChangeValue (newValue);
    }
}
