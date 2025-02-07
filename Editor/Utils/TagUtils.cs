/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250207

using UnityEditor;

namespace Devloader.EditorOnly.Utils
{
    public class TagUtils
    {
        static SerializedObject manager;
        static SerializedProperty prop;

        public static bool Check(string name)
        {
            Init();

            // First check if it is not already present
            bool found = false;
            for (int i = 0; i < prop.arraySize && !found; i++)
                found = prop.GetArrayElementAtIndex(i).stringValue.Equals(name);

            return found;
        }

        private static void Init()
        {
            if(manager is null)
                manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
    
            if(prop is null)
                prop = manager.FindProperty("tags");
        }

        public static string Validate(string tag)
        {
            if (!Check(tag))
            {
                prop.InsertArrayElementAtIndex(0);
                prop.GetArrayElementAtIndex(0).stringValue = tag;

                // and to save the changes
                manager.ApplyModifiedProperties();
            }

            return tag;
        }
    }
}