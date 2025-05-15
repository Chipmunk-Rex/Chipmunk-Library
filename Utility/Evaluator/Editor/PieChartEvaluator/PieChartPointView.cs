using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.Utility.Evaluator.Editor
{
    [UxmlElement]
    public partial class PieChartPointView : VisualElement
    {
        float radius;

        PieChartView pieChartView;
        public Action<float, Initailize<>> onAngleValueChanged;
        public Action<Initailize<>> onSelect;
        public PieChartData chartData { get; private set; }
        public float angle;

        public void Initailize(PieChartView pieChartView, PieChartData pieChartData)
        {
            this.pieChartView = pieChartView;
            this.chartData = pieChartData;
            this.radius = pieChartView.radius;

            SetStyle(pieChartData);

            angle = (pieChartData.percentage / 100) * 360;

            drangNDropManipulator = new RangeDragAndDropManipulator(this, pieChartView, radius);
            drangNDropManipulator.Angle = angle;
            drangNDropManipulator.onAngleChanged += OnAngleValueChanged;
            this.AddManipulator(drangNDropManipulator);

            // SetPos(pieChartData.percentage, pieChartView.radius);

            this.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        }

        private void SetStyle(PieChartData<T> pieChartData)
        {
            this.AddToClassList("pie-chart-point");

            this.style.width = 10;
            this.style.height = 10;

            this.style.backgroundColor = pieChartData.color;

            this.style.borderTopColor = Color.black;
            this.style.borderBottomColor = Color.black;
            this.style.borderLeftColor = Color.black;
            this.style.borderRightColor = Color.black;

            this.style.borderTopWidth = 1;
            this.style.borderBottomWidth = 1;
            this.style.borderLeftWidth = 1;
            this.style.borderRightWidth = 1;

            this.style.borderTopLeftRadius = 50;
            this.style.borderTopRightRadius = 50;
            this.style.borderBottomLeftRadius = 50;
            this.style.borderBottomRightRadius = 50;

            this.style.position = Position.Absolute;
        }

        public void SetMaxAngle(float maxAngle)
        {
            if (drangNDropManipulator.Angle > maxAngle)
            {
                drangNDropManipulator.Angle = maxAngle;
            }

            drangNDropManipulator.maxAngle = maxAngle;
        }

        public void SetMinAngle(float minAngle)
        {
            if (drangNDropManipulator.Angle < minAngle)
            {
                drangNDropManipulator.Angle = minAngle;
            }

            drangNDropManipulator.minAngle = minAngle;
        }

        private void OnAngleValueChanged(float angle)
        {
            Debug.Log("angleChanged");
            this.angle = angle;
            chartData.percentage = (angle / 360) * 100;
            onAngleValueChanged?.Invoke(angle, this);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            onSelect?.Invoke(this);
        }
    }
}