using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chipmunk.Modules.StatSystem.Editor
{
    [CustomPropertyDrawer(typeof(StatOverride))]
    public class StatOverrideDrawer : PropertyDrawer
    {
        private const float ToggleWidth = 95f;
        private const float Padding = 4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float line = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            return (line * 2f) + spacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty statProperty = property.FindPropertyRelative("stat");
            SerializedProperty useOverrideProperty = property.FindPropertyRelative("isUseOverride");
            SerializedProperty overrideValueProperty = property.FindPropertyRelative("overrideValue");

            float line = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            Rect firstLineRect = new Rect(position.x, position.y, position.width, line);
            Rect secondLineRect = new Rect(position.x, position.y + line + spacing, position.width, line);

            EditorGUI.BeginProperty(position, label, property);

            Rect contentRect = EditorGUI.PrefixLabel(firstLineRect, label);
            int previousIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect statRect = contentRect;
            statRect.width -= line + Padding;

            Rect iconRect = new Rect(statRect.xMax + Padding, contentRect.y, line, line);

            EditorGUI.PropertyField(statRect, statProperty, GUIContent.none);
            DrawIcon(iconRect, statProperty.objectReferenceValue);

            Rect toggleRect = new Rect(secondLineRect.x, secondLineRect.y, ToggleWidth, secondLineRect.height);
            Rect valueRect = new Rect(
                toggleRect.xMax + Padding,
                secondLineRect.y,
                secondLineRect.width - toggleRect.width - Padding,
                secondLineRect.height);

            EditorGUI.PropertyField(toggleRect, useOverrideProperty, new GUIContent("Override"));
            using (new EditorGUI.DisabledScope(!useOverrideProperty.boolValue))
            {
                EditorGUI.PropertyField(valueRect, overrideValueProperty, new GUIContent("Value"));
            }

            EditorGUI.indentLevel = previousIndent;
            EditorGUI.EndProperty();
        }

        private static void DrawIcon(Rect iconRect, Object statObject)
        {
            Texture texture = TryGetStatIconTexture(statObject);
            if (texture == null)
            {
                EditorGUI.DrawRect(iconRect, new Color(0f, 0f, 0f, 0.08f));
                EditorGUI.LabelField(iconRect, "-", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            EditorGUI.DrawPreviewTexture(iconRect, texture, null, ScaleMode.ScaleToFit);
        }

        private static Texture TryGetStatIconTexture(Object statObject)
        {
            if (statObject == null)
            {
                return null;
            }

            SerializedObject statSerializedObject = new SerializedObject(statObject);
            SerializedProperty iconProperty = statSerializedObject.FindProperty("icon");
            Sprite iconSprite = iconProperty?.objectReferenceValue as Sprite;
            return iconSprite != null ? iconSprite.texture : null;
        }
    }

    [CustomEditor(typeof(StatOverrideBehavior))]
    [CanEditMultipleObjects]
    public class StatOverrideBehaviorEditor : UnityEditor.Editor
    {
        private const float PreviewIconSize = 24f;

        private readonly HashSet<string> duplicatedStatNames = new();

        private SerializedProperty statOverridesProperty;

        private void OnEnable()
        {
            statOverridesProperty = serializedObject.FindProperty("statOverrides");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptField();
            DrawOverviewSection();

            EditorGUILayout.Space(4f);
            EditorGUILayout.PropertyField(statOverridesProperty, includeChildren: true);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptField()
        {
            if (target is not MonoBehaviour targetBehavior)
            {
                return;
            }

            using (new EditorGUI.DisabledScope(true))
            {
                MonoScript script = MonoScript.FromMonoBehaviour(targetBehavior);
                EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            }
        }

        private void DrawOverviewSection()
        {
            EditorGUILayout.LabelField("Stat Overview", EditorStyles.boldLabel);

            if (statOverridesProperty == null)
            {
                EditorGUILayout.HelpBox("Failed to find statOverrides property.", MessageType.Error);
                return;
            }

            if (statOverridesProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No stat overrides configured.", MessageType.Info);
                return;
            }

            bool hasMissingStatReference = false;
            duplicatedStatNames.Clear();
            HashSet<string> uniqueStatNames = new();

            for (int index = 0; index < statOverridesProperty.arraySize; index++)
            {
                SerializedProperty element = statOverridesProperty.GetArrayElementAtIndex(index);
                Object statObject = GetStatObject(element);

                if (statObject == null)
                {
                    hasMissingStatReference = true;
                    continue;
                }

                string statName = GetStatDisplayName(statObject);
                if (!uniqueStatNames.Add(statName))
                {
                    duplicatedStatNames.Add(statName);
                }
            }

            if (hasMissingStatReference)
            {
                EditorGUILayout.HelpBox("Some entries are missing Stat references.", MessageType.Warning);
            }

            if (duplicatedStatNames.Count > 0)
            {
                string duplicateList = string.Join(", ", duplicatedStatNames);
                EditorGUILayout.HelpBox($"Duplicated stat names: {duplicateList}", MessageType.Warning);
            }

            for (int index = 0; index < statOverridesProperty.arraySize; index++)
            {
                DrawOverviewRow(index);
            }
        }

        private void DrawOverviewRow(int index)
        {
            SerializedProperty element = statOverridesProperty.GetArrayElementAtIndex(index);
            Object statObject = GetStatObject(element);
            bool useOverride = element.FindPropertyRelative("isUseOverride")?.boolValue ?? false;
            float overrideValue = element.FindPropertyRelative("overrideValue")?.floatValue ?? 0f;

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                Rect iconRect = GUILayoutUtility.GetRect(
                    PreviewIconSize,
                    PreviewIconSize,
                    GUILayout.Width(PreviewIconSize),
                    GUILayout.Height(PreviewIconSize));

                DrawOverviewIcon(iconRect, statObject);

                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(GetStatDisplayName(statObject), EditorStyles.boldLabel);
                    string stateLabel = useOverride
                        ? $"Override Value: {overrideValue:0.###}"
                        : "Using Stat Base Value";
                    EditorGUILayout.LabelField(stateLabel, EditorStyles.miniLabel);
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label(useOverride ? "OVERRIDE" : "BASE", EditorStyles.miniBoldLabel, GUILayout.Width(70f));
            }
        }

        private static Object GetStatObject(SerializedProperty element)
        {
            return element.FindPropertyRelative("stat")?.objectReferenceValue;
        }

        private static string GetStatDisplayName(Object statObject)
        {
            if (statObject == null)
            {
                return "(None)";
            }

            SerializedObject statSerializedObject = new SerializedObject(statObject);
            SerializedProperty statNameProperty = statSerializedObject.FindProperty("statName");
            string statName = statNameProperty?.stringValue;

            return string.IsNullOrWhiteSpace(statName) ? statObject.name : statName;
        }

        private static void DrawOverviewIcon(Rect iconRect, Object statObject)
        {
            Texture texture = TryGetStatIconTexture(statObject);
            if (texture == null)
            {
                EditorGUI.DrawRect(iconRect, new Color(0f, 0f, 0f, 0.08f));
                EditorGUI.LabelField(iconRect, "-", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            EditorGUI.DrawPreviewTexture(iconRect, texture, null, ScaleMode.ScaleToFit);
        }

        private static Texture TryGetStatIconTexture(Object statObject)
        {
            if (statObject == null)
            {
                return null;
            }

            SerializedObject statSerializedObject = new SerializedObject(statObject);
            SerializedProperty iconProperty = statSerializedObject.FindProperty("icon");
            Sprite iconSprite = iconProperty?.objectReferenceValue as Sprite;
            return iconSprite != null ? iconSprite.texture : null;
        }
    }
}
