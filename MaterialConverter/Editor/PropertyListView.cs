using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    public class PropertyListView : VisualElement
    {
        private VisualTreeAsset _propertyListViewVisualTreeAsset = null;
        private VisualTreeAsset _propertyViewVisualTreeAsset = null;
        private ScrollView container = null;
        public Label title = null;
        private bool isSource = false;
        public PropertyListView(string rootPath, Shader shader,
            Action<PropertyLink> addPropertyLinkCallback, bool isSource = false)
        {
            this.isSource = isSource;
            LoadResource(rootPath);
            
            _propertyListViewVisualTreeAsset.CloneTree(this);
            
            GetElements();
            CreateProprtyViews(shader, addPropertyLinkCallback);
        }

        private void GetElements()
        {
            title = this.Q<Label>("PropertyListTitle");
            container = this.Q<ScrollView>("PropertyListContainer");
            if (container == null)
            {
                Debug.LogError("PropertyListContainer is null");
                return;
            }
        }

        private void LoadResource(string rootPath)
        {
            {
                string loadPath = $"{rootPath}/Editor/PropertyListView.uxml";
                _propertyListViewVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyListViewVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
            {
                string loadPath = $"{rootPath}/Editor/PropertyView.uxml";
                _propertyViewVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyViewVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
        }

        private void CreateProprtyViews(Shader containerSourceShader,
            Action<PropertyLink> addPropertyLink)
        {
            container.Clear();
            int propertyCount = ShaderUtil.GetPropertyCount(containerSourceShader);
            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = ShaderUtil.GetPropertyName(containerSourceShader, i);
                ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(containerSourceShader, i);

                PropertyView propertyView = new PropertyView(_propertyViewVisualTreeAsset, addPropertyLink, isSource);
                propertyView.Initialize(propertyName, propertyType);
                container.Add(propertyView);
            }
        }
    }
}