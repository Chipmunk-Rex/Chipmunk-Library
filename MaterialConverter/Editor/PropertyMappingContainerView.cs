using System.Runtime.InteropServices;
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
        private VisualTreeAsset _propertyLinkViewContainerVisualTreeAsset;
        private string _rootPath;

        public void Initialize(string root, PropertyMappingContainer container)
        {
            this._container = container;
            _rootPath = root;

            LoadResource();

            this.Clear();

            SplitView splitView = new SplitView();
            CreateObjectField(splitView);

            this.Add(splitView);

            CreatePropertyLinkView(splitView);
            CreatePropertyListView(splitView);
            splitView.fixedPaneInitialDimension = 300;
        }

        private void CreateObjectField(VisualElement root)
        {
            ObjectField sourceShaderField = new ObjectField("Source Shader");
            sourceShaderField.objectType = typeof(Shader);
            sourceShaderField.value = _container.sourceShader;
            sourceShaderField.RegisterValueChangedCallback(evt =>
            {
                _container.sourceShader = evt.newValue as Shader;
                CreatePropertyListView(root);
            });
            this.Add(sourceShaderField);

            ObjectField targetShaderField = new ObjectField("Target Shader");
            targetShaderField.objectType = typeof(Shader);
            targetShaderField.value = _container.targetShader;
            targetShaderField.RegisterValueChangedCallback(evt =>
            {
                _container.targetShader = evt.newValue as Shader;
                CreatePropertyListView(root);
            });
            this.Add(targetShaderField);
        }

        private void CreatePropertyLinkView(VisualElement root = null)
        {
            if (_propertyLinkVisualTreeAsset == null)
            {
                Debug.LogError("PropertyLinkVisualTreeAsset is null");
                return;
            }

            if (root == null)
                root = this.Q<SplitView>();

            VisualElement propertyLinkViewContainer = this.Q<VisualElement>("PropertyLinkViewContainer");
            if (propertyLinkViewContainer == null)
            {
                propertyLinkViewContainer = new VisualElement();
                _propertyLinkViewContainerVisualTreeAsset.CloneTree(propertyLinkViewContainer);
                propertyLinkViewContainer.name = "PropertyLinkViewContainer";
                PropertyDragTargetManipluter propertyDragTargetManipluter =
                    new PropertyDragTargetManipluter(OnPropertyMapped, CanLinkWith);
                propertyLinkViewContainer.AddManipulator(propertyDragTargetManipluter);
                root.Add(propertyLinkViewContainer);
            }

            VisualElement container = propertyLinkViewContainer.Q<VisualElement>("Container");


            container.Clear();

            foreach (var propertyLink in _container.PropertyMappings)
            {
                VisualElement propertyLinkView = new VisualElement();
                _propertyLinkVisualTreeAsset.CloneTree(propertyLinkView);

                string propertyTypeText = propertyLink.sourceType.ToString() ?? "Unknown";
                propertyLinkView.Q<Label>("PropertyTypeText").text = propertyTypeText;
                if (string.IsNullOrEmpty(propertyLink.sourceProperty))
                    CreateDefaultValueField(propertyLinkView, propertyLink);
                else
                    propertyLinkView.Q<Label>("SourcePropertyName").text = propertyLink.sourceProperty;
                propertyLinkView.Q<Label>("TargetPropertyName").text = propertyLink.targetProperty;

                container.Add(propertyLinkView);
            }
        }

        private bool CanLinkWith(PropertyView arg)
        {
            if (arg == null)
                return false;

            if (arg.isSource)
                return false;
            return true;
        }

        private void CreateDefaultValueField(VisualElement propertyLinkView, PropertyLink propertyLink)
        {
            VisualElement sourceProperty = propertyLinkView.Q<VisualElement>("SourceProperty");
            sourceProperty.style.display = DisplayStyle.None;

            VisualElement defaultValueField = propertyLinkView.Q<VisualElement>("DefaultValueField");
            if (defaultValueField == null)
                return;
            defaultValueField.style.display = DisplayStyle.Flex;
            defaultValueField.Clear();

            switch (propertyLink.sourceType)
            {
                case ShaderUtil.ShaderPropertyType.Color:
                {
                    ColorField defaultValueObjectField = new ColorField();
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = (Color)propertyLink.defaultValue;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                case ShaderUtil.ShaderPropertyType.Vector:
                {
                    Vector4Field defaultValueObjectField = new Vector4Field();
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = (Vector4)propertyLink.defaultValue;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                case ShaderUtil.ShaderPropertyType.Float:
                {
                    FloatField defaultValueObjectField = new FloatField();
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = (float)propertyLink.defaultValue;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                case ShaderUtil.ShaderPropertyType.Range:
                {
                    FloatField defaultValueObjectField = new FloatField();
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = (float)propertyLink.defaultValue;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                case ShaderUtil.ShaderPropertyType.TexEnv:
                {
                    ObjectField defaultValueObjectField = new ObjectField();
                    defaultValueObjectField.objectType = typeof(Texture);
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = propertyLink.defaultValue as Texture;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                case ShaderUtil.ShaderPropertyType.Int:
                {
                    IntegerField defaultValueObjectField = new IntegerField();
                    if (propertyLink.defaultValue != null)
                        defaultValueObjectField.value = (int)propertyLink.defaultValue;
                    defaultValueObjectField.RegisterValueChangedCallback(evt =>
                    {
                        propertyLink.defaultValue = evt.newValue;
                        EditorUtility.SetDirty(_container);
                    });
                    defaultValueField.Add(defaultValueObjectField);
                    break;
                }
                default:
                {
                    Debug.LogWarning(
                        $"Unsupported property type: {propertyLink.sourceType} for property {propertyLink.sourceProperty}");
                    break;
                }
            }
        }

        private void OnPropertyMapped(PropertyView obj)
        {
            PropertyLink newPropertyLink = new PropertyLink
            {
                sourceType = obj.propertyType,
                sourceProperty = obj.isSource ? obj.propertyName : null,
                targetProperty = obj.isSource ? null : obj.propertyName,
                defaultValue = null
            };
            AddPropertyLink(newPropertyLink);
        }

        private void CreatePropertyListView(VisualElement root = null)
        {
            if (_container == null)
            {
                Debug.LogError("PropertyMappingContainer is null");
                return;
            }

            if (root == null)
                root = this.Q<SplitView>();

            VisualElement propertyListViewContainer = new VisualElement();
            propertyListViewContainer.name = "PropertyListViewContainer";
            propertyListViewContainer.AddToClassList("PropertyListViewContainer");

            PropertyListView sourcePropertyListView =
                new PropertyListView(_rootPath, _container.sourceShader, AddPropertyLink, true);
            sourcePropertyListView.title.text = "Source Shader";
            sourcePropertyListView.name = "SourcePropertyListView";
            sourcePropertyListView.AddToClassList("PropertyListView");
            propertyListViewContainer.Add(sourcePropertyListView);

            VisualElement columnSplitter = new VisualElement();
            columnSplitter.AddToClassList("ColumnSpliter");
            propertyListViewContainer.Add(columnSplitter);

            PropertyListView targetPropertyListView =
                new PropertyListView(_rootPath, _container.targetShader, AddPropertyLink);
            targetPropertyListView.title.text = "Target Shader";
            targetPropertyListView.name = "TargetPropertyListView";
            targetPropertyListView.AddToClassList("PropertyListView");
            propertyListViewContainer.Add(targetPropertyListView);

            root.Add(propertyListViewContainer);
        }

        private void AddPropertyLink(PropertyLink link)
        {
            _container.PropertyMappings.Add(link);
            EditorUtility.SetDirty(_container);
            CreatePropertyLinkView();
        }


        private void LoadResource()
        {
            {
                string loadPath = $"{_rootPath}/Editor/PropertyLinkView.uxml";
                _propertyLinkVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyLinkVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
            {
                string loadPath = $"{_rootPath}/Editor/PropertyLinkViewContainer.uxml";
                _propertyLinkViewContainerVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
                Debug.Assert(_propertyLinkViewContainerVisualTreeAsset != null, $"Load Failed : {loadPath}");
            }
        }
    }
}