using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chipmunk.Library.Utility.Evaluator
{
    [Serializable]
    public class PieChartEvaluator<T>
    {
        [HideInInspector] [SerializeField] internal List<PieChartData<T>> pieChartDataList = new();

        public T Evaluate(float t)
        {
            if (pieChartDataList.Count == 0)
            {
                Debug.LogError("PieChartEvaluator: No data available");
                return default;
            }


            float total = 0;
            foreach (var data in pieChartDataList)
            {
                total += data.percentage;
            }

            float target = t * total;

            foreach (var data in pieChartDataList)
            {
                if (target <= data.percentage)
                {
                    return data.data;
                }

                target -= data.percentage;
            }

            return default;
        }
    }
    [Serializable]
    internal class PieChartData<T>
    {
        [Range(0, 1)] public float percentage = 0;
        public Color color = Color.green;
        public T data;

        public PieChartData(float percentage, Color color)
        {
            this.percentage = percentage;
        }
    }
}