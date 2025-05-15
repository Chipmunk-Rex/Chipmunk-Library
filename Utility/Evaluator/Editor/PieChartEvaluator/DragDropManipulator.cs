// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Chipmunk.Library.Utility.Evaluator.Editor
// {
//     public class DragDropManipulator : PointerManipulator
//     {
//         public Action onDragEnd;
//         
//         private bool _isDragging = false;
//         private float _start;
//         private float _delta;
//         
//         private int button;
//         protected override void RegisterCallbacksOnTarget()
//         {
//             target.RegisterCallback<PointerDownEvent>(OnPointerDown);
//             target.RegisterCallback<PointerUpEvent>(OnPointerUp);
//             target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
//         }
//
//         protected override void UnregisterCallbacksFromTarget()
//         {
//             target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
//             target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
//             target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
//         }
//
//         private Vector2 targetStartPosition { get; set; }
//
//         private Vector3 pointerStartPosition { get; set; }
//
//         private bool enabled { get; set; }
//
//         private VisualElement root { get; }
//
//         private void OnPointerDown(PointerDownEvent evt)
//         {
//             if (evt.button == 0) // 좌클릭
//             {
//                 _isDragging = true;
//                 target.CaptureMouse();
//
//                 _start = target.transform.position.x;
//             }
//         }
//
//         private void OnPointerMove(PointerMoveEvent evt)
//         {
//             if (_isDragging == false) return;
//
//             moveFunc(_delta);
//             _delta += evt.deltaPosition.x;
//         }
//
//         private void OnPointerUp(PointerUpEvent evt)
//         {
//             if (_isDragging == false) return;
//
//             if (evt.button == button) // 좌클릭
//             {
//                 moveFunc(_delta);
//                 _delta = 0;
//
//                 _isDragging = false;
//                 target.ReleaseMouse();
//                 onDragEnd?.Invoke();
//             }
//         }
//
//         private void MoveElement(float delta)
//         {
//             var newPos = _start + _delta;
//             target.transform.position = new Vector2(newPos, target.transform.position.y);
//         }
//     }
// }