using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.Utility.Evaluator.Editor
{
    public class PieChartView : VisualElement
    {
        public float radius
        {
            get => style.width.value.value / 2;
        }

        public float diameter => radius * 2.0f;
        public PieChartEvaluator pieChartEvaluator;

        public PieChartView(PieChartEvaluator<T> pieChartEvaluator)
        {
            this.pieChartEvaluator = pieChartEvaluator;
            this.AddToClassList("pie-chart-view");
            generateVisualContent += DrawCanvas;
        }

        public void Populate()
        {
            this.Clear();

            CreatePoint(pieChartEvaluator.pieChartDataList);
            SetAngle();
            MarkDirtyRepaint();
        }

        private void CreatePoint(List<PieChartData<T>> pieChartDatas)
        {
            for (int i = 0; i < pieChartDatas.Count; i++)
            {
                Initailize<> initailize = new Initailize<>(this, pieChartDatas[i]);
                pieChartPoints[i] = initailize;
                initailize.onAngleValueChanged += OnPointValueChanged;
                initailize.onSelect += OnSelectPoint;
                this.Add(initailize);
            }
        }

        private void OnSelectPoint(Initailize<> point)
        {
            onSelect?.Invoke(point.chartData);
        }

        private void OnPointValueChanged(float value, Initailize<> point)
        {
            SetAngle();
            onDataChanged?.Invoke(point.chartData);
        }

        public void SetAngle()
        {
            for (int i = 0; i < pieChartPoints.Length; i++)
            {
                SetMaxAngle(i);
                SetMinAngle(i);
            }

            MarkDirtyRepaint();
        }

        private void SetMinAngle(int index)
        {
            Initailize<> point = pieChartPoints[index];
            float minAngle = 0;
            if (index > 0)
            {
                minAngle = pieChartPoints[index - 1].angle;
                // Debug.Log("SetMinAngle" + index + " " + minAngle);
            }

            point.SetMinAngle(minAngle);
        }

        private Initailize<> SetMaxAngle(int index)
        {
            Initailize<> point = pieChartPoints[index];
            if (index < pieChartPoints.Length - 1)
            {
                point.SetMaxAngle(pieChartPoints[index + 1].angle);
            }
            else
            {
                // Debug.Log("SetMaxAngle" + index);
                float maxAngle = 360.0f;
                point.SetMaxAngle(maxAngle);
            }

            return point;
        }

        void DrawCanvas(MeshGenerationContext ctx)
        {
            Painter2D painter = ctx.painter2D;
            painter.strokeColor = Color.white;
            painter.fillColor = Color.white;

            float anglePct = 0.0f;
            float angleStart = 0.0f;
            var targetDraws = pieChartEvaluator.pieChartDataList;

            for (int i = 0; i < targetDraws.Count; i++)
            {
                PieChartData<T> pieChartData = targetDraws[i];
                float angle = 360.0f * pieChartData.percentage;

                anglePct = angle;

                float fillRadius = this.radius;

                painter.fillColor = pieChartData.color;
                painter.BeginPath();
                painter.MoveTo(new Vector2(fillRadius, fillRadius));
                painter.Arc(new Vector2(fillRadius, fillRadius), fillRadius, angleStart, anglePct);
                painter.Fill();

                angleStart = angle;
            }
        }
    }
}