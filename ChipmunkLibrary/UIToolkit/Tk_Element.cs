using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tk_Element<T> : TK_Parent where T : VisualElement
{
    [SerializeField] protected string _elementName;
    protected T _element;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (_elementName.Contains(@"\"))
        {
            string[] parrentNames = _elementName.Split(@"\");
            VisualElement element = document.rootVisualElement;
            foreach (string parentName in parrentNames)
            {
                element = element.Q(parentName);
            }
            _element = element as T;
        }
        else
        {
            _element = document.rootVisualElement.Q(_elementName) as T;
        }
    }
}
