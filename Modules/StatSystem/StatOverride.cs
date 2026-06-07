using System;
using UnityEngine;

namespace Chipmunk.Modules.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [SerializeField] private StatSO stat;
        [SerializeField] private bool isUseOverride;
        [SerializeField] private float overrideValue;

        public StatOverride(StatSO stat) => this.stat = stat;

        public StatSO CreateStat()
        {
            if (stat == null)
            {
                Debug.LogWarning("StatOverride::CreateStat : stat is null");
                return null;
            }

            StatSO newStat = stat.Clone() as StatSO;
            if (newStat == null)
            {
                Debug.LogError($"StatOverride::CreateStat : {stat.name} clone failed");
                return null;
            }

            if (isUseOverride)
            {
                newStat.BaseValue = overrideValue;
            }

            return newStat;
        }
    }
}
