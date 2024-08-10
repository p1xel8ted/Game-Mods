namespace AlchemyResearchRedux;


public static class Ext
{
    
    //get transform path
    public static string GetPath(this Transform transform)
    {
        if (transform == null) return string.Empty;
        return transform.parent.GetPath() + "/" + transform.name;
    }
    
}