using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
[AddComponentMenu("Chipmunk/Toolkit/Toolkit_Document")]
public class Tk_Document : TK_Parent
{
    [SerializeField] public bool showOnStart = false;
    [SerializeField] public UnityEvent onShow;
    [SerializeField] public UnityEvent onHide;
    public List<TK_Parent> tkScripts;
    protected override void OnEnable()
    {
        base.OnEnable();

        tkScripts = transform.GetComponents<TK_Parent>().ToList();
        tkScripts.ForEach(a => a.document = document);

        if (!showOnStart)
            Hide();
        else
            Show();
    }
    public void Show()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
        document.rootVisualElement.style.display = DisplayStyle.Flex;
        onShow?.Invoke();
    }
    public void Hide()
    {
        document.rootVisualElement.style.display = DisplayStyle.None;
        onHide?.Invoke();
    }
}
