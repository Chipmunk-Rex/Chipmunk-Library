using System;
using UnityEditor;

namespace Chipmunk.Library.MaterialConverter.Editor
{
    [System.Serializable]
    public class PropertyLink
    {
        public ShaderUtil.ShaderPropertyType sourceType;
        public string sourceProperty;
        public string targetProperty;
        public object defaultValue;
    }
}