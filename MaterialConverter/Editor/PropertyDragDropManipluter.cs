using UnityEditor;
        using UnityEngine;
        using UnityEngine.UIElements;
        
        namespace Chipmunk.Library.MaterialConverter.Editor
        {
            public class PropertyDragDropManipulator : Manipulator
            {
                private bool _isDragging;
                private Vector2 _startPosition;
                private readonly float _dragThreshold = 5f;
                private readonly System.Action<PropertyView, PropertyView> _onPropertyMapped;
        
                public PropertyDragDropManipulator()
                {
                }
        
                protected override void RegisterCallbacksOnTarget()
                {
                    target.RegisterCallback<PointerDownEvent>(OnPointerDown);
                    target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
                    target.RegisterCallback<PointerUpEvent>(OnPointerUp);
                }
        
                protected override void UnregisterCallbacksFromTarget()
                {
                    target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
                    target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
                    target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
                }
        
                private void OnPointerDown(PointerDownEvent evt)
                {
                    if (evt.button == 0) // Left mouse button
                    {
                        _startPosition = evt.position;
                        _isDragging = true;
                        target.CaptureMouse();
                        evt.StopPropagation();
                    }
                }
        
                private void OnPointerMove(PointerMoveEvent evt)
                {
                    if (!_isDragging || !target.HasMouseCapture())
                        return;
        
                    // Start dragging if moved past threshold
                    Vector2 diff = (Vector2)evt.position - _startPosition;
                    if (diff.magnitude > _dragThreshold)
                    {
                        PropertyView propertyView = target as PropertyView;
                        if (propertyView == null)
                            return;
        
                        // Start drag operation
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.SetGenericData("PropertyView", propertyView);
                        
                        // Set visual mode before starting drag
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
        
                        // Get property name from the label
                        Label nameLabel = propertyView.Q<Label>("PropertyNameText");
                        string displayName = nameLabel?.text ?? "Property";
        
                        DragAndDrop.StartDrag(displayName);
                        _isDragging = false;
                        target.ReleaseMouse();
                    }
                }
                private void OnPointerUp(PointerUpEvent evt)
                {
                    if (target.HasMouseCapture())
                    {
                        target.ReleaseMouse();
                        _isDragging = false;
                    }
                }
            }
        }