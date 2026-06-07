using System;
using System.Collections.Generic;
using System.Linq;
using Chipmunk.Library.Utility.ComponentContainers;
using UnityEngine;

namespace Chipmunk.Modules.StatSystem
{
    public class StatBehavior : MonoBehaviour, IContainerComponent
    {
        protected Dictionary<string, StatSO> stats = new();
        public ComponentContainer ComponentContainer { get; set; }

        public virtual void OnInitialize(ComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        public void AddStat(StatSO stat)
        {
            if (stat == null)
            {
                Debug.LogWarning("Stats::AddStat : stat is null", this);
                return;
            }

            if (string.IsNullOrWhiteSpace(stat.statName))
            {
                Debug.LogWarning("Stats::AddStat : statName is empty", this);
                return;
            }

            if (stats.ContainsKey(stat.statName))
            {
                Debug.LogWarning(
                    $"Stats::AddStat : duplicated stat name `{stat.statName}`. Existing stat will be replaced.", this);
                stats[stat.statName] = stat;
                return;
            }

            stats.Add(stat.statName, stat);
        }

        public StatSO GetStat(StatSO targetStat)
        {
            if (targetStat == null)
            {
                Debug.LogWarning("Stats::GetStat : target stat is null", this);
                return null;
            }

            if (string.IsNullOrWhiteSpace(targetStat.statName))
            {
                Debug.LogWarning("Stats::GetStat : target statName is empty", this);
                return null;
            }

            if (stats.TryGetValue(targetStat.statName, out StatSO outStat))
            {
                return outStat;
            }

            Debug.LogWarning($"Stats::GetStat : `{targetStat.statName}` is not registered", this);
            return null;
        }

        public bool TryGetStat(StatSO targetStat, out StatSO outStat)
        {
            if (targetStat == null || string.IsNullOrWhiteSpace(targetStat.statName))
            {
                outStat = null;
                return false;
            }

            return stats.TryGetValue(targetStat.statName, out outStat);
        }

        public void SetBaseValue(StatSO stat, float value)
        {
            StatSO targetStat = GetStat(stat);
            if (targetStat != null)
            {
                targetStat.BaseValue = value;
            }
        }

        public float GetBaseValue(StatSO stat)
        {
            StatSO targetStat = GetStat(stat);
            return targetStat != null ? targetStat.BaseValue : default;
        }

        public void IncreaseBaseValue(StatSO stat, float value)
        {
            StatSO targetStat = GetStat(stat);
            if (targetStat != null)
            {
                targetStat.BaseValue += value;
            }
        }

        public void AddModifier(StatSO stat, object key, float value)
        {
            StatSO targetStat = GetStat(stat);
            if (targetStat != null)
            {
                targetStat.AddValueModifier(key, value);
            }
        }

        public void RemoveModifier(StatSO stat, object key)
        {
            StatSO targetStat = GetStat(stat);
            if (targetStat != null)
            {
                targetStat.RemoveModifier(key);
            }
        }

        public void CleanAllModifier()
        {
            foreach (StatSO stat in stats.Values)
            {
                stat.ClearModifier();
            }
        }

        public float SubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if (target == null) return defaultValue;
            target.OnValueChanged += handler;
            return target.Value;
        }

        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler)
        {
            StatSO target = GetStat(stat);
            if (target == null) return;
            target.OnValueChanged -= handler;
        }

        #region Save logic

        [Serializable]
        public struct StatSaveData
        {
            public string statName;
            public float baseValue;
        }

        public List<StatSaveData> GetSaveData()
            => stats.Values.Aggregate(new List<StatSaveData>(), (saveList, stat) =>
            {
                saveList.Add(new StatSaveData { statName = stat.statName, baseValue = stat.BaseValue });
                return saveList;
            });


        public void RestoreData(List<StatSaveData> loadedDataList)
        {
            if (loadedDataList == null)
            {
                Debug.LogWarning("Stats::RestoreData : loaded data list is null", this);
                return;
            }

            foreach (StatSaveData loadData in loadedDataList)
            {
                if (string.IsNullOrWhiteSpace(loadData.statName))
                {
                    continue;
                }

                if (stats.TryGetValue(loadData.statName, out StatSO targetStat))
                {
                    targetStat.BaseValue = loadData.baseValue;
                }
            }
        }

        #endregion

        public List<StatSO> GetAllStats() => stats.Values.ToList();
    }
}