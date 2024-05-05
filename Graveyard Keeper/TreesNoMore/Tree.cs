using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TreesNoMore;

[Serializable]
public class Tree
{
    
    [FormerlySerializedAs("Location")] public Vector3 location;
    [FormerlySerializedAs("ObjID")] public string objID;
    
    public Tree(string instanceObjID, Vector3 location)
    {
        objID = instanceObjID;
        this.location = location;
    }
}