using UnityEditor;
using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    public class PropertyView : VisualElement
    {
        Label _propertyNameLabel;
        Label _propertyTypeLabel;
        
        string _propertyName;
        ShaderUtil.ShaderPropertyType _propertyType;
        public PropertyView(VisualTreeAsset _visualTreeAsset)
        {
            _visualTreeAsset.CloneTree(this);
        }

        public void Initialize(string propertyName, ShaderUtil.ShaderPropertyType propertyType)
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
            
            GetElements();
            InitializeElements();
        }

        private void InitializeElements()
        {
            _propertyNameLabel.text = _propertyName;
            _propertyTypeLabel.text = _propertyType.ToString();
        }

        private void GetElements()
        {
            _propertyTypeLabel = this.Q<Label>("PropertyTypeText");
            _propertyNameLabel = this.Q<Label>("PropertyNameText");
        }
    }
}