using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tk_Singler : MonoBehaviour
{
    List<Tk_Document> _Documents = new();
    Tk_Document _CurrentDocument;
    protected void Awake()
    {
        transform.GetComponentsInChildren<Tk_Document>(true, _Documents);
        foreach (Tk_Document doc in _Documents)
        {
            doc.onShow.AddListener(() => OnShowDocument(doc));
            doc.onHide.AddListener(() => OnHideDocument(doc));

            if (doc.showOnStart)
                OnShowDocument(doc);
        }
    }

    private void OnShowDocument(Tk_Document doc)
    {
        if (_CurrentDocument != null && _CurrentDocument != doc)
            _CurrentDocument.Hide();
        _CurrentDocument = doc;
    }
    private void OnHideDocument(Tk_Document doc)
    {
        if (_CurrentDocument == doc) _CurrentDocument = null;
    }
}
