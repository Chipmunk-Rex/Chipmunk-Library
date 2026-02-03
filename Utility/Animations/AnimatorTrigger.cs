using System;
using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Library.Utility.Animations
{
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class AnimatorTrigger : MonoBehaviour, IContainerComponent
    {
        public event Action OnAnimationEnd;
        public event Action OnAnimationTrigger;
        public event Action<TriggerTypeSO> OnAttackTrigger;

        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
        }

        public void AnimationTrigger() => OnAnimationTrigger?.Invoke();
        public void AnimationEnd() => OnAnimationEnd?.Invoke();

        protected virtual void OnOnAttackTrigger(TriggerTypeSO obj)
        {
            OnAttackTrigger?.Invoke(obj);
        }
    }
}