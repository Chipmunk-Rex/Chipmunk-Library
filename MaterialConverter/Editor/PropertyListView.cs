using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk_Library.MaterialConverter.Editor
{
    public class PropertyListView : VisualElement
    {
        public PropertyListView(Shader shader, VisualTreeAsset propertyViewVisualTreeAsset)
        {
            CreateProprtyList(shader, propertyViewVisualTreeAsset);
        }

        private void CreateProprtyList(Shader containerSourceShader, VisualTreeAsset _propertyViewVisualTreeAsset)
        {
            this.Clear();
            int propertyCount = ShaderUtil.GetPropertyCount(containerSourceShader);
            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = ShaderUtil.GetPropertyName(containerSourceShader, i);
                ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(containerSourceShader, i);

                PropertyView propertyView = new PropertyView(_propertyViewVisualTreeAsset);
                propertyView.Initialize(propertyName, propertyType);
                this.Add(propertyView);
            }
        }
    }
}