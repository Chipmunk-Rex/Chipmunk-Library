using System;
using UnityEditor;

namespace Chipmunk_Library.MaterialConverter
{
    [System.Serializable]
    public class PropertyLink
    {
        public ShaderUtil.ShaderPropertyType sourceType;
        public string sourceProperty;
        public string targetProperty;
    }
}