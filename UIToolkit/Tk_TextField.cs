using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Tk_TextField : Tk_Element<TextField>
{
    public string Text
    {
        get => _element.text;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
