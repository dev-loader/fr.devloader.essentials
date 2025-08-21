/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250821

using Devloader.EditorOnly.TagsAndLayers;
using UnityEditor;

namespace Devloader.CustomInspector
{
    [CustomEditor(typeof(TagChecker))]
    public class TagCheckerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TagChecker myScript = serializedObject.targetObject as TagChecker;
            string previousTag = myScript.TagName;

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();

            if(previousTag != myScript.TagName)
            {
                myScript.DeleteTag(previousTag);
                myScript.CheckTag();
            }
        }
    }
}
