using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    public class PropertyView : VisualElement
    {
        Label _propertyNameLabel;
        Label _propertyTypeLabel;

        public string propertyName;
        public ShaderUtil.ShaderPropertyType propertyType;
        public Action<PropertyLink> AddPropertyLink;

        public bool isSource = false;

        public PropertyView(VisualTreeAsset _visualTreeAsset, Action<PropertyLink> AddPropertyLinkCallback, bool isSource = false)
        {
            this.isSource = isSource;
            
            this.AddPropertyLink = AddPropertyLinkCallback;
            
            PropertyDragDropManipulator dragDropManipulator = new PropertyDragDropManipulator();
            PropertyDragTargetManipluter dragTargetManipulator = new PropertyDragTargetManipluter(OnPropertyMapped, CanLinkWith);
            
            this.AddManipulator(dragDropManipulator);
            this.AddManipulator(dragTargetManipulator);
            
            _visualTreeAsset.CloneTree(this);
            this.AddToClassList("container");
        }

        private bool CanLinkWith(PropertyView arg)
        {
            if (arg == null)
                return false;
            
            if(arg.isSource == this.isSource)
                return false;

            if (this.propertyType != arg.propertyType)
                return false;

            if (this.propertyName == arg.propertyName)
                return false;

            return true;
        }

        private void OnPropertyMapped(PropertyView target)
        {
            if (target == null)
                return;

            if (target.propertyType == propertyType)
            {
                Debug.Log($"Property Mapped: {target.propertyName} of type {propertyType}");
                PropertyLink newPropertyLink = new PropertyLink
                {
                    sourceType = propertyType,
                    sourceProperty = this.isSource ? this.propertyName : target.propertyName,
                    targetProperty = this.isSource ? target.propertyName : this.propertyName,
                    defaultValue = null
                };
                AddPropertyLink(newPropertyLink);
            }
            else
            {
                Debug.LogWarning("Property mapping failed due to type mismatch.");
            }
        }

        public void Initialize(string propertyName, ShaderUtil.ShaderPropertyType propertyType)
        {
            this.propertyName = propertyName;
            this.propertyType = propertyType;

            GetElements();
            InitializeElements();
        }

        private void InitializeElements()
        {
            _propertyNameLabel.text = propertyName;
            _propertyTypeLabel.text = propertyType.ToString();
        }

        private void GetElements()
        {
            _propertyTypeLabel = this.Q<Label>("PropertyTypeText");
            _propertyNameLabel = this.Q<Label>("PropertyNameText");
        }
    }
}