using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chipmunk.Library.MaterialConverter;
using Chipmunk.Library.MaterialConverter.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class MaterialConverterEditor : EditorWindow
{
    [FormerlySerializedAs("m_VisualTreeAsset")] [SerializeField]
    private VisualTreeAsset visualTreeAsset = default;

    private static string _rootFolder = null;

    ObjectField _mappingContainerField = default;
    
    VisualElement _buttonContainer = default;
    
    ObjectField _sourceShaderField = default;
    ObjectField _targetShaderField = default;

    Button _convertAllButton = default;

    PropertyMappingContainerView _propertyMappingContainerView = default;


    [MenuItem("Tools/Chipmunk/MaterialConverterEditor")]
    public static void OpenEditor()
    {
        MaterialConverterEditor wnd = GetWindow<MaterialConverterEditor>();
        wnd.titleContent = new GUIContent("MaterialConverterEditor");
    }

    public void CreateGUI()
    {
        InitializeRootFolder();
        VisualElement root = rootVisualElement;
        visualTreeAsset.CloneTree(root);

        GetElements(root);
        InitializeElements();
    }


    private void GetElements(VisualElement root)
    {
        _mappingContainerField = root.Q<ObjectField>("PropertyMappingField");

        _propertyMappingContainerView = root.Q<PropertyMappingContainerView>("PropertyMappingContainerView");
        
        _buttonContainer = root.Q<VisualElement>("ButtonContainer");
        
        _sourceShaderField = _propertyMappingContainerView.Q<ObjectField>("SourceShaderField");
        _targetShaderField = _propertyMappingContainerView.Q<ObjectField>("TargetShaderField");

        _convertAllButton = root.Q<Button>("ConvertAllBtn");
    }

    private void InitializeElements()
    {
        _mappingContainerField.RegisterValueChangedCallback(MappingContainerChangedHandler);

        PropertyMappingContainer container = _mappingContainerField.value as PropertyMappingContainer;
        if (container != null)
        {
            _buttonContainer.style.display = DisplayStyle.Flex;
            _propertyMappingContainerView.Initialize(_rootFolder, container);
        }
        else
        {
            _buttonContainer.style.display = DisplayStyle.None;
            _propertyMappingContainerView.Clear();
        }

        _convertAllButton.clicked += ConvertAllMaterial;
    }

    private void ConvertAllMaterial()
    {
        PropertyMappingContainer container = _mappingContainerField.value as PropertyMappingContainer;
        if (container == null)
        {
            Debug.LogError("Selected PropertyMappingContainer is null");
            return;
        }

        IEnumerable<Material> materials = AssetDatabase.FindAssets("t:Material")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Material>)
            .Where(material => material != null);

        int count = 0;
        
        foreach (Material material in materials)
        {
            if(material.shader != container.sourceShader)
                continue;
            
            Dictionary<string, object> propertyValues = new Dictionary<string, object>();
            foreach (var propertyLink in container.PropertyMappings)
            {
                if (string.IsNullOrEmpty(propertyLink.sourceProperty) ||
                    string.IsNullOrEmpty(propertyLink.targetProperty))
                    continue;

                object value = GetPropertyValue(material, propertyLink);
                if (value != null)
                {
                    propertyValues[propertyLink.targetProperty] = value;
                }
            }

            // Change the shader
            material.shader = container.targetShader;

            // Apply stored property values to the material with the new shader
            foreach (var entry in propertyValues)
            {
                ApplyPropertyValue(material, entry.Key, entry.Value);
            }

            EditorUtility.SetDirty(material);
            count++;
            Debug.Log($"Converted {material.name} from {container.sourceShader.name} to {container.targetShader.name}");
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Completed conversion of {count} materials");
    }

    private object GetPropertyValue(Material material, PropertyLink propertyLink)
    {
        if (material == null || propertyLink == null || string.IsNullOrEmpty(propertyLink.sourceProperty))
            return null;

        try
        {
            switch (propertyLink.sourceType)
            {
                case ShaderUtil.ShaderPropertyType.Color:
                    return material.GetColor(propertyLink.sourceProperty);

                case ShaderUtil.ShaderPropertyType.Vector:
                    return material.GetVector(propertyLink.sourceProperty);

                case ShaderUtil.ShaderPropertyType.Float:
                case ShaderUtil.ShaderPropertyType.Range:
                    return material.GetFloat(propertyLink.sourceProperty);

                case ShaderUtil.ShaderPropertyType.TexEnv:
                    return material.GetTexture(propertyLink.sourceProperty);
                case ShaderUtil.ShaderPropertyType.Int:
                    return material.GetInt(propertyLink.sourceProperty);

                default:
                    Debug.LogWarning(
                        $"Unsupported property type: {propertyLink.sourceType} for property {propertyLink.sourceProperty}");
                    return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error getting property {propertyLink.sourceProperty}: {e.Message}");
            return null;
        }
    }

    private void ApplyPropertyValue(Material material, string propertyName, object value)
    {
        if (material == null || string.IsNullOrEmpty(propertyName) || value == null)
            return;

        if (value is Color color)
        {
            material.SetColor(propertyName, color);
        }
        else if (value is Vector4 vector)
        {
            material.SetVector(propertyName, vector);
        }
        else if (value is float floatValue)
        {
            material.SetFloat(propertyName, floatValue);
        }
        else if (value is Texture texture)
        {
            material.SetTexture(propertyName, texture);
        }
        else if (value is int intValue)
        {
            material.SetInt(propertyName, intValue);
        }
        else
        {
            Debug.LogWarning($"Unsupported property type for {propertyName}: {value.GetType()}");
        }
    }

    private void MappingContainerChangedHandler(ChangeEvent<Object> evt)
    {
        InitializeElements();
    }

    private void InitializeRootFolder()
    {
        MonoScript monoScript = MonoScript.FromScriptableObject(this);
        string scriptPath = AssetDatabase.GetAssetPath(monoScript);
        if (_rootFolder == null)
            _rootFolder = Path.GetDirectoryName(Path.GetDirectoryName(scriptPath)).Replace("\\", "/");
        if (visualTreeAsset == null)
        {
            string loadPath = $"{_rootFolder}/Editor/MaterialConverterEditor.uxml";
            visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(loadPath);
            Debug.Assert(visualTreeAsset != null, $"Load Failed : {loadPath}");
        }
    }
}