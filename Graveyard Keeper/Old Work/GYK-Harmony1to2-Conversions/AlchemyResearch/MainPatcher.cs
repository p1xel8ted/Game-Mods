using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AlchemyResearch
{
    public static class MainPatcher
    {
        public const string KnownRecipesFilePathAndName = "./QMods/AlchemyResearch/Known Recipes.txt";
        public const string ParameterComment = "#";
        public const string ParameterSectionBegin = "[";
        public const string ParameterSectionEnd = "]";
        public const char ParameterSeparator = '|';
        public static string ResultPreviewText = "Result";
        private const string ConfigFilePathAndName = "./QMods/AlchemyResearch/config.txt";

        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.graveyardkeeper.urbanvibes.alchemyresearch");
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Reflection.Initialization();
                ReadParametersFromFile();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AlchemyResearch (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }

        private static void ReadParametersFromFile()
        {
            string[] strArray1;
            try
            {
                strArray1 = File.ReadAllLines(ConfigFilePathAndName);
            }
            catch (Exception)
            {
                return;
            }
            if (strArray1.Length == 0)
                return;
            foreach (var str1 in strArray1)
            {
                var str2 = str1.Trim();
                if (string.IsNullOrEmpty(str2) || str2.StartsWith(ParameterComment)) continue;
                var strArray2 = str2.Split(ParameterSeparator);
                if (strArray2.Length < 2 || strArray2[0].Trim() != "ResultPreviewText" ||
                    string.IsNullOrEmpty(strArray2[1].Trim())) continue;
                ResultPreviewText = strArray2[1].Trim();
                break;
            }
        }
    }
}