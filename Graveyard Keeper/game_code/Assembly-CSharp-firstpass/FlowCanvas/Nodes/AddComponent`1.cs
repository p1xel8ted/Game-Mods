// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AddComponent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If 'Try Get Existing' is true, then if there is an existing component of that type already attached to the gameobject, it will be returned instead of adding another instance.")]
[Category("Unity")]
public class AddComponent<T> : CallableFunctionNode<T, GameObject, bool> where T : Component
{
  public override T Invoke(GameObject gameObject, bool tryGetExisting)
  {
    T obj = default (T);
    if ((Object) gameObject != (Object) null)
    {
      if (tryGetExisting)
        obj = gameObject.GetComponent<T>();
      if ((Object) obj == (Object) null)
        obj = gameObject.AddComponent<T>();
    }
    return obj;
  }
}
