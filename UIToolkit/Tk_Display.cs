using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tk_Display : TK_Parent
{
    [SerializeField] string _elementName;
    VisualElement _element;
    override protected void OnEnable()
    {
        base.OnEnable();
        if (_elementName != null)
        {
            _element = document.rootVisualElement.Q(_elementName);
        }
        if (_element == null)
        {
            _element = document.rootVisualElement;
        }
    }
    public void Flex()
    {
        _element.style.display = DisplayStyle.Flex;
    }
    public void None()
    {
        _element.style.display = DisplayStyle.None;
    }
    public void Toggle(bool isShow)
    {
        if (isShow)
        {
            Flex();
        }
        else
        {
            None();
        }
    }
    public void Show()
    {
        Flex();
    }
    public void Hide()
    {
        None();
    }
}
