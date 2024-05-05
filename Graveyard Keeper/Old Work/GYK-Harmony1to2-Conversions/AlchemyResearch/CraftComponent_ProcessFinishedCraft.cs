using HarmonyLib;
using System;

namespace AlchemyResearch
{
    [HarmonyPatch(typeof(CraftComponent), "ProcessFinishedCraft", new Type[] { })]
    public class CraftComponentProcessFinishedCraft
    {
        [HarmonyPrefix]
        public static void Patch(CraftComponent __instance)
        {
            var str = "empty";
            if (__instance.wgo.data == null || !__instance.wgo.data.id.StartsWith("mf_alchemy_craft_"))
                return;
            if (__instance.current_craft.output.Count > 0)
                str = __instance.current_craft.output[0].id;
            else if (__instance.current_craft.needs.Count > 0)
                str = __instance.current_craft.needs[0].id;
            if (str.Equals("empty") || !AlchemyRecipe.MatchesResult(__instance.wgo.GetInstanceID(), __instance.wgo.obj_id, str) || !AlchemyRecipe.HasValidRecipe())
                return;
            ResearchedAlchemyRecipes.AddCurrentRecipe(str);
        }
    }
}