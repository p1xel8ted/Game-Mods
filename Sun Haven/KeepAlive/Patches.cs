namespace KeepAlive;

[Harmony]
[HarmonyPriority(0)]
public static class Patches
{

    [HarmonyPostfix]
    [HarmonyPriority(0)]
    [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), [])]
    public static void Scene_GetRootGameObjects(ref GameObject[] __result)
    {
        var objectList = new List<GameObject>(__result);
        foreach (var noKill in Plugin.NoKillList)
        {
            objectList.RemoveAll(a => a.name.Contains(noKill, StringComparison.InvariantCultureIgnoreCase));
        }
        __result = objectList.ToArray();
    }
}