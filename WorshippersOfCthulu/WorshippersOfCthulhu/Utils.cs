namespace WorshippersOfCthulhu;

public static class Utils
{
    
    public static List<T> FindIl2CppType<T>() where T : UnityEngine.Object
    {
        var list = new List<T>();
        list.AddRange(Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>()).Select(obj => obj.TryCast<T>()).Where(o => o != null));
        return list;
    }

    public static void AttachToSceneOnLoaded(System.Action<Scene, LoadSceneMode> action)
    {
        SceneManager.sceneLoaded += action;
    }
}