using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
[AddComponentMenu("Chipmunk/Toolkit/Toolkit_Btn")]
public class Tk_BtnClick : Tk_Element<Button>
{
    [SerializeField] public UnityEvent<ClickEvent> onBtnClick;
    protected override void OnEnable()
    {
        base.OnEnable();
        _element.RegisterCallback<ClickEvent>(OnClick);
    }
    private void OnDisable()
    {
        if (_element != null)
            _element.UnregisterCallback<ClickEvent>(OnClick);
    }
    public void OnClick(ClickEvent clickEvent)
    {
        onBtnClick?.Invoke(clickEvent);
    }
}
