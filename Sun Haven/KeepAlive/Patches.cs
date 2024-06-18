namespace KeepAlive;

[HarmonyPatch]
[HarmonyPriority(1)]
public static class Patches
{
    private const string BepInEx = "bepinex";
    internal static int Counter;

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(Scene), nameof(Scene.GetRootGameObjects), [])]
    public static void Scene_GetRootGameObjects(ref GameObject[] __result)
    {
        var objectList = new List<GameObject>(__result);
        Counter += objectList.RemoveAll(a => a.name.Contains(BepInEx, StringComparison.InvariantCultureIgnoreCase));
        __result = objectList.ToArray();
    }
}