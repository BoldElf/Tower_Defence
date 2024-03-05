using System;
using System.IO;
using UnityEngine;

namespace TowerDefence
{
    [Serializable]
    public class Saver<T>
    {
        public static void TryLoad(string filname, ref T data)
        {
            var path = FileHandler.Path(filname);

            if (File.Exists(path))
            {
                //Debug.Log($"Loading from {path}");
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.data;
            }
            else
            {
                Debug.Log($"No file at {path}");
            }
        }

        public static void Save(string filname, T data)
        {
            var wrapper = new Saver<T> { data = data };
            var dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(filname), dataString);
        }
        public T data;
    }

    public static class FileHandler
    {
        public static string Path(string filname)
        {
            return $"{Application.persistentDataPath}/{filname}";
        }
        public static void Reset(string filname)
        {
            var path = Path(filname);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        internal static bool HasFile(string filname)
        {
            return File.Exists(Path(filname));
        }
    }
        
}