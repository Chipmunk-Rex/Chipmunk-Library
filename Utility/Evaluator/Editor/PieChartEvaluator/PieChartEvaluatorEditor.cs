using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.Utility.Evaluator.Editor
{
    [CustomPropertyDrawer(typeof(PieChartEvaluator<>))]
    public class PieChartEvaluatorEditor : PropertyDrawer
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return visualTreeAsset.CloneTree();
        }
    }
}