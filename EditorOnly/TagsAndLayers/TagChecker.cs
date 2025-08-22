/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250822

using UnityEngine;

#if UNITY_EDITOR
using Devloader.Utils.EditorOnly;
#endif

using System.Collections.Generic;
using System.ComponentModel;

namespace Devloader.EditorOnly.TagsAndLayers
{
    [DisplayName("[Editor Only] Tag Checker")]
    public class TagChecker : MonoBehaviour
    {
        [SerializeField] private string _tagName;

        List<string> protectedTags = new List<string>(new string[7]
        {
            "Untagged",
            "Respawn",
            "Finish",
            "EditorOnly",
            "MainCamera",
            "Player",
            "GameController"
        });

        public void CheckTag()
        {
#if UNITY_EDITOR
            if (!protectedTags.Contains(_tagName))
                TagUtils.Validate(_tagName);
#endif

            gameObject.tag = _tagName;
        }

        public void DeleteTag(string tagName)
        {
#if UNITY_EDITOR
            if (!protectedTags.Contains(tagName))
                TagUtils.Remove(tagName);
#endif
        }

        public string TagName => _tagName;
    }
}