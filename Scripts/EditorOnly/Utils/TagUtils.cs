/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250821

using UnityEditor;

namespace Devloader.Utils.EditorOnly
{
    public static class TagUtils
    {
        static SerializedObject manager;
        static SerializedProperty prop;

        public static bool Check(string name, out int index)
        {
            Init();
            index = -1;

            // First check if it is not already present
            bool found = false;
            for (int i = 0; i < prop.arraySize && !found; i++)
            {
                found = prop.GetArrayElementAtIndex(i).stringValue.Equals(name);
                index = i;
            }

            return found;
        }

        private static void Init()
        {
            if(manager is null)
                manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
    
            if(prop is null)
                prop = manager.FindProperty("tags");
        }

        public static bool Remove(string tag)
        {
            if (Check(tag, out int index))
            {
                prop.DeleteArrayElementAtIndex(index);
                manager.ApplyModifiedProperties();

                return true;
            }
            else
                return false;
        }

        public static string Validate(string tag)
        {
            if (!Check(tag, out int index))
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