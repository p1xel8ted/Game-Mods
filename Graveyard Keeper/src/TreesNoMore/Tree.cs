namespace TreesNoMore;

[Serializable]
public class Tree(string instanceObjID, Vector3 location)
{
    
    [FormerlySerializedAs("Location")] public Vector3 location = location;
    [FormerlySerializedAs("ObjID")] public string objID = instanceObjID;

}