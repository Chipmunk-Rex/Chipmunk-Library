using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    public class PropertyDragTargetManipluter : Manipulator
    {
        private readonly Action<PropertyView> _onPropertyMapped;
        private Func<PropertyView, bool> CanLinkFunc { get; set; }

        public PropertyDragTargetManipluter(Action<PropertyView> onPropertyMapped,
            Func<PropertyView, bool> canLinkFunc = null)
        {
            _onPropertyMapped = onPropertyMapped;
            CanLinkFunc = canLinkFunc;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<DragEnterEvent>(OnDragEnter);
            target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
            target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
            target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
        }

        private void OnDragEnter(DragEnterEvent evt)
        {
            PropertyView sourceView = DragAndDrop.GetGenericData("PropertyView") as PropertyView;

            bool canLink = CanLinkFunc?.Invoke(sourceView) ?? true;
            if (canLink)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                target?.AddToClassList("drag-hover");
            }

            evt.StopPropagation();
        }

        private void OnDragLeave(DragLeaveEvent evt)
        {
            target?.RemoveFromClassList("drag-hover");
        }

        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            PropertyView sourceView = DragAndDrop.GetGenericData("PropertyView") as PropertyView;

            bool canLink = CanLinkFunc?.Invoke(sourceView) ?? true;
            if (canLink)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                evt.StopPropagation();
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            PropertyView sourceView = DragAndDrop.GetGenericData("PropertyView") as PropertyView;
            if (sourceView == null) return;

            if (CanLinkFunc?.Invoke(sourceView) ?? true)
            {
                _onPropertyMapped.Invoke(sourceView);
            }

            target?.RemoveFromClassList("drag-hover");
            DragAndDrop.AcceptDrag();
        }
    }
}