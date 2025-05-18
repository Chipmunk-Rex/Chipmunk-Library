using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    [UxmlElement]
    public partial class PropertyMappingContainerView : VisualElement
    {
        private PropertyMappingContainer _container;
        private VisualTreeAsset _propertyLinkVisualTreeAsset;
        private VisualTreeAsset _propertyViewVisualTreeAsset;
        private string _rootPath;

        public void Initialize(string root, PropertyMappingContainer container)
        {
            this._container = container;
            _rootPath = root;

            LoadResource();

            GetElements();
            
            this.Clear();
            CreateObjectField();
            CreatePropertyLinkView();
            CreatePropertyListView();
        }

        private void CreateObjectField()
        {
            ObjectField sourceShaderField = new ObjectField("Source Shader");
            sourceShaderField.objectType = typeof(Shader);
            sourceShaderField.value = _container.sourceShader;
            sourceShaderField.RegisterValueChangedCallback(evt =>
            {
                _container.sourceShader = evt.newValue as Shader;
                CreatePropertyListView();
            });
            this.Add(sourceShaderField);

            ObjectField targetShaderField = new ObjectField("Target Shader");
            targetShaderField.objectType = typeof(Shader);
            targetShaderField.value = _container.targetShader;
            targetShaderField.RegisterValueChangedCallback(evt =>
            {
                _container.targetShader = evt.newValue as Shader;
                CreatePropertyListView();
            });
            this.Add(targetShaderField);
        }

        private void GetElements()
        {
        }

        private void CreatePropertyLinkView()
        {
            if (_propertyLinkVisualTreeAsset == null)
            {
                Debug.LogError("PropertyLinkVisualTreeAsset is null");
                return;
            }

            foreach (var propertyLink in _container.PropertyMappings)
            {
                VisualElement propertyLinkView = new VisualElement(); 
                _propertyLinkVisualTreeAsset.CloneTree(propertyLinkView);
                
                string propertyTypeText = propertyLink.sourceType.ToString() ?? "Unknown";
                propertyLinkView.Q<Label>("PropertyTypeText").text = propertyTypeText;
                propertyLinkView.Q<Label>("SourcePropertyName").text = propertyLink.sourceProperty;
                propertyLinkView.Q<Label>("TargetPropertyName").text = propertyLink.targetProperty;

                this.Add(propertyLinkView);
            }
        }
        private void CreatePropertyListView()
        {
            if (_container == null)
            {
                Debug.LogError("PropertyMappingContainer is null");
                return;
            }
            VisualElement propertyListViewContainer = new VisualElement();
            propertyListViewContainer.name = "PropertyListViewContainer";
            propertyListViewContainer.AddToClassList("PropertyListViewContainer");
            
            PropertyListView sourcePropertyListView = new PropertyListView(_container.sourceShader, _propertyViewVisualTreeAsset);
            sourcePropertyListView.name = "SourcePropertyListView";
            sourcePropertyListView.AddToClassList("PropertyListView");
            propertyListViewContainer.Add(sourcePropertyListView);
            
            VisualElement columnSplitter = new VisualElement();
            columnSplitter.AddToClassList("ColumnSpliter");
            propertyListViewContainer.Add(columnSplitter);
            
            PropertyListView targetPropertyListView = new PropertyListView(_container.targetShader, _propertyViewVisualTreeAsset);
            targetPropertyListView.name = "TargetPropertyListView";
            targetPropertyListView.AddToClassList("PropertyListView");
            propertyListViewContainer.Add(targetPropertyListView);
            
            this.Add(propertyListViewContainer);
        }



        private void LoadResource()
        {
            {
                string loadPath = $"{_rootPath}/Editor/PropertyLinkView.uxml";
                _propertyLinkVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyLinkVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
            {
                string loadPath = $"{_rootPath}/Editor/PropertyView.uxml";
                _propertyViewVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyViewVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
        }
    }
}