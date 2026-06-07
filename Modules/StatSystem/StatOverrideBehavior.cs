using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Modules.StatSystem
{
    public class StatOverrideBehavior : StatBehavior
    {
        [SerializeField] private StatOverride[] statOverrides;

        public override void OnInitialize(ComponentContainer componentContainer)
        {
            base.OnInitialize(componentContainer);

            stats.Clear();
            if (statOverrides == null || statOverrides.Length == 0)
            {
                return;
            }

            for (int index = 0; index < statOverrides.Length; index++)
            {
                StatOverride statOverride = statOverrides[index];
                if (statOverride == null)
                {
                    continue;
                }

                StatSO stat = statOverride.CreateStat();
                AddStat(stat);
            }
        }
    }
}