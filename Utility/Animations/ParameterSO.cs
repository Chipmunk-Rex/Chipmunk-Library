using System;
using UnityEngine;

namespace Chipmunk.Library.Utility.Animations
{
    [CreateAssetMenu(fileName = "Animator param", menuName = "Animations/Parameter", order = 20)]
    public class ParameterSO : ScriptableObject
    {
        [field:SerializeField] public string ParamName { get; private set; }
        [field:SerializeField] public int HashValue { get; private set; }

        private void OnValidate()
        {
            HashValue = Animator.StringToHash(ParamName);
        }
    }
}