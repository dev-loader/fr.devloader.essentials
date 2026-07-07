/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240424

using UnityEngine;

namespace Devloader.Extensions
{
    public static class StringFileExtension
    {
        public static string GetFilePath(this string uri)
        { return Combine(Application.persistentDataPath, uri); }

        public static string GetJpgPath(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".jpg"); }

        public static string GetJsonPath(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".json"); }

        public static string GetMp3Path(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".mp3"); }

        public static string GetPngPath(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".png"); }

        public static string GetPythonPath(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".py"); }

        public static string GetWavPath(this string uri)
        { return Combine(Application.persistentDataPath, uri + ".wav"); }

        public static string Combine(this string path1, string path2)
        {
            if (path1.Length <= 0)
                return path2;
            else if (path2.Length <= 0)
                return path1;
            else if (path1.EndsWith('/') && path2.StartsWith('/'))
                return path1.Substring(0, path1.Length - 1) + path2;
            else if (!path1.EndsWith('/') && !path2.StartsWith('/'))
                return path1 + '/' + path2;
            else
                return path1 + path2;
        }
    }
}