using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chipmunk_Library.MaterialConverter
{
    [CreateAssetMenu(fileName = "ShaderPropertyMapper", menuName = "Chipmunk/MaterialConverter/ShaderPropertyMapper")]
    public class PropertyMappingContainer : ScriptableObject
    {
        [SerializeField] public Shader sourceShader;
        [SerializeField] public Shader targetShader;
        [SerializeField] List<PropertyLink> propertyMappings = new List<PropertyLink>();
        public List<PropertyLink> PropertyMappings => propertyMappings;
    }
}