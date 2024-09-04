using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(Tk_Document))]
public abstract class TK_Parent : MonoBehaviour
{
    [SerializeField] public UIDocument document;
    protected virtual void OnEnable()
    {
        if (document == null)
            document = GetComponent<UIDocument>();
    }
}
