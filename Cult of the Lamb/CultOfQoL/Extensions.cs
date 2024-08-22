namespace CultOfQoL;


public static class Extensions
{
    //get gameobject path
    public static string GetPath(this UnityEngine.GameObject obj)
    {
        return obj.transform.parent == null ? obj.name : obj.transform.parent.gameObject.GetPath() + "/" + obj.name;
    }
}