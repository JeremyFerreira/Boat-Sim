﻿using System;
using UnityEngine;


    public abstract class InputScriptableObject<T> : ScriptableObject
    {
        public event Action<T> OnValueChanged;
        [SerializeField] bool isActive = true;
        public bool IsActive { get { return isActive; } set { if (!value) { ChangeValue(default); } isActive = value; } }
        public virtual void ChangeValue(T value)
        {
            if(isActive)
            {
                OnValueChanged?.Invoke(value);
            }
        }
}
