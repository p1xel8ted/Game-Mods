using HarmonyLib;
using System.Reflection;

namespace AlchemyResearch
{
    public static class Reflection
    {
        public static MethodInfo MethodIsCraftAllowed;
        public static MethodInfo MethodGetCraftDefinition;
        public static FieldInfo FieldCurrentPreset;

        public static void Initialization()
        {
            MethodIsCraftAllowed = typeof(MixedCraftGUI).GetMethod("IsCraftAllowed", AccessTools.all);
            MethodGetCraftDefinition = typeof(MixedCraftGUI).GetMethod("GetCraftDefinition", AccessTools.all);
            FieldCurrentPreset = typeof(MixedCraftGUI).GetField("_current_preset", AccessTools.all);
        }
    }
}