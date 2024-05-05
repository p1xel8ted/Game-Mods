using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AlchemyResearch
{
    public static class Logg
    {
        private const string Path = "./QMods/AlchemyResearch/Mod Output.txt";

        public static void GameObjectInfo(
          MonoBehaviour monoBehaviour,
          bool showComponents = true,
          bool showChildren = true,
          bool showParents = false,
          int indentation = 0)
        {
            GameObjectInfo(monoBehaviour.gameObject, showComponents, showChildren, showParents, indentation);
        }

        public static void Log(string message)
        {
            if (!File.Exists(Path))
            {
                using var text = File.CreateText(Path);
                text.WriteLine(message);
            }
            else
            {
                using var streamWriter = File.AppendText(Path);
                streamWriter.WriteLine(message);
            }
        }

        public static void LogComponent(Object component)
        {
            Log("Component: " + component);
            var fields = component.GetType().GetFields(AccessTools.all);
            foreach (var t in fields)
                Log(" - Field: " + t.Name + " | Value: " + t.GetValue(component));
        }

        public static void LogObject(object @object)
        {
            Log("Object: " + @object);
            var fields = @object.GetType().GetFields(AccessTools.all);
            foreach (var t in fields)
            {
                var str1 = string.Empty;
                var str2 = string.Empty;
                int num;
                if (t.GetValue(@object) is Array array)
                {
                    num = array.Length;
                    str1 = " | Array Length: " + num;
                }
                if (t.GetValue(@object) is List<object> objectList)
                {
                    num = objectList.Count;
                    str2 = " | List Count: " + num;
                }
                Log(" - Field: " + t.Name + " | Value: " + t.GetValue(@object) + str1 + str2);
            }
        }

        public static void LogRectTransform(RectTransform transform)
        {
            Log("RectTransform " + transform.name + ".sizeDelta: " + transform.sizeDelta);
            Log("RectTransform " + transform.name + ".anchorMin: " + transform.anchorMin);
            Log("RectTransform " + transform.name + ".anchorMax: " + transform.anchorMax);
            Log("RectTransform " + transform.name + ".anchoredPosition: " + transform.anchoredPosition);
            Log("RectTransform " + transform.name + ".anchoredPosition3D: " + transform.anchoredPosition3D);
            Log("RectTransform " + transform.name + ".localPosition: " + transform.localPosition);
            Log("RectTransform " + transform.name + ".pivot: " + transform.pivot);
            Log("RectTransform " + transform.name + ".localScale: " + transform.localScale);
            Log("RectTransform " + transform.name + "-Parent: " + transform.parent.name);
        }

        public static void LogTrace() => Log("Trace: " + Environment.StackTrace);

        public static void LogTransform(GameObject @object) => LogTransform(@object.transform);

        private static void GameObjectInfo(GameObject gameObject, bool showComponents = true, bool showChildren = true, bool showParents = false, int indentation = 0)
        {
            while (true)
            {
                bool activeSelf;
                if (gameObject.transform.parent != null)
                {
                    var strArray = new string[9];
                    strArray[0] = new string(' ', indentation);
                    strArray[1] = "- ";
                    strArray[2] = gameObject.name;
                    strArray[3] = " | active: ";
                    activeSelf = gameObject.activeSelf;
                    strArray[4] = activeSelf.ToString();
                    strArray[5] = " | Parent: ";
                    strArray[6] = gameObject.transform.parent?.ToString();
                    strArray[7] = " | Full Path: ";
                    strArray[8] = GetFullPath(gameObject.transform);
                    Log(string.Concat(strArray));
                }
                else
                {
                    var strArray = new string[6] { new string(' ', indentation), "- ", gameObject.name, " | active: ", null, null };
                    activeSelf = gameObject.activeSelf;
                    strArray[4] = activeSelf.ToString();
                    strArray[5] = " | Parent: <none>";
                    Log(string.Concat(strArray));
                }

                indentation += 2;
                if (showComponents)
                {
                    foreach (var component in gameObject.GetComponents<Object>()) Log(new string(' ', indentation) + "o " + component);
                }

                if (showChildren)
                {
                    for (var index = 0; index < gameObject.transform.childCount; ++index) Log(new string(' ', indentation) + "C " + gameObject.transform.GetChild(index));
                    for (var index = 0; index < gameObject.transform.childCount; ++index) GameObjectInfo(gameObject.transform.GetChild(index).gameObject, showComponents, showChildren, showParents, indentation);
                }

                if (!showParents || !(gameObject.transform.parent != null) || !(bool)gameObject.transform.parent.gameObject) return;
                var strArray1 = new string[5] { gameObject.name, " Parent: ", gameObject.transform.parent.gameObject.name, " | active: ", null };
                activeSelf = gameObject.transform.parent.gameObject.activeSelf;
                strArray1[4] = activeSelf.ToString();
                Log(string.Concat(strArray1));
                gameObject = gameObject.transform.parent.gameObject;
            }
        }

        private static string GetFullPath(Transform transform, Object root = null)
        {
            var fullPath = transform.name;
            for (var parent = transform.parent;
                 parent != null && (!(root != null) || !(parent == root));
                 parent = parent.parent)
                fullPath = parent.name + "/" + fullPath;
            return fullPath;
        }

        private static void LogTransform(Transform transform)
        {
            Log("Transform " + transform.name + ".position: " + transform.position);
            var name1 = transform.name;
            var rotation = transform.rotation;
            var str1 = rotation.eulerAngles.ToString();
            Log("Transform " + name1 + ".rotation: " + str1);
            Log("Transform " + transform.name + ".localScale: " + transform.localScale);
            Log("Transform " + transform.name + ".localPosition: " + transform.localPosition);
            var name2 = transform.name;
            var localRotation = transform.localRotation;
            var str2 = localRotation.eulerAngles.ToString();
            Log("Transform " + name2 + ".localRotation: " + str2);
            var activeSelf = false;
            if ((bool)transform.gameObject)
            {
                var strArray = new string[6]
                {
              "Transform ",
              transform.name,
              "-GameObject: ",
              transform.gameObject?.ToString(),
              " - active: ",
              null
                };
                if (transform.gameObject != null) activeSelf = transform.gameObject.activeSelf;
                strArray[5] = activeSelf.ToString();
                Log(string.Concat(strArray));
            }
            else
                Log("Transform " + transform.name + "-GameObject: <none>");
            if ((bool)transform.gameObject)
            {
                var strArray = new string[6]
                {
              "Transform ",
              transform.name,
              "-Parent: ",
              transform.parent.gameObject?.ToString(),
              " - active: ",
              null
                };
                if (transform.parent.gameObject != null) activeSelf = transform.parent.gameObject.activeSelf;
                strArray[5] = activeSelf.ToString();
                Log(string.Concat(strArray));
            }
            else
                Log("Transform " + transform.name + "-Parent: <none>");
        }
    }
}