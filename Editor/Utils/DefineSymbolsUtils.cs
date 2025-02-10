/// Copyright 2025, Antonin Boureau, All rights reserved.
/// Version 20250210

using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Devloader.Utils.EditorOnly
{
    public class DefineSymbolsUtils
    {
        const char SymbolSeparator = ';';

        List<string> _symbols = new List<string>();
        public string[] symbols => _symbols.ToArray();

        BuildTargetGroup _group;

        /// <summary>
        /// Create a Symbol Parser using selected build target group to select scripting defines
        /// </summary>
        public DefineSymbolsUtils() : this(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup), EditorUserBuildSettings.selectedBuildTargetGroup) { }

        /// <summary>
        /// Create a Symbol Parser using given build target group to select scripting defines
        /// </summary>
        public DefineSymbolsUtils(BuildTargetGroup group) : this(PlayerSettings.GetScriptingDefineSymbolsForGroup(group), group) { }

        /// <summary>
        /// Create a Symbol Parser using given symbols string
        /// </summary>
        public DefineSymbolsUtils(string symbols, BuildTargetGroup group)
        {
            _symbols = new List<string>(symbols.Split(SymbolSeparator));
            _group = group;
        }

        public void Add(string symbol) => _symbols.Add(symbol);

        public bool Check(string symbol) => _symbols.Contains(symbol);

        public bool Remove(string symbol) => _symbols.Remove(symbol);

        public void Replace(string oldSymbol, string newSymbol)
        {
            Remove(oldSymbol);
            Validate(newSymbol);
        }

        public void Save() => PlayerSettings.SetScriptingDefineSymbolsForGroup(_group, string.Join(SymbolSeparator, symbols));

        public void Validate(string symbol)
        {
            if (!Check(symbol))
                Add(symbol);
        }
    }
}