using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MaxButton
{
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.QueueEverything")]
    public class MainPatcher
    {
        private const string ConfigFilePathAndName = "./QMods/MaxButton/config.txt";
        private const char ParameterSeparator = '=';

        public static void Patch()
        {
            try
            {
                var harmony = new Harmony("com.graveyardkeeper.urbanvibes.maxbutton");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                ReadParametersFromFile();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MaxButton (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
            }
        }

        private static void ReadParametersFromFile()
        {
            string[] strArray1;
            try
            {
                strArray1 = File.ReadAllLines(ConfigFilePathAndName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MaxButton (Harmony2)]: {ex.Message}, {ex.Source}, {ex.StackTrace}");
                return;
            }
            if (strArray1.Length == 0)
                return;
            foreach (var str1 in strArray1)
            {
                var str2 = str1.Trim();
                if (string.IsNullOrEmpty(str2)) continue;
                var strArray2 = str2.Split(ParameterSeparator);
                if (strArray2.Length < 2 || strArray2[0].Trim() != "MaxButtonName") continue;
                MaxButtonVendor.MaxButtonName = strArray2[1].Trim();
                break;
            }
        }
    }
}