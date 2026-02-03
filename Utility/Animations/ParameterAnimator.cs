using System;
using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Library.Utility.Animations
{
    public class ParameterAnimator : MonoBehaviour, IContainerComponent
    {
        [SerializeField] private Animator animator;

        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
        }

        private void Awake()
        {
            Debug.Assert(animator != null, $"Animator is not assigned in {gameObject.name}");
        }

        public void SetParameter(ParameterSO parameter) => animator.SetTrigger(parameter.HashValue);
        public void SetParameter(ParameterSO parameter, bool value) => animator.SetBool(parameter.HashValue, value);
        public void SetParameter(ParameterSO parameter, float value) => animator.SetFloat(parameter.HashValue, value);
        public void SetParameter(ParameterSO parameter, int value) => animator.SetInteger(parameter.HashValue, value);
    }
}