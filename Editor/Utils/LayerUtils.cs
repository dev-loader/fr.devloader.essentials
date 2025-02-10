/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250210

using UnityEditor;

namespace Devloader.Utils.EditorOnly
{
    public static class LayerUtils
    {
        static SerializedObject manager;
        static SerializedProperty prop;

        public static bool Check(string name, out int i)
        {
            Init();

            // First check if it is not already present
            bool found = false;

            for (i = 10; i < 32 && !found; i++)
                found = prop.GetArrayElementAtIndex(i).stringValue.Equals(name);

            return found;
        }

        private static void Init()
        {
            if (manager is null)
                manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            if (prop is null)
                prop = manager.FindProperty("layers");
        }

        public static int Validate(string name)
        {
            int i;

            if (!Check(name, out i))
            {
                bool found = false;

                for (i = 10; i < 32 && !found; i++)
                    found = prop.GetArrayElementAtIndex(i).stringValue.Equals("");

                prop.GetArrayElementAtIndex(i - 1).stringValue = name;
                manager.ApplyModifiedProperties();
            }

            return i-1;
        }
    }
}