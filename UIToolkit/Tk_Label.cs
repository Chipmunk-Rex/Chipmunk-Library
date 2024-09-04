using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
[AddComponentMenu("Chipmunk/Toolkit/Toolkit_Label")]
public class Tk_Label : Tk_Element<Label>
{
    private string text;
    public string Text
    {
        get => text;
        set
        {
            if (text != value)
            {
                onTextChangeEvent?.Invoke();
                _element.text = value;
            }
            text = value;
        }
    }
    public UnityEvent onTextChangeEvent;
    protected override void OnEnable()
    {
        base.OnEnable();
        text = _element.text;    
    }
}
